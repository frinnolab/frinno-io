using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_core.DTOs;

namespace frinno_application.Authentication
{
    public interface IRegisterService
    {
        Task<bool> Register(CreateAProfileRequest request);
        void Forgotten();
        void Recovery();

    }
}