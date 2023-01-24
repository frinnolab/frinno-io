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
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var newSkill = new Skill
            {
                Name = request.Name,
            };

            //Tools

            var skillTools = new List<SkillTool>();

            request.SkillTools.ForEach((t) =>
            {
                var tool = new SkillTool
                {
                    Name = t.Name,
                    Usage = t.Usage
                };
                skillTools.Add(tool);
            });

            newSkill.Tools = skillTools;

            //Profile
            if (request.ProfileId > 0)
            {
                var skillProfile = profileService.GetSingleById(request.ProfileId);
                if (skillProfile == null)
                {
                    return NotFound("Profile Not Found!.");
                }

                newSkill.Profile = skillProfile;
            }

            //Project
            if (request.ProjectId > 0)
            {
                var skillProject = projectsService.GetSingleById(request.ProjectId);
                if (skillProject == null)
                {
                    return NotFound("Profile Not Found!.");
                }

                newSkill.Project = skillProject;
            }

            var skillResponse = skillService.AddNew(newSkill);

            if (skillResponse == null)
            {
                return BadRequest("Could not Create Skill");
            }

            var response = new CreateNewSkillResponse
            {
                ID = skillResponse.ID,
                // ProfileId = skillResponse.Profile.ID,
                // ProjectId = skillResponse.Project.ID,
            };
            var toolsResponse = skillResponse.Tools.ToList();
            var toolsList = new List<CreateSkillTools>();
            if (toolsResponse != null)
            {
                foreach (var item in toolsResponse)
                {
                    var itemTool = new CreateSkillTools
                    {
                        ID = item.ID,
                        Name = item.Name,
                        Usage = item.Usage
                    };
                    toolsList.Add(itemTool);
                }
            }

            response.SkillTools = toolsList;
            //NewSkill
            return Created(nameof(GetSingleSkill), new {Id = response.ID});
        }


        //Get Single Skill
        [HttpGet("{Id}")]
        public ActionResult<SkillInfoResponse> GetSingleSkill(int Id)
        {
            var skillInfo = skillService.GetSingleById(Id);

            if(skillInfo==null)
            {
                return NotFound("Skill Not Found!.");
            }

            var skillResponse = new SkillInfoResponse 
            {
                ID = skillInfo.ID,
                Name = skillInfo.Name,
                ProfileId = skillInfo.Profile.ID,
                ProjectId = skillInfo.Project.ID,
            };

            var skillTools = new List<CreateSkillTools>();
            var toolsList = skillInfo.Tools.ToList();

            foreach (var item in toolsList)
            {
                var tool = new CreateSkillTools 
                {
                    ID = item.ID,
                    Name = item.Name,
                    Usage = item.Usage
                };

                skillTools.Add(tool);
            }

            skillResponse.SkillTools = skillTools;

            return Ok(skillResponse);
        }

    }




    
}