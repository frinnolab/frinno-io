using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_core.Entities.Profiles;
using frinno_core.Entities.Tags;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace frinno_infrastructure.Mappings
{
    public class TagsMapping : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.HasIndex(t=>t.Name);

            //Shadow FK
            builder.Property<int>("ProfileId");

            //COnfigure FK
            builder.HasOne<Profile>(p=>p.Profile)
            .WithOne()
            .HasForeignKey("ProfileId");
        }
    }
}