using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace frinno_core.DTOs
{
    //Login Request
    public record LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    //Login Response
    public record LoginResponse
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public string Token { get; set; }
    }
    //User Response
    public record UserResponse
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string hashedPassword { get; set; }
    }
    
    //Register Request
    public record RegisterRequest
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string Password { get; set; }
    }
    //Register Response
    public record RegisterResponse
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
    }
}