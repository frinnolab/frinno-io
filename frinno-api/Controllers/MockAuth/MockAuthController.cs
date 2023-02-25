using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using frinno_application.Profiles;
using frinno_core.DTOs;
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
            var dummyToken = BCrypt.Net.BCrypt.HashPassword(profile.FirstName+profile.LastName);
            //Create Token
            var apiSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("5HMQ@FbiMTkWu6m5HMQ@FbiMTkWu6m"));
            var credentials = new SigningCredentials(apiSecret, SecurityAlgorithms.HmacSha256);
            var userClaims = new List<Claim>();

            var tokenOptions = new JwtSecurityToken(
                issuer:"Frinno-IO",
                audience:"Frinno-IO",
                expires:DateTime.Now.AddDays(1),
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
        public ActionResult<CreateAProfileResponse> Register(CreateAProfileRequest request)
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
                Email = data.User.Email

            };

            return Created("",response);
        }
    }
}