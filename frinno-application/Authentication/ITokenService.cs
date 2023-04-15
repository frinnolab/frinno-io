using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_core.DTOs;
using frinno_core.Entities.Profiles;
using Microsoft.AspNetCore.Identity;

namespace frinno_application.Authentication
{
    public interface ITokenService
    {
        //Token Service Definitions
       string GenerateToken(Profile profile, string RoleName);
    }
}