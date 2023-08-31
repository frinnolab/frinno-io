using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_core.Entities.Article.Aggregates;
using frinno_core.Entities.Articles;
using frinno_core.Entities.FileAsset;
using frinno_core.Entities.Profile;
using frinno_core.Entities.Profiles;
using frinno_core.Entities.Projects;
using frinno_core.Entities.Resumes;
using frinno_core.Entities.Skill;
using frinno_core.Entities.Tags;
using frinno_core.Entities.user;
using frinno_infrastructure.Mappings;
using frinno_infrastructure.Mappings.ArticlesMapping;
using frinno_infrastructure.Mappings.SkillsMapping;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace frinno_infrastructure.Data
{
    public class DataContext : IdentityDbContext<Profile>
    {

        public DataContext(DbContextOptions<DataContext> options):base(options)
        {
            
        }
        protected static void Configure(ModelBuilder builder)
        {
            //Artilces Mapping
            new ArticlesMapping().Configure(builder.Entity<Article>());

            //Profile Mapping
            new ProfileMapping().Configure(builder.Entity<Profile>());

            //Configure ArticleTags MTM
            new ArticleTagsMapping().Configure(builder.Entity<ArticleTags>());

            new ArticleLikesMapping().Configure(builder.Entity<ArticleLike>());

            //Configure Skills Mapping
            new SkillsMapping().Configure(builder.Entity<Skill>());

            //Configure File Assets
            new FileAssetsMappings().Configure(builder.Entity<FileAsset>());

        }
        //public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Resume> Resumes { get; set; }

        public DbSet<FileAsset> FileAssets { get; set; }
    }
}