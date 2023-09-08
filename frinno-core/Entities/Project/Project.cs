using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_core.Entities.Profile.ValueObjects;
using frinno_core.Entities.Project.ValueObjects;

namespace frinno_core.Entities.Projects
{
    public class Project: BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Profiles.Profile Profile { get; set; }
        //Project Url
        public string ProjectUrl { get; set; }
        public string RepositoryUrl { get; set; }
        public bool IsRepoPublic { get; set; } = false;
        public int Status { get; set; } = (int)ProjectStatus.Planning;

        //Project Client
        public string ClientName { get; set; }
        public string ClientPublicLink { get; set; }
        public Address ClientAddress { get; set; }
        //Project Company/Agency
        public string CompanyAgencyName { get; set; }
        public string CompanyAgencyPublicLink { get; set; }
        public Address CompanyAgencyAddress { get; set; }

    }
}