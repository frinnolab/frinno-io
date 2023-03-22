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
        private readonly UserManager<Profile> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly DataContext DB;
        private ITokenService tokenService;

        public AuthRepository(DataContext data, ITokenService tService)
        {
            DB = data;
            tokenService = tService;
        }
        public UserResponse FindUserByEmail(string email)
        {
            var user = DB.Profiles.First(u=>u.User.Email == email );
            var userResult = new UserResponse () { Id = user.Id, Email = user.User.Email, FirstName = user.FirstName, hashedPassword = user.User.Password };

            return userResult;
        }

        public UserResponse FindUserById(string userId)
        {
            var user = DB.Profiles.FirstOrDefault(u=>u.Id == userId);

            var userResult = new UserResponse{ Id = user.Id, FirstName = user.FirstName, Email = user.User.Email };

            return userResult;
        }

        public void Forgotten()
        {
            throw new NotImplementedException();
        }

        public void LogOut()
        {

        }

        public LoginResponse Login(UserResponse user)
        {
            var userToken = tokenService.Generate(user);
            var tokenRespnse = new LoginResponse { Email = user.Email, Fullname = user.FirstName, Token = userToken};
            return tokenRespnse;
        }

        public void Recovery()
        {
            throw new NotImplementedException();
        }


        public bool UserExists(string email)
        {
            var user = DB.Profiles.Any(u=>u.User.Email == email);

            return user;
        }

        public bool VerifyPassord(string password, string hashedPassword)
        {
            var isMatched = BCrypt.Net.BCrypt.Verify(password, hashedPassword);

            return isMatched;
        }

        public async Task<bool> Register(CreateAProfileRequest request)
        {
            //Assign Roles.
            var user = await userManager.CreateAsync(new Profile { User = new (), Address = new () });
            return user.Succeeded;
        }
    }
}