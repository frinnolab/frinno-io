using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_application.Profiles;
using frinno_core.DTOs;
using frinno_core.Entities;
using frinno_core.Entities.Articles;
using frinno_core.Entities.Profile.Aggregates;
using frinno_core.Entities.Profile.ValueObjects;
using frinno_core.Entities.Profiles;
using frinno_core.Entities.user;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace frinno_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfilesController : ControllerBase
    {
        private readonly UserManager<Profile> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IProfileService<Profile> profileService;
        public ProfilesController(
            IProfileService<Profile> profiles, 
            UserManager<Profile> userManager_,
            RoleManager<IdentityRole> roleManager_)
        {
            profileService = profiles;
            userManager = userManager_;
            roleManager = roleManager_;
        }

        #region New Profile Endpoint: Admin
        //Creates a New Profile Resource
        // [HttpPost(), Authorize(Roles = "Administrator" )]
        // public async Task<ActionResult<CreateAProfileResponse>> CreateNew([FromBody] CreateAProfileRequest request)
        // {
        //     //Todo, Add Profile Specific Validations
        //     var profile = await userManager.FindByEmailAsync(request.Email);

        //     if (profile!=null)
        //     {
        //         return BadRequest($"A Profile with the same email: {request.Email} already exists!");
        //     }

        //     var response = new CreateAProfileResponse
        //     {

        //     };
        //     return Created("", response);
        // }

        #endregion

        //Updates a Profile Resource
        [HttpPut("{Id}"), Authorize()]
        public async Task<ActionResult<CreateAProfileResponse>> UpdateProfile(string Id, [FromForm] UpdateProfileRequest request)
        {
            var profile = await userManager.FindByIdAsync(Id);

            if(profile == null)
            {
                return NotFound($"Profile Not Found. Please Sign Up.");
            }

            profile.FirstName = request.FirstName;
            profile.LastName = request.LastName;
            profile.Email = request.Email;
            profile.Address = new Address
            {
                City = request.AddressInfo.City,
                Mobile = request.AddressInfo.Mobile
            };

            //Check Role Changes
            if(request.Role != profile.Role)
            {
                //Role Changed.
                var role = await roleManager.RoleExistsAsync(Enum.GetName(request.Role));
                if(!role)
                {
                    await roleManager.CreateAsync(new IdentityRole(){Name = Enum.GetName(request.Role)});
                }

                if(profile.Role != AuthRolesEnum.Administrator)
                {
                    switch(request.Role)
                    {
                        case AuthRolesEnum.Administrator:
                            return BadRequest("Cannot Upgrade to Administrator priveledge");

                        case AuthRolesEnum.Author:
                            profile.Role = AuthRolesEnum.Author;
                            break;
                        case AuthRolesEnum.Guest:
                            profile.Role = AuthRolesEnum.Guest;
                            break;
                    }
                }
                await userManager.AddToRoleAsync(profile,Enum.GetName(request.Role));
            }


            var profileResponse = new Profile();
            try
            {
               profileResponse = await profileService.Update(profile);
            }
            catch (System.Exception Ex)
            {
                
                //throw Ex;

                return BadRequest ($"Failed to update Profile with Error: {Ex.Message}");
                
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
                UserName = profileResponse.UserName,
                FirstName = profileResponse.FirstName,
                LastName =profileResponse.LastName,
                Email = profileResponse.Email,
                Role = profileResponse.Role,
                RoleName = Enum.GetName(profileResponse.Role),
            };

            return Created("",response);
        }

        //Removes Single Profile Resource
        [HttpDelete("{Id}")]
        public async Task<ActionResult<bool>> DeleteProfile(string Id)
        {
            var data = await userManager.FindByIdAsync(Id);

            if (data == null)
            {
                return NotFound("Profile Not found");
            }

            profileService.Remove(data);
            return Ok("Profile Delete Success.!");
        }

        //Returns a Profile Resource
        [HttpGet("{Id}")]
        public async Task< ActionResult<ProfileInfoResponse>> GetSingle(string Id)
        {
            var profile = await userManager.FindByIdAsync(Id);
            if (profile == null)
            {
                return NotFound("Profile NotFound");
            }

            var response = new ProfileInfoResponse()
            {
                Id = profile.Id,
                Username = profile.UserName,
                Email = profile.Email,
                Role = Enum.GetName(profile.Role),
                AddressInfo = new ProfileAddressInfo()
                {
                    Mobile = profile.Address.Mobile,
                    City = profile.Address.City
                },
                TotalArticles = profile.ProfileArticles != null ? profile.ProfileArticles.Count : 0, 
                TotalProjects = profile.Projects != null ? profile.Projects.Count : 0, 
                TotalSkills = profile.Skills != null ? profile.Skills.Count : 0, 
                TotalResumes = profile.Resumes != null ? profile.Resumes.Count : 0, 
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
                Email = p.Email,
                Username = $"{p.FirstName} {p.LastName}",
                TotalArticles = p.ProfileArticles != null ? p.ProfileArticles.Count : 0, 
                TotalProjects = p.Projects != null ? p.Projects.Count : 0, 
                TotalSkills = p.Skills != null ? p.Skills.Count : 0, 
                TotalResumes = p.Resumes != null ? p.Resumes.Count : 0, 
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