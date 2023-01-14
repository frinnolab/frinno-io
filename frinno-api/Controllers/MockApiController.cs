using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_application.Generics;
using frinno_core.Entities.MockModels;
using Microsoft.AspNetCore.Mvc;

namespace frinno_api.Controllers
{
    [ApiController]
    [Route("api/Mocks")]
    public class MockApiController : ControllerBase
    {
        private readonly IMockingDataService mockingService;
        public MockApiController(IMockingDataService service)
        {
            mockingService = service;
        }
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

        [HttpPost("Mocks/AddBulk")]
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
    }
}