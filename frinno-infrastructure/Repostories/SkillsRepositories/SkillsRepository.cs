using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_application.Skills;
using frinno_core.Entities.Skill;
using frinno_infrastructure.Data;

namespace frinno_infrastructure.Repostories.SkillsRepositories
{
    public class SkillsRepository : ISkillsService
    {
        private readonly DataContext DB;
        public SkillsRepository(DataContext data)
        {
            DB = data;
        }
        public void AddNew(Skill newData)
        {
            DB.Skills.Add(newData);
        }

        public IEnumerable<Skill> FetchAll()
        {
            return DB.Skills.ToList();
        }

        public Skill FetchSingle(Skill data)
        {
            return DB.Skills.FirstOrDefault((s)=>s==data);
        }

        public Skill FetchSingleById(int dataId)
        {
            return DB.Skills.Find(dataId);
        }

        public void Remove(int dataId)
        {
            var data = DB.Skills.Find(dataId);
            DB.Skills.Remove(data);
        }

        public void SaveContextChanges()
        {
            DB.SaveChanges();
        }

        public void Update(Skill updateData)
        {
            DB.Skills.Update(updateData);
        }
    }
}