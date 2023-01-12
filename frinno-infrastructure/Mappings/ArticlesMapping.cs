using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_core.Entities.Articles;
using Microsoft.EntityFrameworkCore;

namespace frinno_infrastructure.Mappings
{
    public class ArticlesMapping : IEntityTypeConfiguration<Article>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Article> builder)
        {
            //Configure FKs
            builder.Property<string>("ProfileId");
            //Configure Relationships
            builder.HasOne(p => p.Profile)
            .WithMany(a=>a.Articles)
            .HasForeignKey("ProfileId");
        }
    }
}