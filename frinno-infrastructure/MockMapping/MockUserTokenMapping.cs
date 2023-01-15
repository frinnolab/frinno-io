using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_core.Entities.MockModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace frinno_infrastructure.MockMapping
{
    public class MockUserTokenMapping : IEntityTypeConfiguration<MockTokens>
    {
        public void Configure(EntityTypeBuilder<MockTokens> builder)
        {
            builder.Property<string>("UserId");

            builder.HasOne(u=>u.User)
            .WithOne()
            .HasForeignKey("UserId");
        }
    }
}