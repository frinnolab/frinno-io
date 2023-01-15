using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_core.Entities.Profile.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace frinno_infrastructure.Mappings
{
    public class ProfileArticlesMapping : IEntityTypeConfiguration<ProfileArticles>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ProfileArticles> builder)
        {
            //Configure Indexes
            builder.HasIndex(a=>a.ArticleId);
            builder.HasIndex(a=>a.Article.Title);
            builder.HasIndex(a=>a.ProfileId);
            builder.HasIndex(a=>a.Profile.FirstName);
            builder.HasIndex(a=>a.Profile.LastName);

            //Configure F_K Relathiships
            builder.HasKey(a => new { a.ArticleId, a.ProfileId});
            //Profiles
            builder.HasOne(p=>p.Profile)
            .WithMany()
            .HasForeignKey(p=>p.ProfileId);

            //Articles
            builder.HasOne(a=>a.Article)
            .WithMany()
            .HasForeignKey(a=>a.ArticleId);

        }
    }
}