using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_core.Entities.Article.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace frinno_infrastructure.Mappings.ArticlesMapping
{
    public class ArticleLikesMapping : IEntityTypeConfiguration<ArticleLike>
    {
        public void Configure(EntityTypeBuilder<ArticleLike> builder)
        {
            //Columns
            builder.Property<string>("LikedById")
            .HasColumnName("LikedById");
            builder.Property<int>("ArticleId")
            .HasColumnName("ArticleId");

            //Profile
            builder.HasOne(p=>p.Profile)
            .WithOne();

            builder.HasOne(p=>p.Article)
            .WithOne();
        }
    }
}