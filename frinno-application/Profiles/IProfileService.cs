using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_application.Generics;

namespace frinno_application.Profiles
{
    public interface IProfileService<Profile> : IMasterService<frinno_core.Entities.Profiles.Profile>
    {
        Profile FindByEmail(string email);
        bool ProfileExists(Profile profile);
        void UploadProfileImage();
        void RemoveProfileImage();
    }
}