using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_core.Entities.Profile.ValueObjects;
using frinno_core.Entities.Project;
using frinno_core.Entities.Project.ValueObjects;

namespace frinno_core.Entities.Projects
{
    public class Project: BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime ProjectStart { get; set; }
        public DateTime ProjectEnd { get; set; }
        public Profiles.Profile Profile { get; set; }
        //Project Url
        public string ProjectUrl { get; set; }
        public string RepositoryUrl { get; set; }
        public bool IsRepoPublic { get; set; } = false;
        public int Status { get; set; } = (int)ProjectStatus.Planning;

        public int ProjectType { get; set; } = (int)ProjectTypeEnum.Fullstack;
        //Project Client
        public ProjectClientInfo ClientInfo { get; set; } = new();

        //Project Company/Agency
        public ProjectAgencyInfo CompanyAgencyInfo { get; set; } = new();

    }
}