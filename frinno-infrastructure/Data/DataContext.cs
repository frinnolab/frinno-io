using frinno_core.Entities.Profiles;
using frinno_core.Entities.Projects;
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
        protected void Configure(ModelBuilder builder)
        {
            //Profile Mapping
            new ProfileMapping().Configure(builder.Entity<Profile>());

        }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Project> Projects { get; set; }
    }
}