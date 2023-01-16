using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_application.Generics;
using frinno_core.Entities.MockModels;
using frinno_core.Entities.MockModels.MockDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace frinno_api.Controllers
{
    [ApiController]
    [Route("api/Mocks")]
    public class MockApiController : ControllerBase
    {
        private readonly IMockingDataService mockingService;
        private readonly IMockAuthService mockingAuthService;
        public MockApiController(IMockingDataService service, IMockAuthService auth)
        {
            mockingService = service;
            mockingAuthService = auth;
        }


        [Authorize]
        [HttpGet]
        public ActionResult<MockArticle> GetAll()
        {
            var items = mockingService.FindMockAllDatas();
            if(items.Count()>0)
            {
                return Ok( new {items});
            }
            else{
                return NoContent();
            }
        }

        [Authorize()]
        [HttpGet("Id")]
        public ActionResult<MockArticle> GetSingle(int Id)
        {
            var item = mockingService.FindMockDataById(Id);
            if(item == null)
            {
                return NotFound();
            }
            else{
                return Ok(new{item});
            }
        }

        [HttpPost]
        [Authorize()]
        public ActionResult<MockArticle> Create(MockArticle article)
        {
            if(ModelState.IsValid)
            {
                mockingService.AddMockData(article);
                return Created(nameof(GetSingle), new{ Id = article.Id} );
            }
            else
            {
                return BadRequest();
            }
        }

        [Authorize()]
        [HttpPost("AddBulk")]
        public ActionResult<List<object>> AddBulk(List<MockArticle> datas)
        {
            if(ModelState.IsValid)
            {
                datas.ForEach(d=>mockingService.AddMockData(d));

                return Created(nameof(GetAll), new {datas});
            }
            else{
                return BadRequest();
            }
        }

        //Auth Endpoints
        [AllowAnonymous]
        [HttpPost("Auth/Register")]
        public ActionResult<MockRegisterResponse> Register(MockRegisterRequest request)
        {
            var newUser = new MockRegisterResponse();

            if(request is null)
            {
                return BadRequest();
            }
            //Validate User
            var role = MockRoles.MockUser;
            newUser = mockingAuthService.RegisterUser(request, role);

            return Ok(newUser);
        }

        [AllowAnonymous]
        [HttpPost("Auth/Author/Register")]
        public ActionResult<MockRegisterResponse> RegisterAuthor(MockRegisterRequest request)
        {
            var newUser = new MockRegisterResponse();

            if(request is null)
            {
                return BadRequest();
            }
            //Validate User
            var role = MockRoles.MockAuthor;
            newUser = mockingAuthService.RegisterUser(request, role);

            return Ok(newUser);
        }

        [AllowAnonymous]
        [HttpPost("Auth/Login")]
        public ActionResult<MockLoginResponse> Login(MockLoginRequest request)
        {
            var user = mockingAuthService.GetMockUserByEmail(request.Email);

            if(user is null)
            {
                return BadRequest();
            }

            var IsValid  = mockingAuthService.ValidateUserPassword(request,user);

            if(!IsValid)
            {
                return BadRequest("Passwords dont match");
            }

            var loggedUser = mockingAuthService.AuthenticateUser(user);

            return Ok(loggedUser);
        }

        [AllowAnonymous]
        [HttpPost("Auth/{userId}Logout")]
        public ActionResult<string> Logout(int userId)
        {

            return Ok();
        }
        //Mock Users
        [Authorize]
        [HttpGet("/Users")]
        public ActionResult<List<MockUser>> ListUsers()
        {
            var users = mockingAuthService.GetMockUsers();
            if(users is null)
            {
                return NoContent();
            }
            return Ok(users);
        }
    }
}