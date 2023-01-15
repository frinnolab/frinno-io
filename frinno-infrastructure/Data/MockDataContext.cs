using frinno_core.Entities.Article.Aggregates;
using frinno_core.Entities.Articles;
using frinno_core.Entities.MockModels;
using frinno_core.Entities.Profile.Aggregates;
using frinno_core.Entities.Profiles;
using frinno_infrastructure.Data;
using frinno_infrastructure.Mappings;
using frinno_infrastructure.MockMapping;
using Microsoft.EntityFrameworkCore;

namespace frinno_infrastructure;

public class MockDataContext : DbContext
{
    public MockDataContext(DbContextOptions<MockDataContext> options) : base(options)
    {

    }

    protected void Configure(ModelBuilder builder)
    {
        builder.Entity<MockArticle>()
        .HasIndex(a=>a.Tile);

        new MockUserTokenMapping().Configure(builder.Entity<MockTokens>());
    }

    public DbSet<MockArticle> MockArticles { get; set; }
    public DbSet<MockUser> MockUsers { get; set; }
    public DbSet<MockTokens> MockTokens { get; set; }
}
