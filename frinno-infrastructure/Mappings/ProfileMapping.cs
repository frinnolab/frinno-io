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
            //Configure Owns
            builder.OwnsOne(c=>c.User);
            builder.OwnsOne(c=>c.Address);
        }
    }
}