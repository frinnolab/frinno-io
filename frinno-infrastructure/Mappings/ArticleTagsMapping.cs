using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_core.Entities.Article.Aggregates;
using frinno_core.Entities.Articles;
using Microsoft.EntityFrameworkCore;

namespace frinno_infrastructure.Mappings
{
    public class ArticleTagsMapping : IEntityTypeConfiguration<ArticleTags>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ArticleTags> builder)
        {
            //Configure Indexes
            builder.HasIndex(a=>a.ArticelId);
            builder.HasIndex(a=>a.Article.Title);
            builder.HasIndex(t=>t.TagId);
            builder.HasIndex(t=>t.Tag.Name);
            
            //Configure FK_Relationships
            builder.HasKey(a => new { a.ArticelId, a.TagId});

            //Configure Each Entity
            //Articles
            builder.HasOne<Article>()
            .WithMany(at => at.ArticleTags)
            .HasForeignKey(at => at.ArticelId);

            //Tags
            builder.HasOne(t => t.Tag)
            .WithMany(ta => ta.ArticleTags)
            .HasForeignKey(ta => ta.TagId);
        }
    }
}