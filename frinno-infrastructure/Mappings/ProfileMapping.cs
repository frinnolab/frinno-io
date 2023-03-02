using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            builder.OwnsOne(c=>c.User);
            builder.OwnsOne(c=>c.Address);
            // builder.Navigation(c=>c.Address).IsRequired();

            //Configure Indexes
            builder.HasIndex(p => p.FirstName);
            builder.HasIndex(p => p.LastName);
        }
    }
}