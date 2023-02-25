using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace frinno_core.DTOs
{
    //Create a Profile Request
    
    public record CreateAProfileRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ProfileAddressInfo AddressInfo { get; set; }
    }
    public record CreateAProfileResponse
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public ProfileAddressInfo AddressInfo { get; set; }
    }

    public record ProfileAddressInfo
    {
        public string Mobile { get; set; }
        public string City { get; set; }
    }
    //Profile Single Resource Request
    public record ProfileInfoRequest
    {
        public string Fullname { get; set; }
        public string Email { get; set; }
        
    }

    //Profile Response.
    public record ProfileInfoResponse
    {
        public string Id { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public ProfileAddressInfo AddressInfo { get; set; }
        public int TotalArticles { get; set; } = 0;
        public int TotalProjects { get; set; } = 0;
        public int TotalResumes { get; set; } = 0;
        public int TotalSkills { get; set; } = 0;
    }

    //Profile Update Request
    public record UpdateProfileRequest : CreateAProfileRequest
    {
        public string Id { get; set; }
    }
}