using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace frinno_core.Entities.Project.ValueObjects
{
    public class ProjectSkills : BaseEntity
    {
        public int ProjectId { get; set; }
        public virtual Projects.Project Project { get; set; }

        public int SkillId { get; set; }
        public virtual Skill.Skill Skill { get; set; }

    }
}
