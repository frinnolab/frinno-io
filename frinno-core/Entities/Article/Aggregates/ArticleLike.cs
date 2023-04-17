using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace frinno_core.Entities.Article.Aggregates
{
    public class ArticleLike : BaseEntity
    {
        public int Likes { get; set; }

        public string likedById { get; set; }
        public Profiles.Profile Profile { get; set; }

        //public int ArticleId { get; set; }
        //public Articles.Article Article { get; set; }
    }
}