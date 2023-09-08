using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_core.Entities.FileAsset;
using frinno_core.Entities.Profile;
using frinno_core.Entities.Profiles;
using frinno_core.Entities.Projects;
using frinno_core.Entities.Resumes;
using frinno_infrastructure.Mappings;
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
            //Profile Mapping
            new ProfileMapping().Configure(builder.Entity<Profile>());

            //Project Mapping
            new ProjectsMapping().Configure(builder.Entity<Project>());

            //Configure File Assets
            new FileAssetsMappings().Configure(builder.Entity<FileAsset>());

        }
        //public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Resume> Resumes { get; set; }

        public DbSet<FileAsset> FileAssets { get; set; }
    }
}