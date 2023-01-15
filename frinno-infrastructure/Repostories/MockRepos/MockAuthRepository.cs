using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_application.Generics;
using frinno_core.Entities.MockModels;
using frinno_core.Entities.MockModels.MockDTOs;

namespace frinno_infrastructure.Repostories.MockRepos
{
    public class MockAuthRepository : IMockAuthService
    {
        private readonly MockDataContext DB;

        public MockAuthRepository(MockDataContext context)
        {
            DB = context;
        }
        public MockLoginResponse AuthenticateUser(MockUser user)
        {
            var response = new MockLoginResponse();
            var token = new MockTokens() { User = user, Token = CreateToken(new MockLoginRequest{ Email = user.Email, Password = user.Password }).ApiToken};
            token.IsValidToken = true;
            token.ValidUntil = new DateTime().AddMinutes(10);
            DB.MockTokens.Add(token);
            DB.SaveChanges();
            return response;
        }

        public MockTokenResponse CreateToken(MockLoginRequest request)
        {
            var randomTokn = "ajgaAy6e7weAnamaIa9982";
            var response = new MockTokenResponse() { ApiToken = randomTokn };

            return response;
        }

        public MockUser GetMockUserByEmail(string email)
        {
            var user = DB.MockUsers.FirstOrDefault(x=>x.Email.ToLower() == email.ToLower());

            return user;
        }

        public List<MockUser> GetMockUsers()
        {
            return DB.MockUsers.ToList();
        }

        public string HashedPassword(MockLoginRequest request)
        {
            string password = request.Password;

            return password;
        }

        public MockRegisterResponse RegisterUser(MockRegisterRequest request, MockRoles role)
        {
            var user  = new MockRegisterResponse();

            //Hash pasword

            var password = HashedPassword(new MockLoginRequest { Password = request.Password, Email = request.Email });

            var newUser = new MockUser()
            {
                Fullname = request.Fullname,
                Email = request.Email,
                Password = password,
                Modified = DateTime.Now
            };

            //Add role

            switch (role)
            {
                case MockRoles.MockAdmin:
                    newUser.Role = MockRoles.MockAdmin;
                    break;

                case MockRoles.MockAuthor:
                    newUser.Role = MockRoles.MockAuthor;
                    break;

                case MockRoles.MockUser:
                    newUser.Role = MockRoles.MockUser;
                    break;
                default:
                    newUser.Role = MockRoles.MockUser;
                    break;
            }

            //Persist user
            DB.MockUsers.Add(newUser);
            DB.SaveChanges();

            user.Fullname = newUser.Fullname;
            user.Role = newUser.Role;
            return user;
        }

        public void RevokeToken(string apiToken)
        {
            throw new NotImplementedException();
        }

        public bool ValidateUser(MockUser request)
        {
            var user = DB.MockUsers.FirstOrDefault(u=>u.Password == request.Password);

            if(user is null)
            {
                return false;
            }

            return true;
        }
    }
}