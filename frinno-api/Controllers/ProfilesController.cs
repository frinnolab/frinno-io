using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace frinno_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfilesController : ControllerBase
    {
        //Creates a New Profile Resource
        [HttpPost()]
        public ActionResult<ProfileInfoResponse> CreateNew([FromBody] CreateAProfileRequest request)
        {
            var response = new ProfileInfoResponse 
            { 
                Id = 0, Fullname = request.Fullname, 
                Email = request.Email, 
                AddressInfo = new ProfileAddressInfo 
                { Mobile = request.AddressInfo.Mobile, 
                City = request.AddressInfo.City } 
            };
            return Ok(response);
        }

        //Updates a Profile Resource
        [HttpPut("Id")]
        public ActionResult<ProfileInfoResponse> UpdateProfile([FromQuery] UpdateProfileRequest request)
        {
            var response = new ProfileInfoResponse { Id = request.Id, Email = request.Email, Fullname = request.Fullname };
            return Ok(response);
        }

        //Removes Single Profile Resource
        [HttpDelete("Id")]
        public ActionResult<ProfileInfoResponse> DeleteProfile([FromQuery] ProfileInfoRequest query)
        {
            return Ok();
        }

        //Returns a Profile Resource
        [HttpGet("Id")]
        public ActionResult<ProfileInfoResponse> GetSingle([FromQuery] ProfileInfoRequest query)
        {
            var response = new ProfileInfoResponse { Id = query.Id, Fullname = query.Fullname, Email = query.Email };
            return Ok(response);
        }
        
        //Gets All Profiles
        [HttpGet()]
        public ActionResult<DataListResponse<ProfileInfoResponse>> GetAllProfiles([FromQuery] ProfileInfoRequest query)
        {

            var listResponse = new DataListResponse<ProfileInfoResponse>();

            return Ok(listResponse);

        }
    }
}