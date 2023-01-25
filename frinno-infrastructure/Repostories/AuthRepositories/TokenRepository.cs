using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using frinno_application.Authentication;
using frinno_core.DTOs;
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
        public string Generate(UserResponse user)
        {
            var tokenKey = configuration.GetSection("AppSettings:ApiKey").ToString();
            var key = Encoding.ASCII.GetBytes(tokenKey);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature);
            var handler =new JwtSecurityTokenHandler();
            var userClaims = new ClaimsIdentity(new[]
            {
                new Claim("Id",$"{user.Id}"), 
                new Claim("Firstname",$"{user.FirstName}"), 
                new Claim("Email",$"{user.Email}"),
            });

            var descriptor = new SecurityTokenDescriptor()
            {
                Subject = userClaims,
                Expires = DateTime.UtcNow.AddDays(12),
                SigningCredentials = credentials

            };

            var token = handler.CreateToken(descriptor);
            return handler.WriteToken(token);
        }

        public int? Validate(string token)
        {
            if(string.IsNullOrEmpty(token))
            {
                return null;
            }

            var handler = new JwtSecurityTokenHandler();

            var apiKey = Encoding.ASCII.GetBytes(configuration.GetSection("AppSettings:ApiKey").ToString());

            try
            {
                handler.ValidateToken(token, new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(apiKey),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validToken);

                var accessToken = (JwtSecurityToken)validToken;
                var userId = int.Parse(accessToken.Claims.First(c=>c.Type == "Id").Value);

                return userId;
            }
            catch (System.Exception)
            {
                
                return null;
            }
        }
    }
}