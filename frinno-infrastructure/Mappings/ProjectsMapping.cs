using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_core.Entities.Projects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace frinno_infrastructure.Mappings
{
    public class ProjectsMapping : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {

            //Config FKs
            builder.Property<string>("ProfileId");

            builder.HasOne(p=>p.Profile)
            .WithMany(pr => pr.Projects)
            .HasPrincipalKey("ProfileId");

        }
    }
}