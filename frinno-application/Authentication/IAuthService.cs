using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_core.DTOs;
using frinno_core.Entities.Profiles;
using frinno_core.Entities.user;

namespace frinno_application.Authentication
{
    public interface IAuthService : ILoginService, IRegisterService
    {
        
        string GetAuthToken(Profile profile);
    }

}