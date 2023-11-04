using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_core.Entities.Profile.ValueObjects;
using frinno_core.Entities.Project;
using frinno_core.Entities.Project.ValueObjects;

namespace frinno_core.DTOs
{
            //Create a Project Request
    
    public record CreateProjectRequest
    {
        public DateTime ProjectStart { get; set; }
        public DateTime ProjectEnd { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ProjectStatus Status { get; set; } = ProjectStatus.Planning;
        public ProjectTypeEnum ProjectType { get; set; } = (int)ProjectTypeEnum.Fullstack;
        public string ProjectUrl { get; set; } = string.Empty;
        public string RepositoryUrl { get; set; } = string.Empty;
        public bool IsRepoPublic { get; set; } = false;
        //Project Client
        public ProjectClientInfo ClientInfo { get; set; }
        //Project Company/Agency
        public ProjectAgencyInfo AgencyCompanyInfo { get; set; }

    }
    //Create a Project Response
    
    public record CreateProjectResponse 
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public string ProjectUrl { get; set; }
        public string ProfileId { get; set; }

        public DateTime Created { get; set; }

        public ProjectStatus Status { get; set; }

        public ProjectTypeEnum ProjectType { get; set; }
    }
    //Project Single Resource Request
    public record ProjectInfoRequest
    {
        public string Title { get; set; } = string.Empty;
        public string ProfileId { get; set; } = string.Empty;

        public ProjectStatus Status { get; set; }

        public bool IsRepoPublic { get; set; } = false;
        //Project Client
        public string ClientName { get; set; } = string.Empty;

        //Project Company/Agency
        public string CompanyAgencyName { get; set; } = string.Empty;
    }
        

    //Project Create Response.
    public record ProjectCreateResponse : CreateProjectResponse
    {
        public DateTime Modified { get; set; }
    }

    public record ProjectInfoResponse : ProjectCreateResponse
    {
        public string RepositoryUrl { get; set; } = string.Empty;
        public bool IsRepoPublic { get; set; } = false;
        //Project Client
        public ProjectClientInfo ClientInfo { get; set; }
        //Project Company/Agency
        public ProjectAgencyInfo CompanyAgencyInfo { get; set; }
        public DateTime ProjectStart { get; set; }
        public DateTime ProjectEnd { get; set; }

    }

    //Project Update Request
    public record UpdateProjectRequest : CreateProjectRequest
    {
     public int Id { get; set; }
    }

    public record UpdateProjectResponse : UpdateProjectRequest
    {

    }
}