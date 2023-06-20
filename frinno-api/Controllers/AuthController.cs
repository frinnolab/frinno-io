using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_application.Authentication;
using frinno_core.DTOs;
using frinno_core.Entities.Profile.ValueObjects;
using frinno_core.Entities.Profiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace frinno_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        //private readonly ITokenService tokenService;
         private readonly UserManager<Profile> userManager;
         private readonly RoleManager<IdentityRole> roleManager;
         
        public AuthController(IAuthService authServices,UserManager<Profile> userManager_, RoleManager<IdentityRole> roleManager_)
        {
            authService = authServices;
            userManager = userManager_;
            roleManager = roleManager_;
        }


        //Register
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<CreateAProfileResponse>>  RegisterProfile([FromBody]CreateAProfileRequest request)
        {
            //Validate Model
            if(!ModelState.IsValid)
            {
                return BadRequest("Values cannot be empty.!");
            }
            //Find User
            var Exists = await userManager.FindByEmailAsync(request.Email);
            

            if(Exists!=null)
            {
                return BadRequest("Profile Exists!.");
            }

            //New Profile;
            var newProfile = new Profile()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                UserName = $"{request.FirstName[0].ToString().ToUpper()}-{request.LastName}",
                Address = new Address()
                {
                    City = request.AddressInfo.City,
                    Mobile = request.AddressInfo.Mobile,

                },
                Role = request.Role,
            };
            try
            {
                var obj = await authService.Register(newProfile);
                //Add to Role
                var isRole = await roleManager.RoleExistsAsync(Enum.GetName(request.Role));

                if(!isRole)
                {
                    var newRole = await roleManager.CreateAsync(new IdentityRole(){Name = Enum.GetName(request.Role)});
                }
                
                var roleResult = await userManager.AddToRoleAsync(obj, Enum.GetName(request.Role));
                newProfile = obj;

            }
            catch (System.Exception ex)
            {
                //throw ex;
                return BadRequest(new{ Message = $"Failed to Register user with Error: {ex.Message}" });
            }

            var response = new CreateAProfileResponse 
            {
                Id = newProfile.Id,
                UserName = newProfile.UserName,
                FirstName = newProfile.FirstName,
                LastName = newProfile.LastName,
                Password = newProfile.PasswordHash,
                Email = newProfile.Email,
                Role = newProfile.Role,
                RoleName = $"{Enum.GetName(newProfile.Role)}",
                AddressInfo = new ProfileAddressInfo {
                    City = newProfile.Address.City,
                    Mobile = newProfile.Address.Mobile
                }
            };

            
            return Created("", new {response});
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("Fields can't be empty");
            }

            //find profile
            var profile = await userManager.FindByEmailAsync(request.Email);

            if(profile==null)
            {
                return NotFound("Profile Not Found, Please sign-up");
            }

            //validate Password

            var isMatched = BCrypt.Net.BCrypt.Verify(request.Password, profile.PasswordHash);

            if(!isMatched)
            {
                return BadRequest("Passwords Don't match!. Check password!.");
            }

            //Config Access tokens.
            var token  = authService.GetAuthToken(profile);
    
            var response = new LoginResponse 
            {
                Id = profile.Id,
                Email = profile.Email,
                UserName = profile.UserName,
                Token = token,
                Role = (int)profile.Role,
            };
            return Ok(response);
        }
    }
} 