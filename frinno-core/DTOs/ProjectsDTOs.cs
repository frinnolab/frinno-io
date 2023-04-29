using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_core.Entities.Project.ValueObjects;

namespace frinno_core.DTOs
{
            //Create a Project Request
    
    public record CreateProjectRequest
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; }
        public string Url { get; set; }
        public ProjectStatus Status { get; set; }
    }
    //Create a Project Response
    
    public record CreateProjectResponse
    {
        public string Title { get; set; } 
        public int Id { get; set; }
        public string ProfileId { get; set; }
        public string ProjectUrl { get; set; }
    }
    //Project Single Resource Request
    public record ProjectInfoRequest
    {
        public string Title { get; set; } = string.Empty;
        public string ProfileId { get; set; } = string.Empty;
    }
        

    //Project Response.
    public record ProjectInfoResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public int? SkillsEarned { get; set; }
        public int Status { get; set; }
    }

    //Project Update Request
    public record UpdateProjectRequest : CreateProjectRequest
    {
     public int Id { get; set; }
    }
}