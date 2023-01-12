using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_core.Entities.Resumes;
using Microsoft.EntityFrameworkCore;

namespace frinno_infrastructure.Mappings
{
    public class ResumeMapping : IEntityTypeConfiguration<Resume>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Resume> builder)
        {
            builder.Property<string>("ProfileId");
            
            builder.HasOne(p=>p.Profile)
            .WithOne()
            .HasForeignKey("ProfileId");
        }
    }
}