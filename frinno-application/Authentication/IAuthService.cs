using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_core.DTOs;
using frinno_core.Entities.user;

namespace frinno_application.Authentication
{
    public interface IAuthService : ILoginService, IRegisterService
    {
        UserResponse FindUserByEmail(string email);
        UserResponse FindUserById(string userId);
        bool UserExists(string email);
    }

}