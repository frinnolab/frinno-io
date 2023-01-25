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
        [HttpPost()]
        public ActionResult<CreateNewSkillResponse> CreateNewSkill(CreateNewSkillRequest request)
        {
            var skillProfileId = 0;
            var skillProjectId = 0;
            //Find Profile
            var skillProfile = new Profile();
            if(request.ProfileId>0)
            {
                skillProfile = profileService.FetchSingleById(request.ProfileId);
            }

            //Find Project
            var skillProject = new Project();
            if(request.ProjectId>0)
            {
                skillProject = projectsService.FetchSingleById(request.ProjectId);
            }

            //NewSkill
            var newSkill = new Skill
            {
                Name = request.Name   
            };

            //Add Skill to profile.
            if(skillProfile!=null)
            {
                newSkill.Profile = skillProfile;
                skillProfileId = skillProfile.ID;
            };

            //Add Skill to project
            if(skillProject != null)
            {
                newSkill.Project = skillProject;
                skillProfileId = skillProject.ID;
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

            try
            {
                var skillResponse = skillService.AddNew(newSkill);
                newSkill = skillResponse;
            }
            catch (System.Exception ex)
            {
                return BadRequest(new{ Message = $"{ex.Message}" });
            }

            var response = new CreateNewSkillResponse
            {
                ID = newSkill.ID,
                Name = newSkill.Name,
                ProfileId = skillProfileId,
                ProjectId = skillProjectId
            };
            return Created(nameof(GetSingleSkill), new { Id = response.ID});
        }

        //Updte Single Skill
        [HttpPut("{Id}")]
        public ActionResult<UpdateSkillResponse> UpdateSkill(UpdateSkillRequest request)
        {
            var skill = skillService.FetchSingleById(request.ID);
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
        [HttpDelete("Id")]
        public ActionResult<bool> RemoveSkill(int Id)
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
        public ActionResult<DataListResponse<SkillInfoResponse>> GetAllSkills()
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

            var skillInfosList = new List<SkillInfoResponse>();
            var skillToolsList = new List<CreateSkillTools>();

            // foreach (var skill in skills)
            // {
            //     var skillInfo = new SkillInfoResponse 
            //     {
            //         ID = skill.ID,
            //         Name = skill.Name,
            //     };

            //     var skillTools = skill.Tools.ToList();
            //     if (skillTools != null)
            //     {
            //         foreach (var skillItem in skillTools)
            //         {
            //             var item = new CreateSkillTools
            //             {
            //                 ID = skillItem.ID,
            //                 Name = skillItem.Name,
            //                 Usage = skillItem.Usage
            //             };

            //             skillToolsList.Add(item);
            //         }
            //     }
               
            //     skillInfo.SkillTools = skillToolsList;
            //     skillInfosList.Add(skillInfo);

            // }

            // response.Data = skillInfosList;
            return Ok(skills);
        }

    }




    
}