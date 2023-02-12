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
        [HttpPost()]
        public ActionResult<ProfileInfoResponse> CreateNew([FromBody] CreateAProfileRequest request)
        {
            //Todo, Add Profile Specific Validations
            var exists = profileService
            .ProfileExists(new Profile { User = new User { Email = request.Email } });

            if (exists)
            {
                return BadRequest("Profile Already Exists");
            }
            var newProfile = new Profile
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Address = new Address { City = request.AddressInfo.City, Mobile = request.AddressInfo.Mobile },

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
            var response = new ProfileInfoResponse
            {
                Id = profileResponse.ID,
                AddressInfo = infoAddress,
                Fullname = $"{profileResponse.FirstName} {profileResponse.LastName}",
                Email = profileResponse.User.Email,
                TotalArticles = profileResponse.ProfileArticles.Count,
                TotalProjects = profileResponse.Projects.Count,
                TotalResumes = profileResponse.Resumes.Count
            };
            return Created("", response);
        }

        //Updates a Profile Resource
        [HttpPut("{Id}")]
        public ActionResult<ProfileInfoResponse> UpdateProfile(int Id, [FromBody] UpdateProfileRequest request)
        {
            var profile = profileService.FetchSingleById(Id);

            if (profile == null)
            {
                return NotFound($"Profile: {Id} NotFound!.");
            }

            if (request == null)
            {
                return BadRequest();
            }

            profile.FirstName = request.FirstName;
            profile.LastName = request.LastName;
            profile.User = new User
            {
                Email = request.Email,
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
            var response = new ProfileInfoResponse
            {
                Id = profileResponse.ID,
                AddressInfo = infoAddress,
                Fullname = $"{profileResponse.FirstName} {profileResponse.LastName}",
                Email = profileResponse.User.Email,
                TotalArticles = profileResponse.ProfileArticles.Count,
                TotalProjects = profileResponse.Projects.Count,
                TotalResumes = profileResponse.Resumes.Count
            };

            return Created("",response);
        }

        //Removes Single Profile Resource
        [HttpDelete("{Id}")]
        public ActionResult<string> DeleteProfile(int Id)
        {
            var data = profileService.FetchSingleById(Id);

            if (data == null)
            {
                return NotFound("Profile Not found");
            }

            profileService.Remove(data);
            return Ok("Profile Delete Success.!");
        }

        //Returns a Profile Resource
        [HttpGet("{Id}")]
        public ActionResult<ProfileInfoResponse> GetSingle(int Id, [FromQuery] ProfileInfoRequest query)
        {
            var profile = profileService.FetchSingleById(Id);
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
                Id = profile.ID,
                Fullname = $"{profile.FirstName} {profile.LastName}",
                Email = profile.User.Email,
                TotalArticles = profile.ProfileArticles.Count,
                TotalProjects = profile.Projects.Count,
                TotalResumes = profile.Resumes.Count,
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
                Id = p.ID,
                Email = p.User.Email,
                Fullname = $"{p.FirstName} {p.LastName}",
                TotalArticles = p.ProfileArticles.Count,
                TotalProjects = p.Projects.Count,
                TotalResumes = p.Resumes.Count,
                AddressInfo = new ProfileAddressInfo
                {
                    Mobile = p.Address.Mobile,
                    City = p.Address.City
                }
            } ).ToList();
            response.TotalItems = response.Data.Count;

            return Ok(response);
        }
    }
}