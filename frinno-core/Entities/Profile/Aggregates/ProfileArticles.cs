using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace frinno_core.Entities.Profile.Aggregates
{
    public class ProfileArticles : BaseEntity
    {
        public string ProfileId { get; set; }
        public Profiles.Profile Profile { get; set; }

        public string ArticleId { get; set; }
        public Articles.Article Article { get; set; }
    }
}