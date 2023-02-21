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
            .Include(x=>x.Skills)
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
            .Include(x=>x.Resumes)
            .Include(x=>x.Skills)
            .FirstOrDefault((p)=>p==data);
        }

        public Profile FindById(string dataId)
        {
            return DB.Profiles
            .Include(u=>u.User)
            .Include(a=>a.Address)
            .Include(x=>x.ProfileArticles)
            .Include(x=>x.Projects)
            .Include(x=>x.Resumes)
            .Include(x=>x.Skills)
            .Single(x=>x.Id ==dataId);
        }

        public Profile FindByEmail(string email)
        {
            return DB.Profiles.Include(x=>x.User).SingleOrDefault((p=>p.User.Email == email));
        }

        public bool ProfileExists(Profile profile)
        {
            if(profile.User != null )
            {
                return DB.Profiles.Include(u=>u.User)
                .Any((p)=> p.User.Email == profile.User.Email);
            }
            else
            {
                return DB.Profiles.Any((p)=> p.Id == profile.Id);
            }
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