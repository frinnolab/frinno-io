using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_application.Profiles;
using frinno_core.DTOs;
using frinno_core.Entities.Profiles;
using Microsoft.AspNetCore.Mvc;

namespace frinno_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfilesController : ControllerBase
    {
        private readonly IProfileService<Profile> profileService;
        public ProfilesController(IProfileService<Profile> profiles)
        {
            profileService = profiles;
        }
        //Creates a New Profile Resource
        [HttpPost()]
        public ActionResult<ProfileInfoResponse> CreateNew([FromBody] CreateAProfileRequest request)
        {
            //Todo, Add Profile Specific Validations

            return Ok();
        }

        //Updates a Profile Resource
        [HttpPut("{Id}")]
        public ActionResult<ProfileInfoResponse> UpdateProfile(int Id, [FromBody] UpdateProfileRequest request)
        {

            return Ok();
        }

        //Removes Single Profile Resource
        [HttpDelete("{Id}")]
        public ActionResult<string> DeleteProfile(int Id)
        {
            return Ok("Delete Success.!");
        }

        //Returns a Profile Resource
        [HttpGet("{Id}")]
        public ActionResult<ProfileInfoResponse> GetSingle(int Id, [FromQuery] ProfileInfoRequest query)
        {
            return Ok();
        }
        
        //Gets All Profiles
        [HttpGet()]
        public ActionResult<DataListResponse<ProfileInfoResponse>> GetAllProfiles([FromQuery] ProfileInfoRequest? query)
        {

            return Ok();

        }
    }
}