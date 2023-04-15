using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_application.Authentication;
using frinno_core.DTOs;
using frinno_core.Entities.Profiles;
using frinno_core.Entities.user;
using frinno_infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace frinno_infrastructure.Repostories
{
    public class AuthRepository : IAuthService
    {
        private readonly DataContext DB;
        private ITokenService tokenService;

        public AuthRepository(DataContext data, ITokenService tService)
        {
            DB = data;
            tokenService = tService;
        }

        public void Forgotten(string email)
        {
            throw new NotImplementedException();
        }


        public void Recovery(string newPassword)
        {
            throw new NotImplementedException();
        }

        public async Task<Profile> Register(Profile newProfile)
        {
            var user = await DB.Profiles.AddAsync(newProfile);
            await DB.SaveChangesAsync();
            return user.Entity;
        }

        public string GetAuthToken(Profile profile)
        {
            var roleName = Enum.GetName(profile.Role);
            var generatedToken =  tokenService.GenerateToken(profile, roleName);
            return generatedToken;
        }
    }
}