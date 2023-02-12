using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_application.Profiles;
using frinno_application.Projects;
using frinno_application.Skills;
using frinno_core.DTOs;
using frinno_core.Entities.Profiles;
using frinno_core.Entities.Projects;
using frinno_core.Entities.Skill;
using frinno_core.Entities.Skill.Aggregates;
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
        public SkillsController(
            ISkillsService skills,
            IProfileService<Profile> profiles,
            IProjectsManager<Project> projects
            )
        {
            skillService = skills;
            profileService = profiles;
            projectsService = projects;
        }

        //Create a New Skill
        [HttpPost("{profileId}")]
        public ActionResult<CreateNewSkillResponse> CreateNewSkill(int profileId, CreateNewSkillRequest request)
        {
            var skillProfileId = 0;
            var skillProjectId = 0;
            
            //NewSkill
            var newSkill = new Skill
            {
                Name = request.Name   
            };
            //Find Profile
            var skillProfile = new Profile();
            if(profileId>0)
            {
                skillProfile = profileService.FetchSingleById(profileId);
            }

            if(skillProfile ==null)
            {
                return NotFound("Profile not found.");
            }

            newSkill.Profile = skillProfile;

            //Find Project
            var skillProject = new Project();
            if(request.ProjectId>0)
            {
                skillProject = projectsService.FetchSingleById(request.ProjectId);
            }


            //Add Skill to project
            if(skillProject != null)
            {
                newSkill.Project = skillProject;
            };
            
            var skillTools = new List<SkillTool>();
            
            //Add Skill Tools used
            if(request.SkillTools != null && request.SkillTools.Count >  0)
            {
                foreach (var skItem in request.SkillTools)
                {
                    var item = new SkillTool 
                    {
                        Name = skItem.Name,
                        Usage = skItem.Usage
                    };

                    skillTools.Add(item);
                }
            }

            //Add Tools to Skill
            if(skillTools != null && skillTools.Count>0)
            {
                newSkill.Tools = skillTools;
            };

            var skillResponse = new Skill();

            try
            {
                skillResponse = skillService.AddNew(newSkill);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new{ Message = $"{ex.Message}" });
            }

            //Format Response

            var skillToolsResponse = new List<CreateSkillTools>();

            foreach (var tool in skillResponse.Tools)
            {
                skillToolsResponse.Add(new CreateSkillTools() { ID = tool.ID, Name = tool.Name, Usage = tool.Usage });
            }

            var response = new CreateNewSkillResponse
            {
                ID = newSkill.ID,
                Name = newSkill.Name,
                ProfileId = skillProfileId,
                ProjectId = skillProjectId,
                SkillTools = skillToolsResponse
        
            };
            return Created("", response);
        }

        //Updte Single Skill
        [HttpPut("{Id}/{profileId}")]
        public ActionResult<UpdateSkillResponse> UpdateSkill(UpdateSkillRequest request, int Id, int profileId)
        {
            var skill = skillService.FetchSingleById(Id);
            if(skill == null)
            {
                return NotFound("Resource not found!.");
            }

            if(request==null)
            {
                return BadRequest();
            }

            skill.Name = request.Name;

            var skillResponse = skillService.Update(skill);
            var response = new UpdateSkillResponse 
            {
                ID = skillResponse.ID,
                Name = skillResponse.Name
            };

            return Created(nameof(GetSingleSkill), new { Id = response.ID });
        }
        
        //Remove Skill
        [HttpDelete("Id/{profileId}")]
        public ActionResult<bool> RemoveSkill(int Id, int profileId)
        {
            var skill = skillService.FetchSingleById(Id);
            if(skill == null)
            {
                return NotFound();
            }
            skillService.Remove(skill);
            return NoContent();
        }
        //Get Single Skill
        [HttpGet("{Id}")]
        public ActionResult<SkillInfoResponse> GetSingleSkill(int Id)
        {
            var skillProfileId  = 0;
            var skillProjectId  = 0;
            var skill = skillService.FetchSingleById(Id);

            if(skill == null)
            {
                return NotFound("Resource Not Found.!");
            }

            //Find Profile with this Skill
            var skillProfile = skill.Profile;

            if(skillProfile != null)
            {
                skillProfileId = skillProfile.ID;
            }

            //Find Project with this skill
            var skillProject = skill.Project;

            if(skillProject != null)
            {
                skillProjectId = skillProject.ID;
            }

            //Format response
            var response = new SkillInfoResponse
            {
                ID = skill.ID,
                Name = skill.Name,
                ProfileId = skillProfileId,
                ProjectId = skillProjectId
            };
            return Ok(response);
        }

        //Get All Skills
        [HttpGet()]
        public ActionResult<DataListResponse<SkillInfoResponse>> GetAllSkills(int profileId, int projectId)
        {
            var skills = skillService.FetchAll();

            if(skills == null)
            {
                return NoContent();
            }

            var response = new DataListResponse<SkillInfoResponse>()
            {
                TotalItems = skills.Count(),
            };
            return Ok(skills);
        }

    }




    
}