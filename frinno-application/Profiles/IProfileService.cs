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
        //Replaced with UserManager API
        //Profile FindByEmail(string email);
        //Explicit Profile Fetch
        Profile FindProfileById(string profileId);
        void UploadProfileImage();
        void RemoveProfileImage();
    }
}