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
            //NewSkill
            return Ok();
        }


        //Get Single Skill
        [HttpGet("{Id}")]
        public ActionResult<SkillInfoResponse> GetSingleSkill(int Id)
        {
            return Ok();
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

            foreach (var skill in skills)
            {
                var skillInfo = new SkillInfoResponse 
                {
                    ID = skill.ID,
                    Name = skill.Name,
                };

                foreach (var skillItem in skill.Tools)
                {                    
                    var item = new CreateSkillTools
                    {
                        ID = skillItem.ID,
                        Name = skillItem.Name,
                        Usage = skillItem.Usage
                    };

                    skillToolsList.Add(item);
                }
                skillInfo.SkillTools = skillToolsList;
                skillInfosList.Add(skillInfo);

            }

            response.Data = skillInfosList;
            return Ok(new {response});
        }

    }




    
}