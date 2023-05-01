using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_application.Profiles;
using frinno_application.Projects;
using frinno_application.Skills;
using frinno_core.DTOs;
using frinno_core.Entities.Profiles;
using frinno_core.Entities.Project.ValueObjects;
using frinno_core.Entities.Projects;
using frinno_core.Entities.Skill;
using frinno_core.Entities.Skill.Aggregates;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace frinno_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SkillsController : ControllerBase
    {
        private readonly ISkillsService skillService;
        private readonly IProfileService<Profile> profileService;
        private readonly IProjectsManager<Project> projectsService;
        private readonly UserManager<Profile> userManager;
        public SkillsController(
            ISkillsService skills,
            IProfileService<Profile> profiles,
            IProjectsManager<Project> projects,
            UserManager<Profile> manager
            )
        {
            skillService = skills;
            profileService = profiles;
            projectsService = projects;
            userManager = manager;
        }

        #region To Fix Navigation 

        ////Create a New Skill
        //[HttpPost("{profileId}")]
        //public async Task<ActionResult<CreateNewSkillResponse>> CreateNewSkill(string profileId, CreateNewSkillRequest request)
        //{
        //    var profile = await userManager.FindByIdAsync(profileId);

        //    if(profile == null)
        //    {
        //        return NotFound($"Profile Not found!.");
        //    }

        //    var projectsWithSkill = new List<ProjectSkills>();

        //    if (request.ProjectIds.Length > 0)
        //    {
        //        foreach (var pId in request.ProjectIds)
        //        {
        //            if(pId>0)
        //            {
        //                var project = projectsService.FetchSingleById(pId);

        //                if(project != null)
        //                {
        //                    projectsWithSkill.Add(new ProjectSkills { Project = project});
        //                }
        //            }
        //        }
        //    }

        //    var newSkill = new Skill()
        //    {
        //        Name = request.Name,
        //        Profile = profile
        //    };

        //    newSkill.Projects = projectsWithSkill ?? null;

        //    //Save Skill
        //    try
        //    {
        //        var data = await skillService.AddNew(newSkill);
        //        newSkill = data;
        //    }
        //    catch (Exception Ex)
        //    {

        //        //throw;

        //        return BadRequest($"Failed to Create skill with Error: {Ex.Message}");
        //    }
        //    var response = new CreateNewSkillResponse
        //    {
        //        Name = newSkill.Name,
        //        Id = newSkill.Id,
        //        ProfileId = newSkill.Profile.Id,
        //        ProjectIds = newSkill.Projects.Select((pj=>pj.Project.Id)).ToArray() ?? null
        //    };
        //    return Created("", new {response});
        //}

        ////Updte Single Skill
        //[HttpPut("{Id}/{profileId}")]
        //public async Task<ActionResult<UpdateSkillResponse>> UpdateSkill(int Id, string profileId, UpdateSkillRequest request)
        //{
        //    var profile = await userManager.FindByIdAsync(profileId);

        //    if (profile == null)
        //    {
        //        return NotFound($"Profile Not found!.");
        //    }

        //    //Find Skill
        //    var skill = skillService.FetchSingleById(Id);

        //    if (skill == null)
        //        return NotFound($"Skill Not found!.");

        //    skill.Name = request.Name;
        //    skill.Profile = profile;

        //    //Current Skills

        //    var skillProjects = new List<ProjectSkills>();

        //    if(request.ProjectIds.Length > 0)
        //    {
        //        skillProjects = skill.Projects.ToList();

        //        foreach (var pId in request.ProjectIds) 
        //        {
        //            if(pId>0)
        //            {
        //                var pj = projectsService.FetchSingleById(pId);

        //                if(pj!=null)
        //                {
        //                    //Find in Current List
        //                    var currentPj = skill.Projects.SingleOrDefault((p => p.Id == pj.Id));

        //                    if(currentPj == null)
        //                    {
        //                        skillProjects.Add(new ProjectSkills { Project = pj });
        //                    }
        //                }
        //            }
        //        }

        //        skill.Projects = skillProjects;
        //    }
        //    //Update Skill

        //    try
        //    {
        //        var data = await skillService.Update(skill);
        //        skill = data;
        //    }
        //    catch (Exception Ex)
        //    {

        //        return BadRequest($"Failed to update Skill with Error: {Ex.Message}");
        //    }
        //    var response = new CreateNewSkillResponse
        //    {
        //        Name = skill.Name,
        //        ProfileId = skill.Profile.Id,
        //        ProjectIds = skill.Projects.Select((pj => pj.Project.Id)).ToArray() ?? null
        //    };

        //    return Created("", new { response });
        //}
        
        ////Remove Skill
        //[HttpDelete("{Id}/{profileId}")]
        //public async Task<ActionResult<bool>> RemoveSkill(int Id, string profileId)
        //{
        //    var profile = await userManager.FindByIdAsync(profileId);

        //    if (profile == null)
        //    {
        //        return NotFound($"Profile Not found!.");
        //    }

        //    //Find Skill
        //    var skill = skillService.FetchSingleById(Id);

        //    if (skill == null)
        //        return NotFound($"Skill Not found!.");

        //    //Remove Skill
        //    try
        //    {
        //        skillService.Remove(skill);
        //    }
        //    catch (Exception Ex)
        //    {

        //        BadRequest($"Failed to remove Skill with Error: {Ex.Message}");
        //    }
        //    return NoContent();
        //}
        ////Get Single Skill
        //[HttpGet("{Id}")]
        //public ActionResult<SkillInfoResponse> GetSingleSkill(int Id)
        //{
        //    var skill = skillService.FetchSingleById(Id);

        //    if(skill == null)
        //    {
        //        return NotFound("Resource Not Found.!");
        //    }

        //    //Format response
        //    var response = new SkillInfoResponse
        //    {
        //        Id = skill.Id,
        //        Name = skill.Name,
        //        ProfileId = skill.Profile.Id,
        //        ProjectIds = skill.Projects.Select((p)=>p.Project.Id).ToArray()
        //    };
        //    return Ok(response);
        //}

        ////Get All Skills
        //[HttpGet()]
        //public async Task<ActionResult<DataListResponse<SkillInfoResponse>>> GetAllSkills(string profileId)
        //{
        //    var skills = new List<Skill>();
        //    if(profileId != string.Empty)
        //    {
        //        skills = skillService.FetchAllByProfileId(profileId).ToList();
        //    }
        //    else
        //    {                
        //        var data = await skillService.FetchAll();
        //        skills = data.ToList();
        //    }

        //    if(skills == null)
        //    {
        //        return NoContent();
        //    }

        //    //Format Response

        //  var response = new DataListResponse<SkillInfoResponse>();
        //    response.Data = skills.Select((p)=> new SkillInfoResponse 
        //    {
        //        Id = p.Id,
        //        ProfileId = p.Profile.Id,
        //        Name = p.Name,
        //        ProjectIds = p.Projects.Select((pj)=>pj.Id).ToArray(),
        //    } ).ToList();
        //    response.TotalItems = response.Data.Count;
        //    return Ok(new{response});
        //}

        #endregion

    }





}