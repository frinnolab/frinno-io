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
        public ActionResult<ProjectInfoResponse> CreateNew(int ProfileId, [FromBody] CreateProjectRequest request)
        {
            var profile = new Profile();
            if(ProfileId>0)
            {
                profile = profilesService.FetchSingleById(ProfileId);

                if(profile==null)
                {
                    return NotFound($"Profile Not found!.");
                }
            }
            //Todo, Add Project Specific Validations
            var newProject = new Project
            {
                Title = request.Title,
                Description = request.Description,
                ProjectUrl = request.Url,
                Profile = profile
            };

            switch(request.Status)
            {
                case ProjectStatus.Completed:
                    newProject.Status = ProjectStatus.Completed;
                    break;
                case ProjectStatus.Ongoing:
                    newProject.Status = ProjectStatus.Ongoing;
                    break;
                default:
                    newProject.Status = ProjectStatus.NotStarted;
                    break;
            }


            //Add Profile if not null

            var ProjectResponse = new Project();

            try
            {
                ProjectResponse = projectsService.AddNew(newProject);

            }
            catch (System.Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

            if (newProject == null)
            {
                return BadRequest("Failed to Create a Project!.");
            }

            var response = new ProjectInfoResponse
            {
                Id = ProjectResponse.ID,
            };
            return Created(nameof(GetSingle), new { Message = $"Project Created with ID: {response.Id}" });
        }

        //Updates a Project Resource
        [HttpPut("{Id}/{ProfileId:int}")]
        public ActionResult<ProjectInfoResponse> UpdateProject(int Id, [FromBody] UpdateProjectRequest request)
        {
            var Project = projectsService.FetchSingleById(Id);

            if (Project == null)
            {
                return NotFound($"Project: {Id} NotFound!.");
            }

            if (request == null)
            {
                return BadRequest();
            }

        
            Project.Title = request.Title;
            Project.Description = request.Description;
            Project.ProjectUrl = request.Url;
        

            switch(request.Status)
            {
                case ProjectStatus.Completed:
                    Project.Status = ProjectStatus.Completed;
                    break;
                case ProjectStatus.Ongoing:
                    Project.Status = ProjectStatus.Ongoing;
                    break;
                default:
                    Project.Status = ProjectStatus.NotStarted;
                    break;
                
            }

            var ProjectResponse = projectsService.Update(Project);

            return Created(nameof(GetSingle), new { Message = $"Updated {ProjectResponse.ID}" });
        }

        //Removes Single Project Resource
        [HttpDelete("{Id}/{ProfileId:int}")]
        public ActionResult<string> DeleteProject(int Id)
        {
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
                Title = Project.Title,
                SkillsEarned = Project.Skills.Count,
                Status = Project.Status,
                Url = Project.ProjectUrl
            };
            return Ok(response);
        }

        //Gets All Projects
        [HttpGet()]
        public ActionResult<DataListResponse<ProjectInfoResponse>> GetAllProjects(int ProfileId, [FromQuery] ProjectInfoRequest? query)
        {
            var Projects = projectsService.FetchAllByProfileId(ProfileId);
            if (Projects == null)
            {
                return NoContent();
            }

            //Format response
            var ProjectInfos = Projects.ToList();

            var response = new DataListResponse<ProjectInfoResponse>();
            response.Data = ProjectInfos.Select((p)=> new ProjectInfoResponse 
            {
                Id = p.ID,
                Title = p.Title,
                Description = p.Description,
                Status = p.Status,
                SkillsEarned = p.Skills.Count
            } ).ToList();
            response.TotalItems = response.Data.Count;
           return Ok(response);

        }
    }
}