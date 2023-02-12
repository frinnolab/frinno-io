using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace frinno_core.DTOs
{
    //Transfer objects for Skills Resources

    //Create a New Skill
    public record CreateNewSkillRequest
    {
        public string Name { get; set; }
        public int ProjectId { get; set; }

        public List<CreateSkillTools> SkillTools { get; set; }
    }
    public record CreateSkillTools
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Usage { get; set; }
    }

    public record CreateNewSkillResponse : CreateNewSkillRequest
    {
        public int ID { get; set; }
        public int ProfileId { get; set; }
    }
    public record SkillInfoResponse : CreateNewSkillResponse
    {

    }

    //Update Skill
    public record UpdateSkillRequest
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int ProjectId { get; set; }
        public List<CreateSkillTools> SkillTools { get; set; }
    }
    public record UpdateSkillResponse : UpdateSkillRequest
    {
    }
}