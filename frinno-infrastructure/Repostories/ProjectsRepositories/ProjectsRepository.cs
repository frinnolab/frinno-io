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
        public async Task<Project> AddNew(Project newData)
        {
            var data = await DB.Projects.AddAsync(newData);
            await DB.SaveChangesAsync();
            return data.Entity;
        }

        public async Task<IEnumerable<Project>> FetchAll()
        {
            return await DB.Projects
            .Include(pr=>pr.Profile)
            .ToListAsync();
        }

        public async Task<IEnumerable<Project>> FetchAllByProfileId(string profileId)
        {
            return await DB.Projects
            .Include(pr=>pr.Profile)
            .Where(p=>p.Profile.Id == profileId)
            .ToListAsync();
        }

        public Project FetchSingle(Project data)
        {
            return DB.Projects
            .Include(pr=>pr.Profile)
            .SingleOrDefault((x)=>x==data);
        }

        public Project FetchSingleById(int dataId)
        {
            return DB.Projects
            .Include(pr=>pr.Profile)
            .SingleOrDefault(x=>x.Id==dataId);
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

        public async Task<Project> Update(Project updateData)
        {
            var data = DB.Projects.Update(updateData);
            await DB.SaveChangesAsync();
            return data.Entity;
        }
    }
}