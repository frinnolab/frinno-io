using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_core.Entities.Article.Aggregates;

namespace frinno_core.Entities.Tags
{
    public class Tag : BaseEntity
    {
        public string Name { get; set; }
        public Profiles.Profile Profile { get; set; }
        public IEnumerable<ArticleTags> ArticleTags { get; set; }
    }
}