using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using frinno_application.Profiles;
using frinno_core.DTOs;
using frinno_core.Entities;
using frinno_core.Entities.Profile.ValueObjects;
using frinno_core.Entities.Profiles;
using frinno_core.Entities.user;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace frinno_api.Controllers.MockAuth
{
    [ApiController]
    [Route("api/[controller]")]
    public class MockAuthController : ControllerBase
    {
        private IProfileService<Profile> profileService;
        public MockAuthController(IProfileService<Profile> profiles)
        {
            profileService = profiles;
        }

        [HttpPost("login")]
        public ActionResult<LoginResponse> Login(LoginRequest request)
        {
            var profile = profileService.FindByEmail(request.Email);

            if(profile==null)
            {
                return NotFound("Login profile Not found.");
            }
            //Verify Password
            var verifiedPassword = BCrypt.Net.BCrypt.Verify(request.Password, profile.User.Password);

            if(!verifiedPassword){
                return BadRequest("Password does not match.!");
            }
            //Create Token
            var apiSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("5HMQ@FbiMTkWu6m5HMQ@FbiMTkWu6m"));
            var credentials = new SigningCredentials(apiSecret, SecurityAlgorithms.HmacSha256);
            var userClaims = new List<Claim>()
            {
                new Claim("ProfileId", $"{profile.Id}"),
                new Claim(ClaimTypes.Role, $"{Enum.GetName(profile.Role)}")
            };

            var tokenExpiry = DateTime.Now.AddDays(1);

            switch (profile.Role)
            {
                case AuthRolesEnum.Administrator:
                tokenExpiry = DateTime.Now.AddDays(14);
                break;
                case AuthRolesEnum.Author:
                tokenExpiry = DateTime.Now.AddDays(7);
                break;
                case AuthRolesEnum.User:
                tokenExpiry = DateTime.Now.AddDays(5);
                break;
                case AuthRolesEnum.Guest:
                tokenExpiry = DateTime.Now.AddDays(1);
                break;
            }

            var tokenOptions = new JwtSecurityToken(
                issuer:"Frinno-IO",
                audience:"Frinno-IO",
                expires:tokenExpiry,
                signingCredentials:credentials,
                claims:userClaims
            );

            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            var response = new LoginResponse()
            {
                Id = profile.Id,
                Email = profile.User.Email,
                Fullname = $"{profile.FirstName} {profile.LastName}",
                Token = token
            };

            return Ok(response);
        }
        [HttpPost("register")]
        public ActionResult<CreateAProfileResponse> Register(CreateAProfileRequest request, AuthRolesEnum role)
        {
            var exists = profileService.ProfileExists(new Profile{User = new User { Email = request.Email }});

            if(exists)
            {
                return BadRequest("Profile already exists.");
            }


            var newProfile = new Profile {
                FirstName = request.FirstName,
                LastName = request.LastName,
            };
            switch (role)
            {
                case AuthRolesEnum.Administrator:
                 newProfile.Role = AuthRolesEnum.Administrator;
                 break;
                case AuthRolesEnum.Author:
                 newProfile.Role = AuthRolesEnum.Author;
                 break;
                case AuthRolesEnum.User:
                 newProfile.Role = AuthRolesEnum.User;
                 break;
                default:
                newProfile.Role = AuthRolesEnum.Guest;
                break;
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var newUser = new User()
            {
                Email = request.Email,
                Password = hashedPassword
            };

            var newAddress = new Address()
            {
                Mobile = request.AddressInfo.Mobile,
                City = request.AddressInfo.City
            };

            newProfile.User = newUser;
            newProfile.Address = newAddress;

            var data = profileService.AddNew(newProfile);

            if(data==null)
            {
                return BadRequest();
            }

            //Format Response
            var infoAddress = new ProfileAddressInfo ()
            {
                Mobile = data.Address.Mobile,
                City = data.Address.City   
            };

            var response = new CreateAProfileResponse()
            {
                FirstName = data.FirstName,
                LastName =data.LastName,
                Id = data.Id,
                AddressInfo = infoAddress,
                Email = data.User.Email,
                //Role = Enum.GetName(data.Role)
            };

            return Created("",response);
        }
    }
}