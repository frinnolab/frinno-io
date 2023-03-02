using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_application.Profiles;
using frinno_core.DTOs;
using frinno_core.Entities.Articles;
using frinno_core.Entities.Profile.Aggregates;
using frinno_core.Entities.Profile.ValueObjects;
using frinno_core.Entities.Profiles;
using frinno_core.Entities.user;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


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
        [HttpPost(), Authorize]
        public ActionResult<CreateAProfileResponse> CreateNew([FromBody] CreateAProfileRequest request)
        {
            //Todo, Add Profile Specific Validations
            var exists = profileService
            .ProfileExists(new Profile { User = new User { Email = request.Email } });

            if (exists)
            {
                return BadRequest($"A Profile with the same email: {request.Email} already exists!");
            }
            var newProfile = new Profile
            {
                FirstName = request.FirstName,
                LastName = request.LastName,

            };

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var newUser = new User()
            {
                Email = request.Email,
                Password = hashedPassword
            };

            newProfile.User = newUser;

            var newAddress = new Address()
            {
                Mobile = request.AddressInfo.Mobile,
                City = request.AddressInfo.City
            };

            newProfile.Address = newAddress;


            var profileResponse = new Profile();

            try
            {
                profileResponse = profileService.AddNew(newProfile);

            }
            catch (System.Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

            if (profileResponse == null)
            {
                return BadRequest("Failed to Create a profile!.");
            }

            var infoAddress = new ProfileAddressInfo ()
            {
                Mobile = profileResponse.Address.Mobile,
                City = profileResponse.Address.City   
            };
            var response = new CreateAProfileResponse
            {
                Id = profileResponse.Id,
                AddressInfo = infoAddress,
                FirstName = profileResponse.FirstName,
                LastName =profileResponse.LastName,
                Email = profileResponse.User.Email,
            };
            return Created("", response);
        }

        //Updates a Profile Resource
        [HttpPut("{Id}")]
        public ActionResult<CreateAProfileResponse> UpdateProfile(string Id, [FromBody] UpdateProfileRequest request)
        {
            var profileExists = profileService.ProfileExists(new Profile { User =  new User{ Email = request.Email }});

            if(profileExists)
            {
                return BadRequest($"A Profile with the same email: {request.Email} already exists!");
            }

            var profile = profileService.FindById(Id);

            if (profile == null)
            {
                return NotFound($"Profile: {Id} NotFound!.");
            }


            profile.FirstName = request.FirstName;
            profile.LastName = request.LastName;
            profile.User = new User
            {
                Email =  request.Email,
                Password = request.Password
            };
            profile.Address = new Address
            {
                City = request.AddressInfo.City,
                Mobile = request.AddressInfo.Mobile
            };


            var profileResponse = profileService.Update(profile);

            var infoAddress = new ProfileAddressInfo ()
            {
                Mobile = profileResponse.Address.Mobile,
                City = profileResponse.Address.City   
            };
            var response = new CreateAProfileResponse
            {
                Id = profileResponse.Id,
                AddressInfo = infoAddress,
                FirstName = profileResponse.FirstName,
                LastName =profileResponse.LastName,
                Email = profileResponse.User.Email
            };

            return Created("",response);
        }

        //Removes Single Profile Resource
        [HttpDelete("{Id}")]
        public ActionResult<bool> DeleteProfile(string Id)
        {
            var data = profileService.FindById(Id);

            if (data == null)
            {
                return NotFound("Profile Not found");
            }

            profileService.Remove(data);
            return Ok("Profile Delete Success.!");
        }

        //Returns a Profile Resource
        [HttpGet("{Id}")]
        public ActionResult<ProfileInfoResponse> GetSingle(string Id, [FromQuery] ProfileInfoRequest query)
        {
            var profile = profileService.FindById(Id);
            if (profile == null)
            {
                return NotFound("Profile NotFound");
            }

            var infoAddress = new ProfileAddressInfo
            {
                City = profile.Address.City,
                Mobile = profile.Address.Mobile
            };

            var response = new ProfileInfoResponse
            {
                Id = profile.Id,
                Fullname = $"{profile.FirstName} {profile.LastName}",
                Email = profile.User.Email,
                Role = Enum.GetName(profile.Role),
                TotalArticles = profile.ProfileArticles.Count,
                TotalProjects = profile.Projects.Count,
                TotalResumes = profile.Resumes.Count,
                TotalSkills = profile.Skills.Count,
                AddressInfo = infoAddress
            };
            return Ok(response);
        }

        //Gets All Profiles
        [HttpGet()]
        public ActionResult<DataListResponse<ProfileInfoResponse>> GetAllProfiles([FromQuery] ProfileInfoRequest? query)
        {
            var profiles = profileService.FetchAll();
            if (profiles == null)
            {
                return NoContent();
            }

            //Format response
            var profileInfos = profiles.ToList();

            var response = new DataListResponse<ProfileInfoResponse>();
            response.Data = profileInfos.Select((p)=> new ProfileInfoResponse 
            {
                Id = p.Id,
                Email = p.User.Email,
                Fullname = $"{p.FirstName} {p.LastName}",
                TotalArticles = p.ProfileArticles.Count,
                TotalProjects = p.Projects.Count,
                TotalResumes = p.Resumes.Count,
                TotalSkills = p.Skills.Count,
                Role = Enum.GetName(p.Role),
                AddressInfo = new ProfileAddressInfo
                {
                    Mobile = p.Address.Mobile,
                    City = p.Address.City
                }
            } ).ToList();
            response.TotalItems = response.Data.Count;

            return Ok(response);
        }


        //Profile Image
        //Upload
        [HttpPost("{Id}/upload-avatar")]
        public ActionResult<bool> UploadProfileImage(string Id, IFormFile file)
        {

            return Ok();
        }


        //Remove
        [HttpDelete("{Id}/remove-avatar")]
        public ActionResult<bool> RemoveProfileImage(string Id)
        {
            return Ok();
        }
    }
}