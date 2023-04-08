using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_application.Authentication;
using frinno_core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace frinno_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly ITokenService tokenService;
        public AuthController(IAuthService authServices, ITokenService tokens)
        {
            authService = authServices;
            tokenService = tokens;
        }


        //Register
        [HttpPost("register")]
        public async Task<ActionResult<CreateAProfileResponse>>  Register(CreateAProfileRequest request)
        {
            //Validate Model
            if(!ModelState.IsValid)
            {
                return BadRequest("Values cannot be empty.!");
            }
            //Find User
            var userExists = authService.UserExists(request.Email);

            if(userExists)
            {
                return BadRequest("Profile Exists!.");
            }

            //New Profile;
            var userInfo = false;
            try
            {
                userInfo = await authService.Register(request);
            }
            catch (System.Exception ex)
            {
                //throw ex;
                return BadRequest(new{ Message = $"Failed to Register user with Error: {ex.Message}" });
            }
            return Ok(new{ Message = $"Profile Ceated: {userInfo}" } );
        }

        [HttpPost("login")]
        public IActionResult Login()
        {
            var signedIn = true;
            return Ok(signedIn);
        }
    }
} 