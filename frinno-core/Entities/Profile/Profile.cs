using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using frinno_core.Entities.Articles;
using frinno_core.Entities.Profile.Aggregates;
using frinno_core.Entities.Profile.ValueObjects;
using frinno_core.Entities.Projects;
using frinno_core.Entities.user;
using Microsoft.AspNetCore.Identity;

namespace frinno_core.Entities.Profiles
{
    public class Profile : IdentityUser
    {
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual Address Address { get; set; }
        public virtual AuthRolesEnum Role { get; set; }
        public virtual ICollection<ProfileArticles> ProfileArticles { get; set; }
        public virtual ICollection<Projects.Project> Projects { get; set; }
        public virtual ICollection<Skill.Skill> Skills { get; set; }
        public virtual ICollection<Resumes.Resume> Resumes { get; set; }
    }
}