using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using frinno_core.Entities;

namespace frinno_core.DTOs
{
    //Create a Profile Request
    
    public record CreateAProfileRequest
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public AuthRolesEnum Role { get; set; }
        public ProfileAddressInfo AddressInfo { get; set; }
    }
    public record CreateAProfileResponse: CreateAProfileRequest
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
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
        public string Role { get; set; }
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