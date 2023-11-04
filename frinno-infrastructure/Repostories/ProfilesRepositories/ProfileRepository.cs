using frinno_application.Profiles;
using frinno_core.Entities.Profiles;
using frinno_infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace frinno_infrastructure.Repostories.ProfilesRepositories
{
    public class ProfileRepository : IProfileService<Profile>
    {
        private readonly DataContext DB;
        public ProfileRepository(DataContext data)
        {
            DB = data;
        }
        public async Task<Profile> AddNew(Profile newData)
        {
            var data = await DB.Profiles.AddAsync(newData);
            await DB.SaveChangesAsync();
            return data.Entity;;
        }

        public IEnumerable<Profile> FetchAll()
        {
            return DB.Profiles
            .Include(x=>x.Address)
            .Include(x=>x.Projects)
            .ToList();
        }

        public Profile FetchSingle(Profile data)
        {
            return DB.Profiles
            .Include(x=>x.Address)
            .Include(x=>x.Projects)
            .FirstOrDefault((p)=>p==data);
        }

        public void Remove(Profile data)
        {
            DB.Profiles.Find(data.Id);
            DB.Profiles.Remove(data);
            SaveContextChanges();
        }

        public void RemoveProfileImage()
        {
            throw new NotImplementedException();
        }

        public void SaveContextChanges()
        {
            DB.SaveChanges();
        }

        public async Task<Profile> Update(Profile updateData)
        {
            var data = DB.Update(updateData);
            await DB.SaveChangesAsync();
            return data.Entity;
        }

        public void UploadProfileImage()
        {
            throw new NotImplementedException();
        }

        public Profile FindProfileById(string profileId)
        {
            return DB.Profiles
                .Include(a => a.Address)
                .Include(x => x.Projects)
                .Single(x => x.Id == profileId);
        }
    }
}