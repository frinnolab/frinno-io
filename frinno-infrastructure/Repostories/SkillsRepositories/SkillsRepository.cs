using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_application.Skills;
using frinno_core.Entities.Skill;
using frinno_infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace frinno_infrastructure.Repostories.SkillsRepositories
{
    public class SkillsRepository : ISkillsService
    {
        private readonly DataContext DB;
        public SkillsRepository(DataContext data)
        {
            DB = data;
        }
        public async Task<Skill> AddNew(Skill newData)
        {
            var data = await DB.Skills.AddAsync(newData);
            await DB.SaveChangesAsync();
            return data.Entity;
        }

        public async Task<IEnumerable<Skill>> FetchAll()
        {
            return await DB.Skills
            .Include(p=>p.Profile)
            .ThenInclude(x=>x.Skills)
            .Include(s=>s.Projects)
            .ToListAsync();
        }

        public IEnumerable<Skill> FetchAllByProfileId(string profileId)
        {
            return DB.Skills
            .Include(p=>p.Profile)
            .ThenInclude(p=>p.Skills)
            .Where(p=>p.Profile.Id == profileId)
            .Include(pj=>pj.Projects)
            .Include(s=>s.Projects)
            .ToList();
        }

        public Skill FetchSingle(Skill data)
        {
            return DB.Skills
            .Include(p=>p.Profile)
            .ThenInclude(x=>x.Skills)
            .Include(s=>s.Projects)
            .FirstOrDefault((s)=>s==data);
        }

        public Skill FetchSingleById(int dataId)
        {
            return DB.Skills
            .Include(p=>p.Profile)
            .ThenInclude(x=>x.Skills)
            .Include(s=>s.Projects)
            .FirstOrDefault(x=>x.Id == dataId);
        }

        public Skill FetchSingleByProfileId(int Id, string profileId)
        {
            return DB.Skills
            .Include(p=>p.Profile)
            .ThenInclude(x=>x.Skills)
            .Where(p=>p.Profile.Id == profileId)
            .Include(s=>s.Projects)
            .FirstOrDefault(x=>x.Id == Id);
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

        public async Task<Skill> Update(Skill updateData)
        {
            var data  = DB.Skills.Update(updateData);
            await DB.SaveChangesAsync();
            return data.Entity;
        }
    }
}