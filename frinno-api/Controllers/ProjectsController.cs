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
        [HttpPost("{ProfileId:int}")]
        public ActionResult<CreateProjectResponse> CreateNew(int ProfileId, [FromBody] CreateProjectRequest request)
        {
            
            if(ProfileId>0)
            {
                var profileExists = profilesService.ProfileExists(new Profile { ID = ProfileId});

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
                Id = newProject.ID,
                Title = newProject.Title,
                ProfileId = newProject.Profile.ID
            };
            return Created("", new {response} );
        }

        //Updates a Project Resource
        [HttpPut("{Id}/{ProfileId:int}")]
        public ActionResult<ProjectInfoResponse> UpdateProject(int Id, int ProfileId, [FromBody] UpdateProjectRequest request)
        {

            if (ProfileId > 0)
            {
                var profileExists = profilesService.ProfileExists(new Profile { ID = ProfileId });

                if (!profileExists)
                {
                    return NotFound($"Profile Not found!.");
                }
            }
            
            var Project = projectsService.FetchSingleById(Id);

            if (Project == null)
            {
                return NotFound($"Project: {Id} NotFound!.");
            }
        
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
                Id = ProjectResponse.ID,
                ProfileId = ProjectResponse.Profile.ID,
                Title = ProjectResponse.Title,
                Description = ProjectResponse.Description,
                Url = ProjectResponse.ProjectUrl,
                SkillsEarned = ProjectResponse.Skills.Count,
                Status = ProjectResponse.Status
            };

            return Created("", response);
        }

        //Removes Single Project Resource
        [HttpDelete("{Id}/{ProfileId:int}")]
        public ActionResult<bool> DeleteProject(int Id, int ProfileId)
        {
            if (ProfileId > 0)
            {
                var profileExists = profilesService.ProfileExists(new Profile { ID = ProfileId });

                if (!profileExists)
                {
                    return NotFound($"Profile Not found!.");
                }
            }
            var data = projectsService.FetchSingleById(Id);

            if (data == null)
            {
                return NotFound("Project Not found");
            }

            projectsService.Remove(data);
            return Ok("Project Delete Success.!");
        }

        //Returns a Project Resource
        [HttpGet("{Id}")]
        public ActionResult<ProjectInfoResponse> GetSingle(int Id, [FromQuery] ProjectInfoRequest query)
        {
            var Project = projectsService.FetchSingleById(Id);
            if (Project == null)
            {
                return NotFound("Project NotFound");
            }

            var response = new ProjectInfoResponse
            {
                Id = Project.ID,
                ProfileId = Project.Profile.ID,
                Description = Project.Description,
                Title = Project.Title,
                SkillsEarned = Project.Skills.Count,
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
            if(query.ProfileId>0)
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
                Id = p.ID,
                ProfileId = p.Profile.ID,
                Title = p.Title,
                Status = p.Status,
                Url = p.ProjectUrl,
                Description = p.Description,
                SkillsEarned = p.Skills.Count
            } ).ToList();
            response.TotalItems = response.Data.Count;
           return Ok( new {response});
        }
    }
}