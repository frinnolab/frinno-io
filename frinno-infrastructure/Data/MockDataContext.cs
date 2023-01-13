using frinno_core.Entities.Article.Aggregates;
using frinno_core.Entities.Articles;
using frinno_core.Entities.MockModels;
using frinno_core.Entities.Profile.Aggregates;
using frinno_core.Entities.Profiles;
using frinno_infrastructure.Data;
using frinno_infrastructure.Mappings;
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
    }

    public DbSet<MockArticle> MockArticles { get; set; }
}
