using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        //Skills Used
        //Status
        public int Status { get; set; } = (int)ProjectStatus.NotStarted;

    }
}