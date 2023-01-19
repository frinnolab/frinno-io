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
        public AuthController(IAuthService authServices)
        {
            authService = authServices;
        }


        //Register
        [HttpPost("Register")]
        public ActionResult<RegisterResponse> Register(RegisterRequest request)
        {
            var userExists = authService.FindUserByEmail(request.Email);

            if(userExists!=null)
            {
                return BadRequest(new {message = "Profile Already Exists"});
            }

            var userResponse = authService.Register(request);


            if(userResponse!=null)
            {
                return Created(nameof(GetProfile), new {Id = userResponse.Id});
            }
            return Ok();
        }

        //Get Single User/Profile
        [HttpGet("Profile/{Id}")]
        public ActionResult<UserResponse> GetProfile(int profileId)
        {
            var user = authService.FindUserById(profileId);

            if(user == null)
            {
                return NotFound();
            }
            return Ok(new{user});
        }
    }
}