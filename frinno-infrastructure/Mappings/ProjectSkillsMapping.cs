using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using frinno_core.Entities.Project.ValueObjects;
using frinno_core.Entities.Projects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace frinno_infrastructure.Mappings
{
    public class ProjectSkillsMapping : IEntityTypeConfiguration<ProjectSkills>
    {
        public void Configure(EntityTypeBuilder<ProjectSkills> builder)
        {
            //Configure Indexes
            builder.HasIndex(a => a.Project.Id);
            builder.HasIndex(a => a.Skill.Name);

            //Configure FK_Relationships
            builder.HasKey(a => new { a.ProjectId, a.SkillId });

            //Configure Each Entity
            //Project
            builder.HasOne<Project>()
            .WithMany(at => at.Skills)
            .HasForeignKey(xa => xa.ProjectId);

            //Skills
            builder.HasOne(t => t.Skill)
            .WithMany(ta => ta.Projects)
            .HasForeignKey(ta => ta.SkillId);
        }
    }
}
