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
            var skillProfile = new Profile();
            if(profileId>0)
            {
                var skillProfileExists = profileService.ProfileExists(new Profile { ID = profileId });

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
                        var projectExists = projectsService.Exists(projectId);
                        if(projectExists)
                        {
                            var activeProject = projectsService.FetchSingleById(projectId);
                            skillProjects.Add(activeProject);
                        }
                    }
                };

                newSkill.Projects = skillProjects;
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


            var projectIdsResponse = new List<int>();

            foreach (var project in skillResponse.Projects.ToList())
            {
                projectIdsResponse.Add(project.ID);
            }
            var response = new CreateNewSkillResponse
            {
                ID = skillResponse.ID,
                Name = skillResponse.Name,
                ProfileId = skillResponse.Profile.ID,
                ProjectIds = projectIdsResponse.ToArray(),
        
            };
            return Created("", new {response});
        }

        //Updte Single Skill
        [HttpPut("{Id}/{profileId}")]
        public ActionResult<UpdateSkillResponse> UpdateSkill(UpdateSkillRequest request, int Id, int profileId)
        {
            var skill = new Skill();
            
            //Find Profile
            var skillProfile = new Profile();
            if(profileId>0)
            {
                var skillProfileExists = profileService.ProfileExists(new Profile { ID = profileId });

                if(!skillProfileExists)
                {
                    return NotFound("Profile not found.");
                }

                skillProfile = profileService.FetchSingleById(profileId);

                skill = skillService.FetchSingleByProfileId(Id, skillProfile.ID);
            }else{
                skill = skillService.FetchSingleById(Id);
            }


            if(skill == null)
            {
                return NotFound("Skill not found!.");
            }

            skill.Profile = skillProfile;


            //Skill Projects

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
                            var projectExists = projectsService.Exists(projectId);

                            if(projectExists)
                            {
                                var activeProject = projectsService.FetchSingleById(projectId);
                                skill.Projects.Add(activeProject);
                            }
                        }
                    }
                };

            }

            //Update Skill
            skill.Name = request.Name;            
            var skillResponse = skillService.Update(skill);

            //Format Response

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
                ProjectIds = projectIdsResponse.ToArray()
            };
            return Created("", new { response });
        }
        
        //Remove Skill
        [HttpDelete("Id/{profileId}")]
        public ActionResult<bool> RemoveSkill(int Id, int profileId)
        {
            var skill = new Skill();
            
            //Find Profile
            var skillProfile = new Profile();
            if(profileId>0)
            {
                var skillProfileExists = profileService.ProfileExists(new Profile { ID = profileId });

                if(!skillProfileExists)
                {
                    return NotFound("Profile not found.");
                }

                skillProfile = profileService.FetchSingleById(profileId);

                skill = skillService.FetchSingleByProfileId(Id, skillProfile.ID);
            }else{
                skill = skillService.FetchSingleById(Id);
            }

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
            var skill = skillService.FetchSingleById(Id);

            if(skill == null)
            {
                return NotFound("Resource Not Found.!");
            }

            //Format response
            var response = new SkillInfoResponse
            {
                ID = skill.ID,
                Name = skill.Name,
                ProfileId = skill.Profile.ID,
                ProjectIds = skill.Projects.Select((p)=>p.ID).ToArray()
            };
            return Ok(response);
        }

        //Get All Skills
        [HttpGet()]
        public ActionResult<DataListResponse<SkillInfoResponse>> GetAllSkills(int profileId)
        {
            var skills = new List<Skill>();
            if(profileId>0)
            {
                skills = skillService.FetchAllByProfileId(profileId).ToList();
            }
            else
            {                
                skills = skillService.FetchAll().ToList();
            }

            if(skills == null)
            {
                return NoContent();
            }

            //Format Response

          var response = new DataListResponse<SkillInfoResponse>();
            response.Data = skills.Select((p)=> new SkillInfoResponse 
            {
                ID = p.ID,
                ProfileId = p.Profile.ID,
                Name = p.Name,
                ProjectIds = p.Projects.Select((pj)=>pj.ID).ToArray(),
            } ).ToList();
            response.TotalItems = response.Data.Count;
            return Ok(new{response});
        }

    }




    
}