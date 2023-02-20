using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_application.Profiles;
using frinno_application.Projects;
using frinno_core.DTOs;
using frinno_core.Entities.Profiles;
using frinno_core.Entities.Project.ValueObjects;
using frinno_core.Entities.Projects;
using frinno_core.Entities.user;
using Microsoft.AspNetCore.Mvc;

namespace frinno_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectsManager<Project> projectsService;
        private readonly IProfileService<Profile> profilesService;
        //private readonly I

        public ProjectsController(IProjectsManager<Project> projects, IProfileService<Profile> profiles)
        {
            projectsService = projects;
            profilesService = profiles;
        }
        //Creates a New Project Resource
        [HttpPost("{ProfileId}")]
        public ActionResult<CreateProjectResponse> CreateNew(string ProfileId, [FromBody] CreateProjectRequest request)
        {
            
            if(ProfileId != string.Empty)
            {
                var profileExists = profilesService.ProfileExists(new Profile { Id = ProfileId});

                if(!profileExists)
                {
                    return NotFound($"Profile Not found!.");
                }
            }
            
            var profile = profilesService.FetchSingleById(ProfileId);
            //Todo, Add Project Specific Validations
            var newProject = new Project
            {
                Title = request.Title,
                Description = request.Description,
                ProjectUrl = request.Url, 
                Profile = profile,
                Status = request.Status
            };

            try
            {
                projectsService.AddNew(newProject);

            }
            catch (System.Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

            var response = new CreateProjectResponse
            {
                Id = newProject.Id,
                Title = newProject.Title,
                ProfileId = newProject.Profile.Id
            };
            return Created("", new {response} );
        }

        //Updates a Project Resource
        [HttpPut("{Id}/{ProfileId}")]
        public ActionResult<ProjectInfoResponse> UpdateProject(string Id, string ProfileId, [FromBody] UpdateProjectRequest request)
        {

            if (ProfileId != string.Empty)
            {
                var profileExists = profilesService.ProfileExists(new Profile { Id = ProfileId });

                if (!profileExists)
                {
                    return NotFound($"Profile Not found!.");
                }
            }
            
            var exists = projectsService.Exists(Id);

            if (!exists)
            {
                return NotFound($"Project: {Id} NotFound!.");
            }

            var Project = projectsService.FetchSingleById(Id);
        
            Project.Title = request.Title;
            Project.Description = request.Description;
            Project.ProjectUrl = request.Url;
            Project.Status = request.Status;

            var ProjectResponse = new Project();

            try
            {
                ProjectResponse = projectsService.Update(Project);

            }
            catch (System.Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

            if (ProjectResponse == null)
            {
                return BadRequest("Failed to Update Project!.");
            }

            var response = new ProjectInfoResponse
            {
                Id = ProjectResponse.Id,
                ProfileId = ProjectResponse.Profile.Id,
                Title = ProjectResponse.Title,
                Description = ProjectResponse.Description,
                Url = ProjectResponse.ProjectUrl,
                Status = ProjectResponse.Status
            };

            return Created("", response);
        }

        //Removes Single Project Resource
        [HttpDelete("{Id}/{ProfileId}")]
        public ActionResult<bool> DeleteProject(string Id, string ProfileId)
        {
            if (ProfileId != string.Empty)
            {
                var profileExists = profilesService.ProfileExists(new Profile { Id = ProfileId });

                if (!profileExists)
                {
                    return NotFound($"Profile Not found!.");
                }
            }

            var exists = projectsService.Exists(Id);

            if (!exists)
            {
                return NotFound($"Project: {Id} NotFound!.");
            }
            var data = projectsService.FetchSingleById(Id);

            projectsService.Remove(data);
            return Ok("Project Delete Success.!");
        }

        //Returns a Project Resource
        [HttpGet("{Id}")]
        public ActionResult<ProjectInfoResponse> GetSingle(string Id, [FromQuery] ProjectInfoRequest query)
        {
            var exists = projectsService.Exists(Id);

            if (!exists)
            {
                return NotFound($"Project: {Id} NotFound!.");
            }

            var Project = projectsService.FetchSingleById(Id);

            var response = new ProjectInfoResponse
            {
                Id = Project.Id,
                ProfileId = Project.Profile.Id,
                Description = Project.Description,
                Title = Project.Title,
                Status = Project.Status,
                Url = Project.ProjectUrl
            };
            return Ok(response);
        }

        //Gets All Projects
        [HttpGet()]
        public ActionResult<DataListResponse<ProjectInfoResponse>> GetAllProjects([FromQuery] ProjectInfoRequest query)
        {
            var projects = new List<Project>();;
            if(query.ProfileId != string.Empty)
            {
                projects = projectsService.FetchAllByProfileId(query.ProfileId);
            }
            else{
                projects = projectsService.FetchAll().ToList();
            }

            if (projects == null)
            {
                return NoContent();
            }

            //Format response)
            var response = new DataListResponse<ProjectInfoResponse>();
            response.Data = projects.Select((p)=> new ProjectInfoResponse 
            {
                Id = p.Id,
                ProfileId = p.Profile.Id,
                Title = p.Title,
                Status = p.Status,
                Url = p.ProjectUrl,
                Description = p.Description,
            } ).ToList();
            response.TotalItems = response.Data.Count;
           return Ok( new {response});
        }
    }
}