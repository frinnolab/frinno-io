using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace frinno_core.DTOs
{
    //Create a Article Request
    
    public record CreateArticleRequest
    {
        public string Title { get; set; } = string.Empty;
        public string LongText { get; set; } = string.Empty;
    }
    //Create a Profile Request
    
    public record CreateArticleResponse
    {
        public int Id { get; set; }
    }
    //Article Single Resource Request
    public record ArticleInfoRequest
    {
        public int? Id { get; set; }
        public string Title { get; set; } = string.Empty;
    }

    //Article Response.
    public record ArticleInfoResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string LongText { get; set; }
        public int TotalArticlesTags { get; set; } = 0;
    }

    //Article Update Request
    public record UpdateArticleRequest : CreateArticleRequest
    {
        public int? Id { get; set; }
        public int? ProfileId { get; set; }
    }
}