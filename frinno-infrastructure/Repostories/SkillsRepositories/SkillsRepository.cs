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
        public Skill AddNew(Skill newData)
        {
            var data = DB.Skills.Add(newData);
            SaveContextChanges();

            return data.Entity;
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

        public void Remove(Skill data)
        {
            DB.Skills.Remove(data);
            SaveContextChanges();
        }

        public void SaveContextChanges()
        {
            DB.SaveChanges();
        }

        public Skill Update(Skill updateData)
        {
            var data  = DB.Skills.Update(updateData);
            SaveContextChanges();
            return data.Entity;
        }
    }
}