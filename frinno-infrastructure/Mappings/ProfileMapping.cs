using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;
using frinno_core.Entities.Profile;
using frinno_core.Entities.Profiles;
using frinno_core.Entities.user;

namespace frinno_infrastructure.Mappings
{
    public class ProfileMapping : Microsoft.EntityFrameworkCore.IEntityTypeConfiguration<Profile>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Profile> builder)
        {
            // //Configure Owns Properties
            builder.Property<string>(p=>Enum.GetName(p.Role));
            // builder.Property<string>("Email");
            // builder.Property<string>("Password");
            // builder.Property<string>("Mobile");
            // builder.Property<string>("City");
            //Configure Owns
            builder.OwnsOne(c=>c.Address);
            // builder.Navigation(c=>c.Address).IsRequired();

            //Configure Indexes
            builder.HasIndex(p => p.FirstName);
            builder.HasIndex(p => p.LastName);

            //Default Data
           // builder.HasData(new Profile()
            //{
            //    FirstName = "Frank Leons",
            //    LastName = "Malisawa",
            //    Address = new frinno_core.Entities.Profile.ValueObjects.Address()
            //    {
            //        City = "DSM",
            //        Mobile = "0756589799"
            //    },
            //    Role = frinno_core.Entities.AuthRolesEnum.Administrator,
            //    Email = "dev.frinno@gmail.com",
            //    PasswordHash = BCrypt.Net.BCrypt.HashPassword("@SuspectZer0"),
            //    UserName = "frinno"
            //});
        }
    }
}