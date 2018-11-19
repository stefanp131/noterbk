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
    [Route("api/topics")]
    [Authorize]
    public class TopicsController : Controller
    {
        private readonly INoterRepository noterRepository;
        private readonly IUrlHelper urlHelper;

        public TopicsController(INoterRepository noterRepository, IUrlHelper urlHelper)
        {
            this.noterRepository = noterRepository;
            this.urlHelper = urlHelper;
        }

        [HttpGet(Name = "GetTopics")]
        public IActionResult GetTopics(TopicsResourceParameters topicsResourceParameters)
        {
            var topicEntities = noterRepository.GetTopics(topicsResourceParameters);

            var previousPageLink = topicEntities.HasPrevious ?
                CreateTopicsResourceUri(topicsResourceParameters, ResourceUriType.PreviousPage) 
                : null;

            var nextPageLink = topicEntities.HasNext ?
                CreateTopicsResourceUri(topicsResourceParameters, ResourceUriType.NextPage)
                : null;

            var paginationMetadata = new
            {
                totalCount = topicEntities.TotalCount,
                pageSize = topicEntities.PageSize,
                currentPage = topicEntities.CurrentPage,
                totalPages = topicEntities.TotalPages,
                previousPage = previousPageLink,
                nextPage = nextPageLink
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));

            var topicDtos = Mapper.Map<IEnumerable<TopicDto>>(topicEntities);
            return Ok(topicDtos);
        }

        private string CreateTopicsResourceUri(TopicsResourceParameters topicsResourceParameters, ResourceUriType resourceUriType)
        {
            switch(resourceUriType)
            {
                case ResourceUriType.PreviousPage: return urlHelper.Link("GetTopics",
                    new
                    {
                        searchTitle =  topicsResourceParameters.SearchTitle,
                        pageNumber = topicsResourceParameters.PageNumber - 1,
                        pageSize =  topicsResourceParameters.PageSize
                    });
                case ResourceUriType.NextPage: return urlHelper.Link("GetTopics",
                    new
                    {
                        searchTitle = topicsResourceParameters.SearchTitle,
                        pageNumber = topicsResourceParameters.PageNumber + 1,
                        pageSize =  topicsResourceParameters.PageSize
                    });
                default:
                    return urlHelper.Link("GetTopics",
                    new
                    {
                        searchTitle = topicsResourceParameters.SearchTitle,
                        pageNumber = topicsResourceParameters.PageNumber,
                        pageSize = topicsResourceParameters.PageSize
                    });
            }           
        }

        [HttpGet("{id}")]
        public IActionResult GetTopic(Guid id)
        {
            if (!noterRepository.TopicExists(id))
            {
                return NotFound();
            }
            var topicEntity = noterRepository.GetTopic(id);
            var topicDto = Mapper.Map<TopicDto>(topicEntity);

            return Ok(topicDto);
        }

        [HttpGet("page")]
        public IActionResult GetTopicThatIsPage()
        {
            //to be implemented by userId
            var topicEntity = noterRepository.GetTopicThatisPage();
            var topicDto = Mapper.Map<TopicDto>(topicEntity);

            return Ok(topicDto);
        }
        

        [HttpPost]
        public IActionResult CreateTopic([FromBody] TopicForCreationDto topicForCreation)
        {
            if (topicForCreation == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var topicEntity = Mapper.Map<Topic>(topicForCreation);
            noterRepository.CreateTopic(topicEntity);

            noterRepository.Save();

            return Ok(); // TO DO - to be implemented created at route
        }

        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdateTopic(Guid id,[FromBody] JsonPatchDocument<TopicForUpdateDto> patchDoc)
        {
            if (!noterRepository.TopicExists(id))
            {
                return NotFound();
            }

            if (patchDoc == null)
            {
                return BadRequest();
            }

            var topicEntity = noterRepository.GetTopic(id);
            var topicDto = Mapper.Map<TopicForUpdateDto>(topicEntity);
            patchDoc.ApplyTo(topicDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TryValidateModel(topicDto);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mapper.Map(topicDto, topicEntity);

            noterRepository.Save();
            
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTopic(Guid id)
        {
            if(!noterRepository.TopicExists(id))
            {
                return NotFound();
            }
            noterRepository.DeleteTopic(id);

            noterRepository.Save();
            
            return NoContent();
        }
    }
}
