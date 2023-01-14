using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace frinno_application.Authentication
{
    public interface ILoginService
    {
        void Login();
        void LogOut();
    }
}