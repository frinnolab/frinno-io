using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_core.Entities.Project.ValueObjects;
using frinno_core.Entities.Skill.Aggregates;

namespace frinno_core.Entities.Skill
{
    //Eg: WebApi Design & Implementation:Tools = []
    public class Skill : BaseEntity
    {
        public string Name { get; set; }
        public Profiles.Profile Profile { get; set; }

        public virtual ICollection<ProjectSkills> Projects { get; set; }

    }
}