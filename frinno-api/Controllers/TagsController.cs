using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using frinno_application.Tags;
using frinno_core.DTOs;
using frinno_core.Entities.Tags;
using Microsoft.AspNetCore.Mvc;

namespace frinno_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly ITagsService<Tag> tagsService;
        public TagsController(ITagsService<Tag> tags)
        {
            tagsService = tags;
        }
        //Creates a New Tag Resource
        [HttpPost()]
        public ActionResult<TagInfoResponse> CreateNew([FromBody] CreateTagRequest request)
        {
            //Todo, Add Tag Specific Validations
            var newTag = new Tag
            {
                Name = request.Name,
            };

            //Add Profile if not null

            var TagResponse = new Tag();

            try
            {
                TagResponse = tagsService.AddNew(newTag);

            }
            catch (System.Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

            if (newTag == null)
            {
                return BadRequest("Failed to Create a Tag!.");
            }

            var response = new TagInfoResponse
            {
                Id = TagResponse.ID,
            };
            return Created(nameof(GetSingle), new { Message = $"Tag Created with ID: {response.Id}" });
        }

        //Updates a Tag Resource
        [HttpPut("{Id}")]
        public ActionResult<TagInfoResponse> UpdateTag(int Id, [FromBody] UpdateTagRequest request)
        {
            var Tag = tagsService.FetchSingleById(Id);

            if (Tag == null)
            {
                return NotFound($"Tag: {Id} NotFound!.");
            }

            if (request == null)
            {
                return BadRequest();
            }

            Tag.Name = request.Name;

            var TagResponse = tagsService.Update(Tag);

            return Created(nameof(GetSingle), new { Message = $"Updated {TagResponse.ID}" });
        }

        //Removes Single Tag Resource
        [HttpDelete("{Id}")]
        public ActionResult<string> DeleteTag(int Id)
        {
            var data = tagsService.FetchSingleById(Id);

            if (data == null)
            {
                return NotFound("Tag Not found");
            }

            tagsService.Remove(data);
            return Ok("Tag Delete Success.!");
        }

        //Returns a Tag Resource
        [HttpGet("{Id}")]
        public ActionResult<TagInfoResponse> GetSingle(int Id, [FromQuery] TagInfoRequest query)
        {
            var Tag = tagsService.FetchSingleById(Id);
            if (Tag == null)
            {
                return NotFound("Tag NotFound");
            }

            var response = new TagInfoResponse
            {
                Id = Tag.ID,
                Name = Tag.Name,
                TotalArtilcesUsed = Tag.ArticleTags.Select(a=>a.Article).ToList().Count
            };
            return Ok(response);
        }

        //Gets All tags
        [HttpGet()]
        public ActionResult<DataListResponse<TagInfoResponse>> GetAlltags([FromQuery] TagInfoRequest? query)
        {
            var tags = tagsService.FetchAll();
            if (tags == null)
            {
                return NoContent();
            }

            //Format response
            var TagInfos = tags.ToList();

            var response = new DataListResponse<TagInfoResponse>();
            response.Data = TagInfos.Select((p)=> new TagInfoResponse 
            {
                Id = p.ID,
                Name = p.Name,
                TotalArtilcesUsed = p.ArticleTags.Select(a=>a.Article).ToList().Count
            } ).ToList();
            response.TotalItems = response.Data.Count;
           return Ok(response);

        }
    }
}