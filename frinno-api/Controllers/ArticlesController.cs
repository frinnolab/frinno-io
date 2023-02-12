using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_application.Articles;
using frinno_core.DTOs;
using frinno_core.Entities.Articles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace frinno_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticlesService<Article> articlesService;
        public ArticlesController(IArticlesService<Article> articles)
        {
            articlesService = articles;
        }
        //Creates a New Article Resource
        [HttpPost("{profileId}")]
        public ActionResult<ArticleInfoResponse> CreateNew([FromBody] CreateArticleRequest request, int profileId)
        {
            //Todo, Add Article Specific Validations
            var newArticle = new Article
            {
                Title = request.Title,
                LongText = request.LongText
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

            var response = new ArticleInfoResponse
            {
                Id = ArticleResponse.ID,
            };
            return Created(nameof(GetSingle), new { Message = $"Article Created with ID: {response.Id}" });
        }

        //Updates a Article Resource
        [HttpPut("{Id}/{profileId}")]
        public ActionResult<ArticleInfoResponse> UpdateArticle(int Id,int profileId, [FromBody] UpdateArticleRequest request)
        {
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
        [HttpDelete("{Id}/{profileId}")]
        public ActionResult<string> DeleteArticle(int Id, int profileId)
        {
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
                Title = p.Title,
                LongText = p.LongText,
                TotalArticlesTags = p.ArticleTags.Count
            } ).ToList();
            response.TotalItems = response.Data.Count;
           return Ok(response);

        }
    }
}