using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_core.DTOs;

namespace frinno_application.Authentication
{
    public interface ITokenService
    {
        string Generate(UserResponse user);

        int? Validate(string token);
    }
}