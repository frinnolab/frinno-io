using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_application.Profiles;
using frinno_core.Entities.Profiles;
using frinno_infrastructure.Data;

namespace frinno_infrastructure.Repostories.ProfilesRepositories
{
    public class ProfileRepository : IProfileService<Profile>
    {
        private readonly DataContext DB;
        public ProfileRepository(DataContext data)
        {
            DB = data;
        }
        public void AddNew(Profile newData)
        {
            DB.Profiles.Add(newData);
        }

        public IEnumerable<Profile> FetchAll()
        {
            return DB.Profiles.ToList();
        }

        public Profile FetchSingle(Profile data)
        {
            return DB.Profiles.FirstOrDefault((p)=>p==data);
        }

        public Profile FetchSingleById(int dataId)
        {
            return DB.Profiles.Find(dataId);
        }

        public void Remove(int dataId)
        {
            var data = DB.Profiles.Find(dataId);
            DB.Profiles.Remove(data);
        }

        public void RemoveProfileImage()
        {
            throw new NotImplementedException();
        }

        public void SaveContextChanges()
        {
            DB.SaveChanges();
        }

        public void Update(Profile updateData)
        {
            DB.Profiles.Update(updateData);
        }

        public void UploadProfileImage()
        {
            throw new NotImplementedException();
        }
    }
}