using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_application.Authentication;
using frinno_core.DTOs;
using frinno_core.Entities.Profiles;
using frinno_core.Entities.user;
using frinno_infrastructure.Data;

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
        public UserResponse FindUserByEmail(string email)
        {
            
            var user = DB.Profiles.First(u=>u.User.Email == email );

            if(user == null)
            return null;

            var userResult = new UserResponse () { Id = user.ID, Email = user.User.Email, FirstName = user.FirstName };

            return userResult;
        }

        public UserResponse FindUserById(int userId)
        {
            var user = DB.Profiles.FirstOrDefault(u=>u.ID == userId);

            var userResult = new UserResponse{ Id = user.ID, FirstName = user.FirstName, Email = user.User.Email };

            return userResult;
        }

        public void Forgotten()
        {
            throw new NotImplementedException();
        }

        public void LogOut()
        {

        }

        public LoginResponse Login(LoginRequest request)
        {
            //var tokenUser = tokenService.Generate()

            var tokenRespnse = new LoginResponse {};
            return tokenRespnse;
        }

        public void Recovery()
        {
            throw new NotImplementedException();
        }

        public RegisterResponse Register(RegisterRequest request)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var userOptions = new User { Email = request.Email, Password = hashedPassword };
            var newUser = new Profile { FirstName =  request.FirstName, User = userOptions  };
            //Future Add to Role
            DB.Profiles.Add(newUser);
            DB.SaveChanges();

            var userResponse = new RegisterResponse { Fullname = newUser.FirstName, Email = newUser.User.Email, Id = newUser.ID };

            return userResponse;
        }

        public bool UserExists(string email)
        {
            var user = DB.Profiles.Any(u=>u.User.Email == email);

            return user;
        }
    }
}