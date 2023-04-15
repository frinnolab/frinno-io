using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_core.DTOs;
using frinno_core.Entities.Profiles;

namespace frinno_application.Authentication
{
    public interface IRegisterService
    {
        //Register Service Definitions
        Task<Profile> Register(Profile newProfile);
        void Forgotten(string email);
        void Recovery(string newPassword);
    }
}