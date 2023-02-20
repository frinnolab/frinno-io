using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_application.Articles;
using frinno_application.Profiles;
using frinno_core.DTOs;
using frinno_core.Entities.Articles;
using frinno_core.Entities.Profiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace frinno_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticlesService<Article> articlesService;
        private readonly IProfileService<Profile> profileService;
        public ArticlesController(IArticlesService<Article> articles,IProfileService<Profile> profiles)
        {
            articlesService = articles;
            profileService = profiles;
        }
        //Creates a New Article Resource
        [HttpPost("{profileId:int}")]
        public ActionResult<CreateArticleResponse> CreateNew([FromBody] CreateArticleRequest request, int profileId)
        {
            var profileExists = profileService.ProfileExists(new Profile{ ID = profileId});

            if(!profileExists)
            {
                return NotFound(new { Message = "Author not Found!." });
            }

            var author = profileService.FetchSingleById(profileId);


            //Todo, Add Article Specific Validations
            var newArticle = new Article
            {
                Title = request.Title,
                LongText = request.LongText,
                Author = author
            };

            var ArticleResponse = new Article();

            try
            {
                ArticleResponse = articlesService.AddNew(newArticle);

            }
            catch (System.Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

            if (newArticle == null)
            {
                return BadRequest("Failed to Create a Article!.");
            }

            var response = new CreateArticleResponse
            {
                Id = ArticleResponse.ID,
                AuthorId = ArticleResponse.Author.ID,
                Title = ArticleResponse.Title

            };
            return Created("", new { response });
        }

        //Updates a Article Resource
        [HttpPut("{Id:int}/{profileId:int}")]
        public ActionResult<ArticleInfoResponse> UpdateArticle(int Id,int profileId, [FromBody] UpdateArticleRequest request)
        {
            var profileExists = profileService.ProfileExists(new Profile{ ID = profileId});

            if(!profileExists)
            {
                return NotFound(new { Message = "Author not Found!." });
            }

            var author = profileService.FetchSingleById(profileId);
            var Article = articlesService.FetchSingleById(Id);

            if (Article == null)
            {
                return NotFound($"Article: {Id} NotFound!.");
            }

            if (request == null)
            {
                return BadRequest();
            }

            Article.Title = request.Title;
            Article.LongText = request.LongText;

            var ArticleResponse = articlesService.Update(Article);

            return Created(nameof(GetSingle), new { Message = $"Updated {ArticleResponse.ID}" });
        }

        //Removes Single Article Resource
        [HttpDelete("{Id}")]
        public ActionResult<string> DeleteArticle(int Id, int profileId)
        {
            var profileExists = profileService.ProfileExists(new Profile{ ID = profileId});

            if(!profileExists)
            {
                return NotFound(new { Message = "Author not Found!." });
            }

            var author = profileService.FetchSingleById(profileId);
            var data = articlesService.FetchSingleById(Id);

            if (data == null)
            {
                return NotFound("Article Not found");
            }

            articlesService.Remove(data);
            return Ok("Article Delete Success.!");
        }

        //Returns a Article Resource
        [HttpGet("{Id}")]
        public ActionResult<ArticleInfoResponse> GetSingle(int Id, [FromQuery] ArticleInfoRequest query)
        {
            var Article = articlesService.FetchSingleById(Id);
            if (Article == null)
            {
                return NotFound("Article NotFound");
            }

            var response = new ArticleInfoResponse
            {
                Id = Article.ID,
                AuthorId = Article.Author.ID,
                Title = Article.Title,
                LongText = Article.LongText
            };
            return Ok(response);
        }

        //Gets All Articles
        [HttpGet()]
        [AllowAnonymous]
        public ActionResult<DataListResponse<ArticleInfoResponse>> GetAllArticles([FromQuery] ArticleInfoRequest? query, int profileId)
        {
            var Articles = articlesService.FetchAll();
            if (Articles == null)
            {
                return NoContent();
            }

            //Format response
            var ArticleInfos = Articles.ToList();

            var response = new DataListResponse<ArticleInfoResponse>();
            response.Data = ArticleInfos.Select((p)=> new ArticleInfoResponse 
            {
                Id = p.ID,
                AuthorId = p.Author.ID,
                Title = p.Title,
                LongText = p.LongText,
            } ).ToList();
            response.TotalItems = response.Data.Count;
           return Ok(response);

        }
    }
}