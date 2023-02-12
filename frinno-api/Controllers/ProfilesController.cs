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
                User = new User { Email = request.Email, Password = request.Password },
                Address = new Address { City = request.AddressInfo.City, Mobile = request.AddressInfo.Mobile },
                // ProfileArticles = new List<ProfileArticles>(){
                //     new ProfileArticles { Article = new Article { Title = "Test Artile 0", ArticleTags = new List<frinno_core.Entities.Article.Aggregates.ArticleTags>()
                //     {
                //         new frinno_core.Entities.Article.Aggregates.ArticleTags { Tag = new frinno_core.Entities.Tags.Tag { Name = "Test Tag0" } },
                //         new frinno_core.Entities.Article.Aggregates.ArticleTags { Tag = new frinno_core.Entities.Tags.Tag { Name = "Test Tag1" } },
                //         new frinno_core.Entities.Article.Aggregates.ArticleTags { Tag = new frinno_core.Entities.Tags.Tag { Name = "Test Tag2" } },
                //     } }  },
                //     new ProfileArticles { Article = new Article { Title = "Test Artile 1", ArticleTags = new List<frinno_core.Entities.Article.Aggregates.ArticleTags> ()
                //     {
                //                                 new frinno_core.Entities.Article.Aggregates.ArticleTags { Tag = new frinno_core.Entities.Tags.Tag { Name = "Test Tag2" } },
                //     } }  },
                //     new ProfileArticles { Article = new Article { Title = "Test Artile 2", ArticleTags = new List<frinno_core.Entities.Article.Aggregates.ArticleTags> ()
                //     {
                //                                 new frinno_core.Entities.Article.Aggregates.ArticleTags { Tag = new frinno_core.Entities.Tags.Tag { Name = "Test Tag3" } },
                //     } }  },
                //     new ProfileArticles { Article = new Article { Title = "Test Artile 3", ArticleTags = new List<frinno_core.Entities.Article.Aggregates.ArticleTags> ()
                //     {
                //                                 new frinno_core.Entities.Article.Aggregates.ArticleTags { Tag = new frinno_core.Entities.Tags.Tag { Name = "Test Tag4" } },
                //     } }  },
                // }

            };

            var profileResponse = new Profile();

            try
            {
                profileResponse = profileService.AddNew(newProfile);

            }
            catch (System.Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

            if (newProfile == null)
            {
                return BadRequest("Failed to Create a profile!.");
            }

            // var profileProjects = profileResponse.Projects.Select((p)=>p.Profile == profileResponse).ToList();
            // var profileArticles = profileResponse.ProfileArticles.Select((p)=>p.Profile == profileResponse).ToList();
            // var profileResumes = profileResponse.Resumes.Select((p)=>p.Profile == profileResponse).ToList();

            var response = new ProfileInfoResponse
            {
                Id = profileResponse.ID,
            };
            return Created(nameof(GetSingle), new { Message = $"Profile Created with: {response.Id}" });
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

            // var response = new ProfileInfoResponse
            // {
            //     Id = profileResponse.ID,
            //     Fullname = $"{profileResponse.FirstName} {profileResponse.LastName}",
            //     Email = profileResponse.User.Email,
            //     // TotalArticles = profileArticles.Count,
            //     // TotalProjects = profileProjects.Count,
            //     // TotalResumes = profileResumes.Count,
            //     AddressInfo = new ProfileAddressInfo
            //     {
            //         City = profileResponse.Address.City,
            //         Mobile = profileResponse.Address.Mobile
            //     }
            // };

            return Created(nameof(GetSingle), new { Message = $"Updated {profileResponse.ID}" });
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

            var response = new ProfileInfoResponse
            {
                Id = profile.ID,
                Fullname = $"{profile.FirstName} {profile.LastName}",
                Email = profile.User.Email,
                // TotalArticles = profileArticles.Count,
                // TotalProjects = profileProjects.Count,
                // TotalResumes = profileResumes.Count,
                AddressInfo = new ProfileAddressInfo
                {
                    City = profile.Address.City,
                    Mobile = profile.Address.Mobile
                }
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