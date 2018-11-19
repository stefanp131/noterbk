using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Noter.API.Models;
using Noter.DAL.Entities;
using Noter.DAL.Helpers;
using Noter.DAL.Repository;
using System;
using System.Collections.Generic;

namespace Noter.API.Controllers
{
    [Route("api/topics/{topicId}/commentaries")]
    [Authorize]
    public class CommentariesController : Controller
    {
        private readonly INoterRepository noterRepository;
        private readonly IUrlHelper urlHelper;

        public CommentariesController(INoterRepository noterRepository, IUrlHelper urlHelper)
        {
            this.noterRepository = noterRepository;
            this.urlHelper = urlHelper;
        }

        [HttpGet(Name = "GetCommentariesForTopic")]
        public IActionResult GetCommentariesForTopic(Guid topicId, CommentariesResourceParameters commentariesResourceParameters)
        {
            var commentaryEntities = noterRepository.GetCommentariesForTopic(topicId, commentariesResourceParameters);
            var commentaryDtos = Mapper.Map<IEnumerable<CommentaryDto>>(commentaryEntities);

            var previousPageLink = commentaryEntities.HasPrevious ?
                CreateCommentariesResourceUri(commentariesResourceParameters, ResourceUriType.PreviousPage)
                : null;

            var nextPageLink = commentaryEntities.HasNext ?
                CreateCommentariesResourceUri(commentariesResourceParameters, ResourceUriType.NextPage)
                : null;

            var paginationMetadata = new
            {
                totalCount = commentaryEntities.TotalCount,
                pageSize = commentaryEntities.PageSize,
                currentPage = commentaryEntities.CurrentPage,
                totalPages = commentaryEntities.TotalPages,
                previousPage = previousPageLink,
                nextPage = nextPageLink
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));

            return Ok(commentaryDtos);
        }

        
        private string CreateCommentariesResourceUri(CommentariesResourceParameters commentariesResourceParameters, ResourceUriType resourceUriType)
        {
            switch(resourceUriType)
            {
                case ResourceUriType.PreviousPage: return urlHelper.Link("GetCommentariesForTopic",
                    new
                    {
                        pageNumber = commentariesResourceParameters.PageNumber - 1,
                        pageSize = commentariesResourceParameters.PageSize
                    });
                case ResourceUriType.NextPage: return urlHelper.Link("GetCommentariesForTopic",
                    new
                    {
                        pageNumber = commentariesResourceParameters.PageNumber + 1,
                        pageSize = commentariesResourceParameters.PageSize
                    });
                default:
                    return urlHelper.Link("GetCommentariesForTopic",
                    new
                    {
                        pageNumber = commentariesResourceParameters.PageNumber,
                        pageSize = commentariesResourceParameters.PageSize
                    });
            }           
        }

        [HttpGet("{id}")]
        public IActionResult GetCommentaryForTopic(Guid topicId, Guid id)
        {
            if(!noterRepository.TopicExists(topicId))
            {
                return NotFound();
            }

            if(!noterRepository.CommentaryExistsForTopic(topicId, id))
            {
                return NotFound();
            }

            var commentaryEntity = noterRepository.GetCommentaryForTopic(topicId, id);
            var commentaryDto = Mapper.Map<CommentaryDto>(commentaryEntity);

            return Ok(commentaryDto);
        }

        [HttpPost]
        public IActionResult CreateCommentaryForTopic(Guid topicId, [FromBody] CommentaryForCreation commentaryForCreation)
        {
            if(!noterRepository.TopicExists(topicId))
            {
                return NotFound();
            }

            if(commentaryForCreation == null)
            {
                return BadRequest();
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var commentaryEntity = Mapper.Map<Commentary>(commentaryForCreation);
            noterRepository.CreateCommentaryForTopic(commentaryEntity);

            noterRepository.Save();

            return Ok(); // TO DO - to be implemented created at route
        }

        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdateCommentaryForTopic(Guid topicId, Guid id, [FromBody] JsonPatchDocument<CommentaryForUpdate> patchDoc)
        {
            if(!noterRepository.TopicExists(topicId))
            {
                return NotFound();
            }

            if(!noterRepository.CommentaryExistsForTopic(topicId, id))
            {
                return NotFound();
            }

            if(patchDoc == null)
            {
                return BadRequest();
            }

            var commentaryEntity = noterRepository.GetCommentaryForTopic(topicId, id);
            var commentaryDto = Mapper.Map<CommentaryForUpdate>(commentaryEntity);
            patchDoc.ApplyTo(commentaryDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TryValidateModel(commentaryDto);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mapper.Map(commentaryDto, commentaryEntity);

            noterRepository.Save();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCommentaryForTopic(Guid topicId, Guid id)
        {
            if(!noterRepository.TopicExists(topicId))
            {
                return NotFound();
            }

            if(!noterRepository.CommentaryExistsForTopic(topicId, id))
            {
                return NotFound();
            }

            noterRepository.DeleteCommentaryForTopic(topicId, id);

            noterRepository.Save();            

            return NoContent();        
        }
    }
}
