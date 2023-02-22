using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_application.Projects;
using frinno_core.Entities.Projects;
using frinno_infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace frinno_infrastructure.Repostories.ProjectsRepositories
{
    public class ProjectsRepository : IProjectsManager<Project>
    {
        private readonly DataContext DB;

        public ProjectsRepository(DataContext data)
        {
            DB = data;
        }
        public Project AddNew(Project newData)
        {
            var data = DB.Projects.Add(newData);
            SaveContextChanges();
            return data.Entity;
        }

        public IEnumerable<Project> FetchAll()
        {
            return DB.Projects
            .Include(pr=>pr.Profile)
            .ThenInclude(pr=>pr.Skills)
            .ToList();
        }

        public List<Project> FetchAllByProfileId(string profileId)
        {
            return DB.Projects
            .Include(pr=>pr.Profile)
            .ThenInclude(pr=>pr.Skills)
            .Where(p=>p.Profile.Id == profileId)
            .ToList();
        }

        public Project FetchSingle(Project data)
        {
            return DB.Projects
            .Include(pr=>pr.Profile)
            .ThenInclude(pr=>pr.Skills)
            .Single((x)=>x==data);
        }

        public Project FetchSingleById(int dataId)
        {
            return DB.Projects
            .Include(pr=>pr.Profile)
            .ThenInclude(pr=>pr.Skills)
            .Single(x=>x.Id==dataId);
        }

        public bool Exists(int dataId)
        {
            return DB.Projects.Any(p=>p.Id ==dataId);
        }

        public void Remove(Project data)
        {
            DB.Projects.Remove(data);
            SaveContextChanges();
        }

        public void SaveContextChanges()
        {
            DB.SaveChanges();
        }

        public Project Update(Project updateData)
        {
            var data = DB.Projects.Update(updateData);
            SaveContextChanges();
            return data.Entity;
        }
    }
}