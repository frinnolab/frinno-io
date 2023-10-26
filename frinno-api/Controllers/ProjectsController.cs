using frinno_application.Profiles;
using frinno_application.Projects;
using frinno_core.DTOs;
using frinno_core.Entities.Profiles;
using frinno_core.Entities.Project.ValueObjects;
using frinno_core.Entities.Projects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace frinno_api.Controllers
{
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
                Title = request.Title,
                Description = request.Description,
                ProjectUrl = request.Url,
                Profile = profile,
                //Check Status

                Status = request.Status switch
                {
                    ProjectStatus.Completed => (int)ProjectStatus.Completed,
                    ProjectStatus.Ongoing => (int)ProjectStatus.Ongoing,
                    _ => (int)ProjectStatus.NotStarted,
                }
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
                ProjectUrl = newProject.ProjectUrl
            };
            return Created("", new {response} );
        }

        //Updates a Project Resource
        [HttpPut("{Id}/{profileId}")]
        public async Task<ActionResult<ProjectInfoResponse>> UpdateProject(int Id, string profileId, [FromBody] UpdateProjectRequest request)
        {
            var profile = await userManager.FindByIdAsync(profileId);

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
            project.Title = request.Title;
            project.Description = request.Description;
            project.ProjectUrl = request.Url;
            project.Profile = profile;
            project.Status = (int)request.Status;

            try
            {
                var data = projectsService.FetchSingleById(Id);
                project = data;
            }
            catch (Exception Ex)
            {

                //throw;
                return BadRequest($"Failed to update project with Error: {Ex.Message}");
            }

            var response = new ProjectInfoResponse
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                Url = project.ProjectUrl,
                Status = project.Status
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
                Description = project.Description,
                Url = project.ProjectUrl,
                Status = project.Status
            };
            return Ok(response);
        }

        //Gets All Projects
        [HttpGet(), AllowAnonymous]
        public async Task<ActionResult<DataListResponse<ProjectInfoResponse>>> GetAllProjects([FromQuery] ProjectInfoRequest query)
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

            if (projects == null)
            {
                return NoContent();
            }

            //Format response
            DataListResponse<ProjectInfoResponse>? response = new()
            {
                Data = projects.Select((p) => new ProjectInfoResponse
                {
                    Id = p.Id,
                    Title = p.Title,
                    Url = p.ProjectUrl,
                    Status = p.Status,
                    Description = p.Description,
                }).ToList()
            };
            response.TotalItems = response.Data.Count;
           return Ok( new {response});
        }
    }
}