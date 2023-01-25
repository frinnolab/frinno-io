using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace frinno_core.Entities.Skill.Aggregates
{
    //Eg {"WebApi Design":[{"VsCode":"Backend Dev"},{"Azure":"Backend Host"}]
    public class SkillTool : BaseEntity
    {
        public string Name { get; set; }   
        public string Usage { get; set; }
        public Skill Skill { get; set; }
    }
}