using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_application.Articles;
using frinno_application.Tags;
using frinno_core.DTOs;
using frinno_core.Entities.Article.Aggregates;
using frinno_core.Entities.Articles;
using frinno_core.Entities.Profiles;
using frinno_core.Entities.Tags;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace frinno_api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly ITagsService<Tag> tagsService;
        private readonly IArticlesService<Article> articleService;
        private readonly UserManager<Profile> userManager;

        public TagsController(
            ITagsService<Tag> tags, 
            UserManager<Profile> manager,
            IArticlesService<Article> articleService_)
        {
            tagsService = tags;
            userManager = manager;
            articleService = articleService_;
        }
        //Creates a New Tag Resource
        [HttpPost("{profileId}")]
        [Authorize(Roles ="Administrator, Author")]
        public async Task<ActionResult<TagInfoResponse>> CreateNew(string profileId, [FromBody] CreateTagRequest request)
        {
            //Find Profile
            var profile = await userManager.FindByIdAsync(profileId);

            if(profile == null)
            {
                return NotFound($"Profile Not found!.");
            }

            //Add new Tag
            var newTag = new Tag
            {
                Name = request.Name,
                Profile = profile,
            };

            //Tag to Articles

            var articlesToTag = new List<ArticleTags>();

            if(request.articleIds.Length > 0)
            {
                foreach (var aId in request.articleIds)
                {
                    if(aId>0)
                    {
                        var article = articleService.FetchSingleById(aId);
                        if(article !=  null)
                        {
                            articlesToTag.Add(new ArticleTags() { Article = article });
                        }
                    }
                }
            }

            newTag.ArticleTags = articlesToTag ?? null;

            try
            {
                var data = await tagsService.AddNew(newTag);
                newTag = data;

            }
            catch (System.Exception ex)
            {
                return BadRequest($"Failed to create Tag with Error: {ex.Message}");
            }

            var response = new TagInfoResponse
            {
                Id = newTag.Id,
                Name = newTag.Name,
                ProfileId = newTag.Profile.Id,
                TotalArtilcesUsed = newTag.ArticleTags.Select((a) => a.Article).Count()
            };
            return Created("", response);
        }

        //Updates a Tag Resource
        [HttpPut("{Id}/{profileId}")]
        [Authorize(Roles = "Administrator, Author")]
        public async Task<ActionResult<TagInfoResponse>> UpdateTag(int Id, string profileId, [FromBody] UpdateTagRequest request)
        {
            //Find Profile
            var profile = await userManager.FindByIdAsync(profileId);

            if (profile == null)
            {
                return NotFound($"Profile Not found!.");
            }

            var Tag = tagsService.FetchSingleById(Id);

            if (Tag == null)
            {
                return NotFound($"Tag: NotFound!.");
            }


            Tag.Name = request.Name;
            Tag.Profile = profile;
            Tag.Modified = DateTime.UtcNow;

            var articlesToTag = new List<ArticleTags>();

            if (request.articleIds.Length > 0)
            {
                articlesToTag = Tag.ArticleTags.ToList();
                foreach (var aId in request.articleIds)
                {
                    if (aId > 0)
                    {
                        var article = articleService.FetchSingleById(aId);

                        if(article !=null)
                        {
                            var currentArticle = Tag.ArticleTags.Select((a => a.Article)).SingleOrDefault((ay=>ay.Id == article.Id));
                            if(currentArticle == null)
                            {
                                //Add tag to existing articles tags
                                articlesToTag.Add(new ArticleTags() { Article = article });
                            }
                        }

                    }
                }

                Tag.ArticleTags = articlesToTag;
            }

            try
            {
                var data = await tagsService.Update(Tag);
                Tag = data;

            }
            catch (Exception ex)
            {

                return BadRequest($"Failed to update Tag with Error: {ex.Message}");
            }

            //Format Response
            var response = new TagInfoResponse
            {
                Id = Tag.Id,
                Name = Tag.Name,
                ProfileId = Tag.Profile.Id,
                TotalArtilcesUsed = Tag.ArticleTags.Select((a) => a.Article).Count()
            };


            return Created("", response);
        }

        //Removes Single Tag Resource
        [HttpDelete("{Id}/{profileId}")]
        [Authorize(Roles = "Administrator, Author")]
        public async Task<ActionResult<bool>> DeleteTag(int Id, string profileId)
        {
            var profile = await userManager.FindByIdAsync(profileId);

            if (profile == null)
            {
                return NotFound($"Profile Not found!.");
            }

            var Tag = tagsService.FetchSingleById(Id);

            if (Tag == null)
            {
                return NotFound($"Tag: NotFound!.");
            }

            try
            {
                tagsService.Remove(Tag);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to remove Tag with Error: {ex.Message}");
            }
            return NoContent();
        }

        //Returns a Tag Resource
        [HttpGet("{Id}")]
        [AllowAnonymous]
        public ActionResult<TagInfoResponse> GetSingle(int Id, [FromQuery] TagInfoRequest query)
        {
            var Tag = tagsService.FetchSingleById(Id);
            if (Tag == null)
            {
                return NotFound("Tag NotFound");
            }

            var response = new TagInfoResponse
            {
                Id = Tag.Id,
                Name = Tag.Name,
                ProfileId = Tag.Profile.Id,
                TotalArtilcesUsed = Tag.ArticleTags.Select(a=>a.Article).Count()
            };
            return Ok(response);
        }

        //Gets All tags
        [HttpGet()]
        [AllowAnonymous]
        public async Task<ActionResult<DataListResponse<TagInfoResponse>>> GetAlltags([FromQuery] TagInfoRequest? query)
        {
            var tags = await tagsService.FetchAll();
            if (tags == null)
            {
                return NoContent();
            }

            //Format response
            var TagInfos = tags.ToList();

            var response = new DataListResponse<TagInfoResponse>();
            response.Data = TagInfos.Select((p)=> new TagInfoResponse 
            {
                Id = p.Id,
                Name = p.Name,
                ProfileId = p.Profile.Id,
                TotalArtilcesUsed = p.ArticleTags.Select(a=>a.Article).Count()
            } ).ToList();
            response.TotalItems = response.Data.Count;
           return Ok(response);

        }
    }
}