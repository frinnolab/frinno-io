using frinno_application.Articles;
using frinno_application.Profiles;
using frinno_core.DTOs;
using frinno_core.Entities;
using frinno_core.Entities.Article.Aggregates;
using frinno_core.Entities.Articles;
using frinno_core.Entities.Profiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace frinno_api.Controllers
{
    [ApiController]
    //[Authorize(Roles = ("Administrator, Author") )]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticlesService<Article> articlesService;
        private readonly IProfileService<Profile> profileService;
        private readonly UserManager<Profile> userManager;
        public ArticlesController(IArticlesService<Article> articles,IProfileService<Profile> profiles,
        UserManager<Profile> users)
        {
            articlesService = articles;
            profileService = profiles;
            userManager = users;
        }
        //Creates a New Article Resource
        [Authorize(Roles = "Administrator, Author")]
        [HttpPost("{profileId}")]
        public async Task<ActionResult<CreateArticleResponse>> CreateNew([FromBody] CreateArticleRequest request, string profileId)
        {
            var profile = await userManager.FindByIdAsync(profileId);

            if(profile == null)
            {
                return NotFound(new { Message = "Profile Not found, Please Sign up!." });
            }

        
            //Todo, Add Article Specific Validations
            var newArticle = new Article
            {
                Title = request.Title,
                LongText = request.LongText,
                Author = profile
            };

            var ArticleResponse = new Article();

            try
            {
                ArticleResponse = await articlesService.AddNew(newArticle);

            }
            catch (System.Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

            var response = new CreateArticleResponse
            {
                Id = ArticleResponse.Id,
                AuthorId = ArticleResponse.Author.Id,
                Title = ArticleResponse.Title

            };
            return Created("", new { response });
        }

        //Updates a Article Resource
        [HttpPut("{Id}/{profileId}")]
        public async Task<ActionResult<ArticleInfoResponse>> UpdateArticle(int Id,string profileId, [FromBody] UpdateArticleRequest request)
        {
            var profile = await userManager.FindByIdAsync(profileId);

            if(profile == null)
            {
                return NotFound(new { Message = "Author Not found, Please Sign up!." });
            }
            var article = articlesService.FetchSingleById(Id);

            if (article == null)
            {
                return NotFound($"Article: {Id} NotFound!.");
            }

            article.Title = request.Title;
            article.LongText = request.LongText;

            var ArticleResponse = articlesService.Update(article);

            return Created(nameof(GetSingle), new { Message = $"Updated {ArticleResponse.Id}" });
        }

        //Removes Single Article Resource
        [HttpDelete("{Id}/{profileId}")]
        public async Task<ActionResult<bool>> DeleteArticle(int Id, string profileId)
        {
            var profile = await userManager.FindByIdAsync(profileId);

            if(profile == null)
            {
                return NotFound(new { Message = "Author Not found, Please Sign up!." });
            }

            var article = articlesService.FetchSingleById(Id);

            if (article == null)
            {
                return NotFound("Article Not found");
            }

            articlesService.Remove(article);
            return NoContent();
        }

        //Returns a Article Resource
        [HttpGet("{Id}"), AllowAnonymous]
        public ActionResult<ArticleInfoResponse> GetSingle(int Id, [FromQuery] ArticleInfoRequest query)
        {
            var Article = articlesService.FetchSingleById(Id);
            if (Article == null)
            {
                return NotFound("Article NotFound");
            }

            var response = new ArticleInfoResponse
            {
                Id = Article.Id,
                AuthorId = Article.Author.Id,
                Title = Article.Title,
                LongText = Article.LongText
            };
            return Ok(response);
        }

        //Gets All Articles
        [HttpGet()]
        [AllowAnonymous]
        public async Task<ActionResult<DataListResponse<ArticleInfoResponse>>> GetAllArticles()
        {
            var Articles = await articlesService.FetchAll();
            if (Articles == null)
            {
                return NoContent();
            }

            //Format response
            var articleInfos = Articles.ToList();

            var response = new DataListResponse<ArticleInfoResponse>(){
                TotalItems = articleInfos.Count,
                Data = articleInfos.Select((a=>new ArticleInfoResponse() 
                {
                    
                    Id = a.Id,
                    AuthorId = a.Author.Id,
                    Title = a.Title,
                    LongText = a.LongText,
                    //TotalLikes = a.Likes.Likes
                })).ToList(),
            };
            
           return Ok(response);

        }
    }
}