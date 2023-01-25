using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_core.Entities.Articles;
using frinno_core.Entities.Profile.Aggregates;
using frinno_core.Entities.Profile.ValueObjects;
using frinno_core.Entities.Projects;
using frinno_core.Entities.user;

namespace frinno_core.Entities.Profiles
{
    public class Profile : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public User User { get; set; }
        public Address Address { get; set; }
        public List<ProfileArticles> ProfileArticles { get; set; }
        public List<Projects.Project> Projects { get; set; }
        public List<Skill.Skill> Skills { get; set; }
        public List<Resumes.Resume> Resumes { get; set; }
    }
}