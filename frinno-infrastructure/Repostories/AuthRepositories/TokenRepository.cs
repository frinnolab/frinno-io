using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using frinno_application.Authentication;
using frinno_core.DTOs;
using frinno_core.Entities;
using frinno_core.Entities.Profiles;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace frinno_infrastructure.Repostories.AuthRepositories
{
    public class TokenRepository : ITokenService
    {
        
        private readonly IConfiguration configuration;

        public TokenRepository(IConfiguration config)
        {
            configuration = config;
        }
        public string GenerateToken(Profile user, string roleName)
        {

            #region Old
            // var tokenKey = configuration["AppSettings:ApiKey"];
            // var key = Encoding.ASCII.GetBytes(tokenKey);
            // var credentials = new SigningCredentials(new SymmetricSecurityKey(key),
            // SecurityAlgorithms.HmacSha256Signature);
            // var handler =new JwtSecurityTokenHandler();
            // var userClaims = new ClaimsIdentity(new[]
            // {
            //     new Claim("Id",$"{user.Id}"), 
            //     new Claim("Username",$"{user.UserName}"), 
            //     new Claim(ClaimTypes.Email,$"{user.Email}"),
            //     new Claim(ClaimTypes.Role,$"{roleName}"),
            // });

            // var expiresAt = DateTime.UtcNow.AddHours(2);

            // switch (user.Role)
            // {
            //     case AuthRolesEnum.Administrator:
            //         expiresAt = DateTime.UtcNow.AddDays(7);
            //         break;
            //     case AuthRolesEnum.Author:
            //         expiresAt = DateTime.UtcNow.AddDays(5);
            //         break;
            //     case AuthRolesEnum.Guest:
            //         expiresAt = DateTime.UtcNow.AddDays(1);
            //         break;
            // }


            // var descriptor = new SecurityTokenDescriptor()
            // {
            //     Subject = userClaims,
            //     Expires = expiresAt,
            //     SigningCredentials = credentials
            // };

            // var tokenHandler = handler.CreateToken(descriptor);
            // //var token = handler.WriteToken(tokenHandler);
            #endregion
            var apiKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AppSettings:ApiKey"]));

            var userClaims = new List<Claim>(){
                new Claim("Id",$"{user.Id}"), 
                new Claim("Username",$"{user.UserName}"), 
                new Claim(ClaimTypes.Email,$"{user.Email}"),
                new Claim(ClaimTypes.Role,$"{roleName}"),
            };

            var expiresAt = DateTime.UtcNow.AddHours(2);

            switch (user.Role)
            {
                case AuthRolesEnum.Administrator:
                    expiresAt = DateTime.UtcNow.AddDays(7);
                    break;
                case AuthRolesEnum.Author:
                    expiresAt = DateTime.UtcNow.AddDays(5);
                    break;
                case AuthRolesEnum.Guest:
                    expiresAt = DateTime.UtcNow.AddDays(1);
                    break;
            }
            var tokenSecurity = new JwtSecurityToken(
                issuer:configuration["AppSettings:ApiKey"],
                audience:configuration["AppSettings:Audience"],
                expires:expiresAt,
                claims:userClaims,
                signingCredentials:new SigningCredentials(apiKey,SecurityAlgorithms.HmacSha256)
            );
            var token = new JwtSecurityTokenHandler().WriteToken(tokenSecurity);
            return token;
        }
    
    }

}