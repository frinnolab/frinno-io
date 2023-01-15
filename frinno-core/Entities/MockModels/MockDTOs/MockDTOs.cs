using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace frinno_core.Entities.MockModels.MockDTOs
{
    //Configure Mock Requests and Responses

    //Register
    public record MockRegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        //public MockRoles Role { get; set; }
    }

    public record MockRegisterResponse
    {
        public string Fullname { get; set; }
        public MockRoles Role { get; set; }
    }

    //Login
    public record MockLoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public record MockLoginResponse
    {
        public string Email { get; set; }
        public MockTokenResponse Token { get; set; }

    }
    public record MockTokenResponse
    {
        public string ApiToken { get; set; }

    }

    
}