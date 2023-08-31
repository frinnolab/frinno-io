using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_application.FileAssets;
using frinno_application.Profiles;
using frinno_core.DTOs;
using frinno_core.Entities;
using frinno_core.Entities.Articles;
using frinno_core.Entities.FileAsset;
using frinno_core.Entities.Profile.Aggregates;
using frinno_core.Entities.Profile.ValueObjects;
using frinno_core.Entities.Profiles;
using frinno_core.Entities.user;
using frinno_infrastructure.Repostories;
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
        private readonly IFileAssetService fileService;
        public ProfilesController(
            IProfileService<Profile> profiles, 
            UserManager<Profile> userManager_,
            RoleManager<IdentityRole> roleManager_,
            IFileAssetService fileAssets)
        {
            profileService = profiles;
            userManager = userManager_;
            roleManager = roleManager_;
            fileService = fileAssets;
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
        [Authorize]
        [HttpPut("{Id}")]
        public async Task<ActionResult<CreateAProfileResponse>> UpdateProfile(string Id, [FromBody] UpdateProfileRequest request)
        {
            var profile = await userManager.FindByIdAsync(Id);

            if(profile == null)
            {
                return NotFound($"Profile Not Found. Please Sign Up.");
            }

            profile.FirstName = request.FirstName;
            profile.LastName = request.LastName;
            profile.Email = request.Email;

            if (!string.IsNullOrEmpty(request.Username))
            {
                profile.UserName = request.Username;
            }else
            {
                profile.UserName = $"{request.FirstName[0].ToString().ToUpper()}-{request.LastName}";
            }


            if (!string.IsNullOrEmpty(request.Password))
            {
                profile.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            }
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
                    //await roleManager.CreateAsync(new IdentityRole(){Name = Enum.GetName(request.Role)});
                    return NotFound("Role Not Found!!.");
                }

                if(profile.Role != AuthRolesEnum.Administrator)
                {
                    switch(request.Role)
                    {
                        case AuthRolesEnum.Administrator:
                            return BadRequest("Cannot Upgrade to this Role.");

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


            Profile? profileResponse;
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
        [Authorize]
        [HttpDelete("{Id}")]
        public async Task<ActionResult<bool>> DeleteProfile(string Id)
        {
            var data = await userManager.FindByIdAsync(Id);

            if (data == null)
            {
                return NotFound("Profile Not found");
            }

            profileService.Remove(data);
            return NoContent();
        }

        //Returns a Profile Resource
        //[Authorize()]
        [HttpGet("{Id}"), AllowAnonymous]
        public async Task< ActionResult<ProfileInfoResponse>> GetSingle(string Id)
        {
            var profile = await userManager.FindByIdAsync(Id);
            if(profile == null){
                return NotFound("Profile does not exist!.");
            }



            var profileResponse = profileService.FindProfileById(profile.Id);

            if (profileResponse != null)
            {
                //Format Response
                
                var response = new ProfileInfoResponse()
                {
                    Id = profileResponse.Id,
                    Username = profileResponse.UserName,
                    Email = profileResponse.Email,
                    Role = Enum.GetName(profileResponse.Role),
                    AddressInfo = new ProfileAddressInfo()
                    {
                        Mobile = profileResponse.Address.Mobile,
                        City = profileResponse.Address.City
                    },
                    TotalArticles = profileResponse.ProfileArticles != null ? profileResponse.ProfileArticles.Count : 0, 
                    TotalProjects = profileResponse.Projects != null ? profileResponse.Projects.Count : 0, 
                    TotalSkills = profileResponse.Skills != null ? profileResponse.Skills.Count : 0, 
                    TotalResumes = profileResponse.Resumes != null ? profileResponse.Resumes.Count : 0, 
                };

                return Ok(response);
            }

            return Unauthorized();
        }

        //Gets All Profiles
        [AllowAnonymous]
        [HttpGet()]
        public ActionResult<DataListResponse<ProfileInfoResponse>> GetAllProfiles([FromQuery] ProfileInfoRequest? query)
        {
            var profiles = profileService.FetchAll();
            if (profiles == null)
            {
                return NoContent();
            }

            if(query!=null)
            {
                
            }
            

            //Format response
            var profileInfos = profiles.ToList();

            var response = new DataListResponse<ProfileInfoResponse>
            {
                Data = profileInfos.Select((p) => new ProfileInfoResponse
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
                }).ToList()
            };
            response.TotalItems = response.Data.Count;
            return Ok(response);
        }


        //Profile Image
        //Upload
        [Authorize]
        [HttpPost("{Id}/upload-files")]
        public async Task<ActionResult<FileUploadReponse>> UploadProfileImage([FromForm] FileUploadRequest fileUploadInfo , IFormFile file)
        {

            if(file == null)
            {
                return BadRequest();
            }

            var uploaderProfile = await userManager.FindByIdAsync(fileUploadInfo.ProfileId);

            if(uploaderProfile == null)
            {
                return NotFound("No Profile found!..");
            }

            var newFile = new FileAsset()
            {
                Name = fileUploadInfo.FileName,
                Description = fileUploadInfo.FileDescription,
                FileType = fileUploadInfo.FileType,
                UploadProfile = uploaderProfile
            };

            //Process File to Stream
            using (var fileStream = new MemoryStream())
            {
                file.CopyTo(fileStream);

                newFile.FileData = fileStream.ToArray();
            }

            var data = await fileService.SaveFileAsynyc(newFile);
            var response = data;
            return Ok(new {response});
        }


        //Remove
        // [Authorize]
        // [HttpDelete("{Id}/remove-avatar")]
        // public ActionResult<bool> RemoveProfileImage(string Id)
        // {
        //     return Ok();
        // }
    }
}