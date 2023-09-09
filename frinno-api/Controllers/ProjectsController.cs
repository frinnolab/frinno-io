using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_application.Profiles;
using frinno_application.Projects;
using frinno_core.DTOs;
using frinno_core.Entities.Profiles;
using frinno_core.Entities.Project;
using frinno_core.Entities.Project.ValueObjects;
using frinno_core.Entities.Projects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace frinno_api.Controllers
{
    /// <summary>
    /// To Encapsulate Logic to Services
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly UserManager<Profile> userManager;
        private readonly IProjectsManager<Project> projectsService;
        private readonly IProfileService<Profile> profilesService;
        //private readonly 
        public ProjectsController(
            IProjectsManager<Project> projects, 
            IProfileService<Profile> profiles,
            UserManager<Profile> userManager_)
        {
            projectsService = projects;
            profilesService = profiles;
            userManager = userManager_;
        }
        //Creates a New Project Resource
        [HttpPost("{profileId}")]
        public async Task<ActionResult<CreateProjectResponse>> CreateNew(string profileId, [FromBody] CreateProjectRequest request)
        {
            var profile = await userManager.FindByIdAsync(profileId);

            if(profile == null)
            {
                return NotFound($"Profile Not found!.");
            }

            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            var newProject = new Project
            {
                Created = DateTime.Now,
                Profile = profile,
                ProjectStart = request.ProjectStart,
                ProjectEnd = request.ProjectEnd,
                Title = request.Title,
                Description = request.Description,
                ProjectUrl = request.ProjectUrl,
                RepositoryUrl = request.RepositoryUrl,
                IsRepoPublic = request.IsRepoPublic,
                ProjectType = request.ProjectType switch
                {
                    ProjectTypeEnum.Graphics => (int)ProjectTypeEnum.Graphics,
                    ProjectTypeEnum.BackEnd => (int)ProjectTypeEnum.BackEnd,
                    ProjectTypeEnum.FrontEnd => (int)ProjectTypeEnum.FrontEnd,
                    ProjectTypeEnum.Mobile => (int)ProjectTypeEnum.Mobile,
                    _ => (int)ProjectTypeEnum.Fullstack,
                },
                Status = request.Status switch
                {
                    ProjectStatus.Deployed => (int)ProjectStatus.Deployed,
                    ProjectStatus.Ongoing => (int)ProjectStatus.Ongoing,
                    _ => (int)ProjectStatus.Planning,
                },
                ClientInfo = new ProjectClientInfo()
                {
                    ClientName = request.ClientInfo.ClientName,
                    ClientMobile = request.ClientInfo.ClientMobile,
                    ClientCity = request.ClientInfo.ClientCity,
                    ClientCountry = request.ClientInfo.ClientCountry,
                    ClientPublicLink = request.ClientInfo.ClientPublicLink,
                },
                CompanyAgencyInfo = new ProjectAgencyInfo()
                {
                    CompanyAgencyName = request.AgencyCompanyInfo.CompanyAgencyName,
                    CompanyAgencyCity = request.AgencyCompanyInfo.CompanyAgencyCity,
                    CompanyAgencyCountry = request.AgencyCompanyInfo.CompanyAgencyCountry,
                    CompanyAgencyMobile = request.AgencyCompanyInfo.CompanyAgencyMobile,
                    CompanyAgencyPublicLink = request.AgencyCompanyInfo.CompanyAgencyPublicLink,
                },
            };
            try
            {
                var data = await projectsService.AddNew(newProject);
                newProject = data;
            }
            catch (System.Exception Ex)
            {
                //throw;
                return BadRequest($"Failed to create a project with Error: {Ex.Message}");
            }
            var response = new CreateProjectResponse
            {
                Title = newProject.Title,
                Id = newProject.Id,
                ProfileId = newProject.Profile.Id,
                ProjectUrl = newProject.ProjectUrl,
                Created = newProject.Created,
                Status = newProject.Status switch
                {
                    (int)ProjectStatus.Deployed => ProjectStatus.Deployed,
                    (int)ProjectStatus.Ongoing => ProjectStatus.Ongoing,
                    _ => ProjectStatus.Planning,
                },
                ProjectType = newProject.ProjectType switch
                {
                    (int)ProjectTypeEnum.Graphics => ProjectTypeEnum.Graphics,
                    (int)ProjectTypeEnum.BackEnd => ProjectTypeEnum.BackEnd,
                    (int)ProjectTypeEnum.FrontEnd => ProjectTypeEnum.FrontEnd,
                    (int)ProjectTypeEnum.Mobile => ProjectTypeEnum.Mobile,
                    _ => (int)ProjectTypeEnum.Fullstack,
                },
            };
            return Created("", new {response} );
        }

        //Updates a Project Resource
        [HttpPut("{Id}/{profileId}")]
        public async Task<ActionResult<UpdateProjectResponse>> UpdateProject(int Id, string profileId, [FromBody] UpdateProjectRequest request)
        {
            //var profile = await userManager.FindByIdAsync(profileId);

            var profile = profilesService.FindProfileById(profileId);

            if(profile == null)
            {
                return NotFound($"Profile Not found!.");
            }

            Project? project;
            try
            {
                project = projectsService.FetchSingleById(Id);
            }
            catch (System.Exception Ex)
            {

                //throw;
                return BadRequest($"Failed to fetch project with Error: {Ex.Message}");
            }

            if (project == null)
            {
                return NotFound($"Project Not found!.");
            }

            //Update Project
            project.Modified = DateTime.Now;
            project.ProjectStart = request.ProjectStart;
            project.ProjectEnd = request.ProjectEnd;
            project.Title = request.Title;
            project.Profile = profile;
            project.Description = request.Description;
            project.ProjectUrl = request.ProjectUrl;
            project.RepositoryUrl = request.RepositoryUrl;
            project.IsRepoPublic = request.IsRepoPublic;
            project.ProjectType = request.ProjectType switch
            {
                ProjectTypeEnum.Graphics => (int)ProjectTypeEnum.Graphics,
                ProjectTypeEnum.BackEnd => (int)ProjectTypeEnum.BackEnd,
                ProjectTypeEnum.FrontEnd => (int)ProjectTypeEnum.FrontEnd,
                ProjectTypeEnum.Mobile => (int)ProjectTypeEnum.Mobile,
                _ => (int)ProjectTypeEnum.Fullstack,
            };

            project.Status = request.Status switch
            {
                ProjectStatus.Deployed => (int)ProjectStatus.Deployed,
                ProjectStatus.Ongoing => (int)ProjectStatus.Ongoing,
                _ => (int)ProjectStatus.Planning,
            };

            project.ClientInfo = new ProjectClientInfo()
            {
                ClientName = request.ClientInfo.ClientName,
                ClientMobile = request.ClientInfo.ClientMobile,
                ClientCity= request.ClientInfo.ClientCity,
                ClientCountry = request.ClientInfo.ClientCountry,
                ClientPublicLink = request.ClientInfo.ClientPublicLink,
            };
            project.CompanyAgencyInfo = new ProjectAgencyInfo()
            {
                CompanyAgencyName = request.AgencyCompanyInfo.CompanyAgencyName,
                CompanyAgencyCity = request.AgencyCompanyInfo.CompanyAgencyCity,
                CompanyAgencyCountry = request.AgencyCompanyInfo.CompanyAgencyCountry,
                CompanyAgencyMobile = request.AgencyCompanyInfo.CompanyAgencyMobile,
                CompanyAgencyPublicLink = request.AgencyCompanyInfo.CompanyAgencyPublicLink,
            };
              
            try
            {
                var data = await projectsService.Update(project);
                project = data;
            }
            catch (Exception Ex)
            {

                //throw;
                return BadRequest($"Failed to update project with Error: {Ex.Message}");
            }

            var response = new ProjectCreateResponse
            {
                Id = project.Id,
                Title = project.Title,
                ProjectUrl = project.ProjectUrl,
                Created = project.Created,
                Modified = project.Modified,
                Status = project.Status switch
                {
                    (int)ProjectStatus.Deployed => ProjectStatus.Deployed,
                    (int)ProjectStatus.Ongoing => ProjectStatus.Ongoing,
                    _ => ProjectStatus.Planning,
                }
            };

            return Created("", response);
        }

        //Removes Single Project Resource
        [HttpDelete("{Id}/{profileId}")]
        public async Task<ActionResult<bool>> DeleteProject(int Id, string profileId)
        {
            var profile = await userManager.FindByIdAsync(profileId);

            if(profile == null)
            {
                return NotFound($"Profile Not found!.");
            }

            try
            {
                var project = projectsService.FetchSingleById(Id);

                projectsService.Remove(project);


            }
            catch (Exception Ex)
            {

                //throw;
                return BadRequest($"Failed to remove project with Error: {Ex.Message}");
            }
            
            return NoContent();
        }

        //Returns a Project Resource
        [HttpGet("{Id}"), AllowAnonymous]
        public ActionResult<ProjectInfoResponse> GetSingle(int Id, [FromQuery] ProjectInfoRequest query)
        {
            Project? project;

            try
            {
                project = projectsService.FetchSingleById(Id);

                if(query!=null){

                }
            }
            catch (System.Exception Ex)
            {
                
                //throw;
                return BadRequest($"Failed to fetch project with Error: {Ex.Message}");
            }

            if(project == null)
            {
                return NotFound("Project Not found!.");
            }

            var response = new ProjectInfoResponse
            {
                Id = project.Id,
                Title = project.Title,
                Desctiption = project.Description,
                ProfileId = project.Profile.Id,
                ProjectUrl = project.ProjectUrl,
                RepositoryUrl = project.RepositoryUrl,
                IsRepoPublic = project.IsRepoPublic,
                ClientInfo = project.ClientInfo,
                CompanyAgencyInfo = project.CompanyAgencyInfo,
                Status = project.Status switch
                {
                    (int)ProjectStatus.Deployed => ProjectStatus.Deployed,
                    (int)ProjectStatus.Ongoing => ProjectStatus.Ongoing,
                    _ => ProjectStatus.Planning,
                },
                ProjectType = project.ProjectType switch
                {
                    (int)ProjectTypeEnum.Graphics => ProjectTypeEnum.Graphics,
                    (int)ProjectTypeEnum.BackEnd => ProjectTypeEnum.BackEnd,
                    (int)ProjectTypeEnum.FrontEnd => ProjectTypeEnum.FrontEnd,
                    (int)ProjectTypeEnum.Mobile => ProjectTypeEnum.Mobile,
                    _ => (int)ProjectTypeEnum.Fullstack,
                },

            };
            return Ok(response);
        }

        //Gets All Projects
        [HttpGet(), AllowAnonymous]
        public async Task<ActionResult<DataListResponse<ProjectCreateResponse>>> GetAllProjects([FromQuery] ProjectInfoRequest query)
        {
            var projects = new List<Project>();;
            if(query.ProfileId != string.Empty)
            {
                var profile = await userManager.FindByIdAsync(query.ProfileId);
                if(profile == null)
                {
                    return NotFound($"Profile with Id: {query.ProfileId} is not found!");
                }
                var data = await projectsService.FetchAllByProfileId(profile.Id);
                projects = data.ToList();
            }
            else{
                var data = await projectsService.FetchAll();
                projects = data.ToList();
            }

            DataListResponse<ProjectCreateResponse>? response = new();

            if (projects == null)
            {
                return NoContent();
            }else
            {
                response = new()
                {
                    Data = projects.Select((p) => new ProjectCreateResponse
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Status = p.Status switch
                        {
                            (int)ProjectStatus.Deployed => ProjectStatus.Deployed,
                            (int)ProjectStatus.Ongoing => ProjectStatus.Ongoing,
                            _ => ProjectStatus.Planning,
                        },
                        ProjectType = p.ProjectType switch
                        {
                            (int)ProjectTypeEnum.Graphics => ProjectTypeEnum.Graphics,
                            (int)ProjectTypeEnum.BackEnd => ProjectTypeEnum.BackEnd,
                            (int)ProjectTypeEnum.FrontEnd => ProjectTypeEnum.FrontEnd,
                            (int)ProjectTypeEnum.Mobile => ProjectTypeEnum.Mobile,
                            _ => (int)ProjectTypeEnum.Fullstack,
                        },

                    }).ToList()
                };
            }

            //Format response
            
            response.TotalItems = response.Data.Count;
           return Ok( new {response});
        }
    }
}