using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_application.Generics;

namespace frinno_application.Profiles
{
    public interface IProfileService<Profile> : IDataManager<Profile>
    {
        IEnumerable<Profile> FetchAll();
        Profile FindByEmail(string email);
        Profile FindById(string profileId);
        bool ProfileExists(Profile profile);
        void UploadProfileImage();
        void RemoveProfileImage();
    }
}