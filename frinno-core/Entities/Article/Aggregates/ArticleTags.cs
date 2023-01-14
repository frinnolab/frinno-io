using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace frinno_core.Entities.Article.Aggregates
{
    public class ArticleTags
    {
        public int ArticelId{ get; set; }
        public Articles.Article Article { get; set; }

        public int TagId{ get; set; }
        public Tags.Tag Tag { get; set; }
    }
}