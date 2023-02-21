using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_core.Entities.Skill;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace frinno_infrastructure.Mappings.SkillsMapping
{
    public class SkillsMapping : IEntityTypeConfiguration<Skill>
    {
        public void Configure(EntityTypeBuilder<Skill> builder)
        {
            //Config Indexes
            builder.HasIndex(s=>s.Id);
            builder.HasIndex(s=>s.Name);
            builder.HasIndex(s=>s.Profile.Id);
            builder.HasIndex("ProjectId");
            //Configure Fkeys
            builder.Property<string>("ProjectId")
            .HasColumnName("ProjectId");

            builder.Property<string>(pr=> pr.Profile.Id)
            .HasColumnName("ProfileId");

            //M2Ms
            //Profile
            builder.HasOne(s=>s.Profile)
            .WithMany(pr=>pr.Skills)
            .HasForeignKey("ProfileId");

            //Projects
            builder.HasMany(pr=>pr.Projects)
            .WithOne()
            .HasForeignKey("ProjectId");
        }
    }
}