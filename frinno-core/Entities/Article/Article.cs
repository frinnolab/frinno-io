using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_core.Entities.Article.Aggregates;
using frinno_core.Entities.Profile.Aggregates;

namespace frinno_core.Entities.Articles
{
    public class Article : BaseEntity
    {
        public string Title { get; set; }

        public string LongText { get; set; }

        public Profiles.Profile Author { get; set; }
        public List<ArticleTags> ArticleTags { get; set; }

        //public ArticleLike Likes { get; set; }


    }
}