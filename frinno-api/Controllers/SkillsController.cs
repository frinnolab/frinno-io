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
            //NewSkill
            var newSkill = new Skill
            {
                Name = request.Name   
            };
            //Find Profile
            var skillProfileExists = false;
            var skillProfile = new Profile();
            if(profileId>0)
            {
                skillProfileExists = profileService.ProfileExists(new Profile { ID = profileId });

                if(!skillProfileExists)
                {
                    return NotFound("Profile not found.");
                }

                skillProfile = profileService.FetchSingleById(profileId);

                newSkill.Profile = skillProfile;
            }

            //Find Project
            var skillProjects = new List<Project>();
            if(request.ProjectIds.Length>0)
            {
                foreach (var projectId in request.ProjectIds)
                {
                    if(projectId>0){
                        var activeProject = projectsService.FetchSingleById(projectId);
                        if(activeProject!=null){
                            skillProjects.Add(activeProject);
                        }
                    }
                };
            }

            var skillTools = new List<SkillTool>();
            
            //Add Skill Tools used
            if(request.SkillTools.Count >  0)
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
                //Add Tools to Skill
                newSkill.Tools = skillTools;
            }


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

            var projectIdsResponse = new List<int>();

            foreach (var project in skillResponse.Projects)
            {
                projectIdsResponse.Add(project.ID);
            }
            var response = new CreateNewSkillResponse
            {
                ID = skillResponse.ID,
                Name = skillResponse.Name,
                ProfileId = skillResponse.Profile.ID,
                ProjectIds = projectIdsResponse.ToArray(),
                SkillTools = skillToolsResponse
        
            };
            return Created("", response);
        }

        //Updte Single Skill
        [HttpPut("{Id}/{profileId}")]
        public ActionResult<UpdateSkillResponse> UpdateSkill(UpdateSkillRequest request, int Id, int profileId)
        {
            
            if(request==null)
            {
                return BadRequest();
            }

            var skill = skillService.FetchSingleById(Id);// To Add Where Query

            if(skill == null)
            {
                return NotFound("Skill not found!.");
            }

            //Find Profile
            var skillProfile = new Profile();

            if(profileId>0)
            {
                skillProfile = profileService.FetchSingleById(profileId);
                if(skillProfile ==null)
                {
                    return NotFound("Profile not found.");
                }

                skill.Profile = skillProfile;

            }


            //Skill Projects
            var skillProjects = new List<Project>();

            if (request.ProjectIds.Length > 0)
            {
                foreach (var projectId in request.ProjectIds)
                {
                    if(projectId>0)
                    {
                        var skP = skill.Projects.Find(p=>p.ID ==projectId);
                        //New Updated Project
                        if( skP == null && projectId>0)
                        {
                            var activeProject = projectsService.FetchSingleById(projectId);

                            if (activeProject != null)
                            {
                                skillProjects.Add(activeProject);
                            }
                        }
                    }
                };

                skill.Projects = skillProjects;

            }

            //Skill Tools

            var skillTools = new List<SkillTool>();
            
            //Add Skill Tools used
            if(request.SkillTools != null && request.SkillTools.Count >  0)
            {
                foreach (var skItem in request.SkillTools)
                {
                    //find skillItem
                    if(skItem.ID>0)
                    {
                        var skItemUpdate = skill.Tools.Single(x=>x.ID == skItem.ID);
                        skItemUpdate.Name = skItem.Name;
                        skItemUpdate.Usage = skItem.Usage;
                    }
                    else{
                        var item = new SkillTool 
                        {
                            Name = skItem.Name,
                            Usage = skItem.Usage
                        };
                        skillTools.Add(item);
                    }

                }

                skill.Tools = skillTools;
            }

                //Add Tools to Skill

            //Update Skill
            skill.ID = request.ID;
            skill.Name = request.Name;
            
            var skillResponse = skillService.Update(skill);

            //Format Response

            var skillToolsResponse = new List<CreateSkillTools>();

            foreach (var tool in skillResponse.Tools)
            {
                skillToolsResponse.Add(new CreateSkillTools() { ID = tool.ID, Name = tool.Name, Usage = tool.Usage });
            }

            var projectIdsResponse = new List<int>();

            foreach (var project in skillResponse.Projects)
            {
                projectIdsResponse.Add(project.ID);
            }
            var response = new CreateNewSkillResponse
            {
                ID = skillResponse.ID,
                Name = skillResponse.Name,
                ProfileId = skillResponse.Profile.ID,
                ProjectIds = projectIdsResponse.ToArray(),
                SkillTools = skillToolsResponse
        
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

            // //Find Project with this skill
            // var skillProject = skill.Project;

            // if(skillProject != null)
            // {
            //     skillProjectId = skillProject.ID;
            // }

            //Format response
            var response = new SkillInfoResponse
            {
                ID = skill.ID,
                Name = skill.Name,
                ProfileId = skillProfileId,
                // ProjectId = skillProjectId
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