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

        //Login
        // [HttpPost("Login")]
        // public ActionResult<LoginResponse> Login(LoginRequest request)
        // {

        //     var userExists = authService.UserExists(request.Email);

        //     if (!userExists)
        //     {
        //         return BadRequest(new { message = "User does not exist.!" });
        //     }

        //     var user = authService.FindUserByEmail(request.Email);

        //     //Validate Password
        //     var isMatched = authService.VerifyPassord(request.Password, user.hashedPassword);

        //     if (!isMatched)
        //     {
        //         return BadRequest(new { message = "Passowrds don't match!" });
        //     }
        //     //Login
        //     var loggedUser = authService.Login(user);
        //     return Ok(new {loggedUser});
        // }


        //Register
        // [HttpPost("Register")]
        // public ActionResult<RegisterResponse> Register([FromBody] RegisterRequest request)
        // {
        //     var userExists = authService.UserExists(request.Email);

        //     if (userExists)
        //     {
        //         return BadRequest(new { message = "Profile Already Exists" });
        //     }

        //     var userResponse = authService.Register(request);


        //     if (userResponse == null)
        //     {
        //         return BadRequest("Failed to create profile");
        //     }
        //     return Created(nameof(GetProfile), new { Id = userResponse.Id });
        // }

        //Get Single User/Profile
        // [HttpGet("Profile/{Id}")]
        // public ActionResult<UserResponse> GetProfile(int Id)
        // {
        //     var user = authService.FindUserById(Id);

        //     if (user == null)
        //     {
        //         return NotFound();
        //     }
        //     return Ok(new { user });
        // }
    }
}