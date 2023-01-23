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
            builder.HasIndex(s=>s.ID);
            builder.HasIndex(s=>s.Name);
            builder.HasIndex(s=>s.Profile.ID);
            builder.HasIndex(s=>s.Project.ID);
            //Configure Fkeys
            builder.Property<int>(pj=>pj.Project.ID)
            .HasColumnName("ProjectId");

            builder.Property<int>(pr=>pr.Profile.ID)
            .HasColumnName("ProfileId");

            //M2Ms
            //Profile
            builder.HasOne(s=>s.Profile)
            .WithMany(pr=>pr.Skills)
            .HasForeignKey("ProfileId");

            //Project
            builder.HasOne(s=>s.Project)
            .WithMany(pr=>pr.Skills)
            .HasForeignKey("ProjectId");

            //Tools
            builder.HasMany(s=>s.Tools)
            .WithOne(pr=>pr.Skill)
            .HasForeignKey("ToolId");
        }
    }
}