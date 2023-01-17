using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using frinno_application.Generics;
using frinno_core.Entities.MockModels;
using frinno_core.Entities.MockModels.MockDTOs;
using Microsoft.IdentityModel.Tokens;

namespace frinno_infrastructure.Repostories.MockRepos
{
    public class MockAuthRepository : IMockAuthService
    {
        private int hashKeySize = 64;
        private int hashIterations = 4000;

        private HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
        private readonly MockDataContext DB;

        public MockAuthRepository(MockDataContext context)
        {
            DB = context;
        }
        public MockLoginResponse AuthenticateUser(MockUser user)
        {
            var response = new MockLoginResponse();
            var token = new MockTokens() { User = user, Token = CreateToken(user).ApiToken};
            token.IsValidToken = true;
            token.ValidUntil = new DateTime().AddMinutes(10);
            DB.MockTokens.Add(token);
            response.Email = token.User.Email;
            response.ApiToken = token.Token;
            DB.SaveChanges();
            return response;
        }

        public MockTokenResponse CreateToken(MockUser user)
        {
            var randomTokn = "5HMQ@FbiMTkWu6m";
            var response = new MockTokenResponse();

            var sKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(randomTokn));

            var sCreds = new SigningCredentials(sKey, SecurityAlgorithms.HmacSha256);

            var tokeOptions = new JwtSecurityToken(
                issuer: "https://localhost:5001",
                audience: "https://localhost:5001",
                claims: new List<Claim>(),
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: sCreds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

            response.ApiToken = tokenString;

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

        public MockRegisterResponse RegisterUser(MockRegisterRequest request, MockRoles role)
        {
            var user  = new MockRegisterResponse();
            

            //Hash pasword

            var hashRequest = new MockLoginRequest()
            {
                Password = request.Password,
                Email = request.Email
            };

            var newUser = new MockUser()
            {
                Fullname = request.Fullname,
                Email = request.Email,
                Modified = DateTime.Now
            };

            DB.MockUsers.Add(newUser);

            var password = HashedPassword(hashRequest);
            newUser.Password = password.hashedPassword;
            newUser.confirmPassword = password.hashedSalt;


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
            DB.SaveChanges();

            user.Fullname = newUser.Fullname;
            user.Role = newUser.Role;
            return user;
        }

        public void RevokeToken(string apiToken)
        {
            throw new NotImplementedException();
        }

        public PasswordHasModel HashedPassword(MockLoginRequest request)
        {
            var salt = RandomNumberGenerator.GetBytes(hashKeySize);

            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(request.Password),
                salt,
                hashIterations,
                hashAlgorithm,
                hashKeySize
            );

            return new PasswordHasModel{ hashedPassword = Convert.ToHexString(hash), hashedSalt = salt };
        }

        public bool ValidateUserPassword(MockLoginRequest loginRequest, MockUser user)
        {
            var compare = Rfc2898DeriveBytes
            .Pbkdf2(loginRequest.Password, user.confirmPassword, hashIterations, hashAlgorithm, hashKeySize);

            return compare.SequenceEqual(Convert.FromHexString(user.Password));
        }
    }
}