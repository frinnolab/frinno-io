using frinno_core.Entities.Article.Aggregates;
using frinno_core.Entities.Articles;
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
        //Artilces Mapping
        new ArticlesMapping().Configure(builder.Entity<Article>());

        //Profile Mapping
        new ProfileMapping().Configure(builder.Entity<Profile>());

        //Configure ArticleTags MTM
        new ArticleTagsMapping().Configure(builder.Entity<ArticleTags>());
        
        new ProfileArticlesMapping().Configure(builder.Entity<ProfileArticles>());

    }

    public DbSet<Profile> Profiles { get; set; }
    public DbSet<Article> Articles { get; set; }
}
