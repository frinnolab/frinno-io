using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_core.Entities.Article.Aggregates;
using frinno_core.Entities.Articles;
using frinno_core.Entities.Profile;
using frinno_core.Entities.Profiles;
using frinno_core.Entities.Projects;
using frinno_core.Entities.Resumes;
using frinno_core.Entities.user;
using frinno_infrastructure.Mappings;
using Microsoft.EntityFrameworkCore;

namespace frinno_infrastructure.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options):base(options)
        {
            
        }



        protected void Configure(ModelBuilder builder)
        {
            //Artilces Mapping
            new ArticlesMapping().Configure(builder.Entity<Article>());

            //Profile Mapping
            new ProfileMapping().Configure(builder.Entity<Profile>());

            //Configure ArticleTags MTM
            new ArticleTagsMapping().Configure(builder.Entity<ArticleTags>());

        }
        //public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Resume> Resumes { get; set; }

    }
}