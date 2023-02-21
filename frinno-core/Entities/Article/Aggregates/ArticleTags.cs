using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace frinno_core.Entities.Article.Aggregates
{
    public class ArticleTags : BaseEntity
    {
        public string ArticelId{ get; set; }
        public Articles.Article Article { get; set; }

        public string TagId{ get; set; }
        public Tags.Tag Tag { get; set; }
    }
}