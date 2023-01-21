using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_core.DTOs;

namespace frinno_application.Authentication
{
    public interface ILoginService
    {
        LoginResponse Login(UserResponse user);
        void LogOut();

        bool VerifyPassord(string password, string hashedPassword);
    }
}