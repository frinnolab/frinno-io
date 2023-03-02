using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_core.Entities.Articles;
using Microsoft.EntityFrameworkCore;

namespace frinno_infrastructure.Mappings.ArticlesMapping
{
    public class ArticlesMapping : IEntityTypeConfiguration<Article>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Article> builder)
        {
            //Configure Indexes
            builder.HasIndex(a=>a.Title);

            //Profile
            builder.Property<string>(p=> p.Author.Id)
            .HasColumnName("ProfileId");

            builder.HasOne(p=>p.Author)
            .WithMany()
            .HasForeignKey("ProfileId");

            
        }
    }
}