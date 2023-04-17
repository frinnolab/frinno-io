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

    public record CreateArticleLikeRequest
    {
        public bool IsLiked { get; set; } = false;
    }
    //Create a Profile Request
    
    public record CreateArticleResponse
    {
        public int Id { get; set; }
        public string AuthorId { get; set; }
        public string Title { get; set; }
        //public int TotalLikes { get; set; } = 0;

    }

    //Article Single Resource Request
    public record ArticleInfoRequest
    {
        public string Title { get; set; } = string.Empty;
    }

    //Article Response.
    public record ArticleInfoResponse
    {
        public int Id { get; set; }
        public string AuthorId { get; set; }
        public string Title { get; set; }
        public string LongText { get; set; }
        //public int TotalLikes { get; set; } = 0;

    }

    //Article Update Request
    public record UpdateArticleRequest : CreateArticleRequest
    {
        
    }
}