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

            //New Profile instance
            var newProfile = new Profile
            {
                FirstName = request.Fullname.Split(" ").ToArray()[0],
                LastName = request.Fullname.Split(" ").ToArray()[1],
                User = new frinno_core.Entities.user.User 
                {
                    Email = request.Email,
                    Password = request.Password,
                },
                Address = new frinno_core.Entities.Profile.ValueObjects.Address
                {
                    Mobile = request.AddressInfo.Mobile,
                    City = request.AddressInfo.City
                }
            };

            var profileResponse = profileService.AddNew(newProfile);

            if(profileResponse == null)
            {
                return BadRequest("Failed to create a new Profile");
            }
            var response = new ProfileInfoResponse 
            { 
                Id = profileResponse.ID, 
                Fullname = $"{profileResponse.FirstName} {profileResponse.LastName}", 
                Email = profileResponse.User.Email, 
                AddressInfo = new ProfileAddressInfo 
                { 
                    Mobile = profileResponse.Address.Mobile,
                    City = profileResponse.Address.City 
                } 
            };
            return Created(nameof(GetSingle), new{Id = response.Id});
        }

        //Updates a Profile Resource
        [HttpPut("{Id}")]
        public ActionResult<ProfileInfoResponse> UpdateProfile(int Id, [FromBody] UpdateProfileRequest request)
        {
            var currentProfile = profileService.GetSingleById(Id);

            if(currentProfile == null)
            {
                return NotFound();
            }

            currentProfile.FirstName = request.Fullname.Split(" ").ToArray()[0];
            currentProfile.LastName = request.Fullname.Split(" ").ToArray()[1];
            currentProfile.User.Email = request.Email;
            currentProfile.User.Password = request.Password;
            currentProfile.Address.City = request.AddressInfo.City;
            currentProfile.Address.Mobile = request.AddressInfo.Mobile;

            var updateProfile = profileService.Update(currentProfile);
            var response = new ProfileInfoResponse 
            { 
                Id = updateProfile.ID, 
                Email = updateProfile.User.Email, 
                Fullname = $"{updateProfile.FirstName} {updateProfile.LastName}",


            };
            return Created(nameof(GetSingle), new { Id = response.Id });
        }

        //Removes Single Profile Resource
        [HttpDelete("{Id}")]
        public ActionResult<string> DeleteProfile(int Id)
        {
            var profileInfo = profileService.GetSingleById(Id);

            if (profileInfo == null)
            {
                return NotFound();
            }

            profileService.Remove(profileInfo.ID);
            return Ok("Delete Success.!");
        }

        //Returns a Profile Resource
        [HttpGet("{Id}")]
        public ActionResult<ProfileInfoResponse> GetSingle(int Id, [FromQuery] ProfileInfoRequest query)
        {
            var profileInfo = profileService.GetSingleById(Id);

            if(profileInfo == null)
            {
                return NotFound();
            }
            var response = new ProfileInfoResponse (){ 
                Id = profileInfo.ID, 
                Fullname = $"{profileInfo.FirstName} {profileInfo.LastName}", 
                Email = profileInfo.User.Email,
                AddressInfo = new ProfileAddressInfo (){
                    City = profileInfo.Address.City,
                    Mobile = profileInfo.Address.Mobile
                },
            };
            return Ok(response);
        }
        
        //Gets All Profiles
        [HttpGet()]
        public ActionResult<DataListResponse<ProfileInfoResponse>> GetAllProfiles([FromQuery] ProfileInfoRequest? query)
        {
            var profiles = profileService.GetAll();

            var listResponse = new DataListResponse<ProfileInfoResponse>();

            if(profiles == null)
            {
                return NoContent();
            }

            return Ok(profiles);

        }
    }
}