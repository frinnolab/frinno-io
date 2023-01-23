using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using frinno_application.Profiles;
using frinno_core.Entities.Profiles;
using frinno_infrastructure.Data;

namespace frinno_infrastructure.Repostories
{
    public class ProfilesRepository : IProfileService<Profile>
    {
        private readonly DataContext DB;
        public ProfilesRepository(DataContext db)
        {
            DB = db;
        }
        public Profile AddNew(Profile newData)
        {
            var response = DB.Profiles.Add(newData);
            DB.SaveChanges();

            return response.Entity;

        }

        public IEnumerable<Profile> GetAll()
        {
            return DB.Profiles.ToList();
        }

        public IEnumerable<Profile> GetAllBy(Expression<Func<Profile, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Profile GetSingle(Profile data)
        {
            throw new NotImplementedException();
        }

        public Profile GetSingleById(int dataId)
        {
            var data = DB.Profiles.Find(dataId);

            return data;
        }

        public void Remove(int dataId)
        {
            var data = DB.Profiles.Find(dataId);
            DB.Profiles.Remove(data);
            DB.SaveChanges();
        }

        public void RemoveProfileImage()
        {
            throw new NotImplementedException();
        }

        public void SaveContextChanges()
        {
            throw new NotImplementedException();
        }

        public Profile Update(Profile updateData)
        {
            var response = DB.Profiles.Update(updateData);
            DB.SaveChanges();
            return response.Entity;
        }

        public void UploadProfileImage()
        {
            throw new NotImplementedException();
        }
    }
}