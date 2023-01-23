using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            var response = DB.Skills.Add(newData);
            SaveContextChanges();
            return response.Entity;
        }

        public IEnumerable<Skill> GetAll()
        {
            return DB.Skills.ToList();
        }

        public IEnumerable<Skill> GetAllBy(Expression<Func<Skill, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Skill GetSingle(Skill data)
        {
            var response = DB.Skills.FirstOrDefault(x=>x==data);
            return response;
        }

        public Skill GetSingleById(int dataId)
        {
            var response = DB.Skills.Find(dataId);
            return response;
        }

        public void Remove(int dataId)
        {
            var response = DB.Skills.Find(dataId);
            DB.Skills.Remove(response);
            SaveContextChanges();
            
        }

        public void SaveContextChanges()
        {
            DB.SaveChanges();
        }

        public Skill Update(Skill updateData)
        {
            var resposnse = DB.Skills.Update(updateData);
            SaveContextChanges();
            return resposnse.Entity;
        }
    }
}