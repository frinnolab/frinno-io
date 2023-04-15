using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_application.Authentication;
using frinno_core.DTOs;
using frinno_core.Entities.Profiles;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace frinno_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly ITokenService tokenService;
         private readonly UserManager<Profile> userManager;
        public AuthController(IAuthService authServices, ITokenService tokens,UserManager<Profile> userManager_)
        {
            authService = authServices;
            tokenService = tokens;
            userManager = userManager_;
        }


        //Register
        [HttpPost("register")]
        public async Task<ActionResult<CreateAProfileResponse>>  RegisterProfile([FromForm]CreateAProfileRequest request)
        {
            //Validate Model
            if(!ModelState.IsValid)
            {
                return BadRequest("Values cannot be empty.!");
            }
            //Find User
            var Exists = await userManager.FindByEmailAsync(request.Email);
            var userExists = authService.UserExists(request.Email);

            if(userExists)
            {
                return BadRequest("Profile Exists!.");
            }

            //New Profile;
            var newProfile = new Profile()
            {

            };
            try
            {
                var obj = await authService.Register(newProfile);
                //Add to Role
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
                FirstName = newProfile.FirstName,
                Email = newProfile.Email,

            };

            
            return Created("", new {response});
        }

        [HttpPost("login")]
        public IActionResult Login()
        {
            var signedIn = true;
            return Ok(signedIn);
        }
    }
} 