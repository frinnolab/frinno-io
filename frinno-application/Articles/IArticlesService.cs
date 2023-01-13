using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_application.Generics;
using frinno_core.Entities;
using frinno_core.Entities.Tags;

namespace frinno_application.Articles
{
    public interface IArticlesService <Article> : IMasterService<frinno_core.Entities.Articles.Article>
    {
        Article CreateAticlesWithTag(int articleId, Tag tag);
        Article CreateAticlesWithTags(int articleId, Tag[] tags);
    }
}