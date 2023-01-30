using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_application.Profiles;
using frinno_core.Entities.Profiles;
using frinno_infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace frinno_infrastructure.Repostories.ProfilesRepositories
{
    public class ProfileRepository : IProfileService<Profile>
    {
        private readonly DataContext DB;
        public ProfileRepository(DataContext data)
        {
            DB = data;
        }
        public Profile AddNew(Profile newData)
        {
            var data = DB.Profiles.Add(newData);
            SaveContextChanges();
            return data.Entity;;
        }

        public IEnumerable<Profile> FetchAll()
        {
            return DB.Profiles
            .Include(x=>x.User)
            .Include(x=>x.Address)
            .Include(x=>x.ProfileArticles)
            .Include(x=>x.Projects)
            .Include(x=>x.Resumes)
            .ToList();
        }

        public Profile FetchSingle(Profile data)
        {
            return DB.Profiles
            .Include(x=>x.User)
            .Include(x=>x.Address)
            .Include(x=>x.ProfileArticles)
            .Include(x=>x.Projects)
            .Include(x=>x.Resumes).FirstOrDefault((p)=>p==data);
        }

        public Profile FetchSingleById(int dataId)
        {
            return DB.Profiles.Find(dataId);
        }

        public bool ProfileExists(Profile profile)
        {
            return DB.Profiles.Any((p)=> p.User.Email == profile.User.Email);
        }

        public void Remove(Profile data)
        {
            DB.Profiles.Find(data.ID);
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

        public Profile Update(Profile updateData)
        {
            var data = DB.Profiles.Update(updateData);
            SaveContextChanges();
            return data.Entity;
        }

        public void UploadProfileImage()
        {
            throw new NotImplementedException();
        }
    }
}