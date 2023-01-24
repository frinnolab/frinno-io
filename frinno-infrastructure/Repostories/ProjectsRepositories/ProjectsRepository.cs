using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_application.Projects;
using frinno_core.Entities.Projects;
using frinno_infrastructure.Data;

namespace frinno_infrastructure.Repostories.ProjectsRepositories
{
    public class ProjectsRepository : IProjectsManager<Project>
    {
        private readonly DataContext DB;

        public ProjectsRepository(DataContext data)
        {
            DB = data;
        }
        public void AddNew(Project newData)
        {
            DB.Projects.Add(newData);
        }

        public IEnumerable<Project> FetchAll()
        {
            return DB.Projects.ToList();
        }

        public Project FetchSingle(Project data)
        {
            return DB.Projects.FirstOrDefault((x)=>x==data);
        }

        public Project FetchSingleById(int dataId)
        {
            return DB.Projects.Find(dataId);
        }

        public void Remove(int dataId)
        {
            var data = DB.Projects.Find(dataId);
            DB.Projects.Remove(data);
        }

        public void SaveContextChanges()
        {
            DB.SaveChanges();
        }

        public void Update(Project updateData)
        {
            DB.Projects.Update(updateData);
        }
    }
}