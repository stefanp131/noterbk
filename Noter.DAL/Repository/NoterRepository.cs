using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Noter.DAL.Context;
using Noter.DAL.Entities;
using Noter.DAL.Helpers;

namespace Noter.DAL.Repository
{
    public class NoterRepository : INoterRepository
    {
        private readonly NoterContext context;

        public NoterRepository(NoterContext context)
        {
            this.context = context;
        }

        public bool CommentaryExistsForTopic(Guid topicId, Guid id)
        {
            return context.Commentaries.FirstOrDefault(e => e.TopicId == topicId && e.Id == id) != null;
        }

        public void CreateCommentaryForTopic(Commentary commentary)
        {
            commentary.Created = DateTime.Now;
            context.Commentaries.Add(commentary);
        }

        public void CreateTopic(Topic topic)
        {
            context.Topics.Add(topic);
        }

        public void DeleteCommentaryForTopic(Guid topicId, Guid id)
        {
            var commentaryEntity = context.Commentaries.FirstOrDefault(e => e.TopicId == topicId && e.Id == id);
            context.Commentaries.Remove(commentaryEntity);
        }

        public void DeleteTopic(Guid id)
        {
            var topicEntity = context.Topics.FirstOrDefault(e => e.Id == id);
            context.Topics.Remove(topicEntity);
        }

        public PagedList<Commentary> GetCommentariesForTopic(Guid topicId, CommentariesResourceParameters commentariesResourceParameters)
        {
            var query = context.Commentaries.Where(e => e.TopicId == topicId);
            return PagedList<Commentary>.Create(query, commentariesResourceParameters.PageNumber, commentariesResourceParameters.PageSize);
        }

        public Commentary GetCommentaryForTopic(Guid topicId, Guid id)
        {
            return context.Commentaries.FirstOrDefault(e => e.TopicId == topicId && e.Id == id);
        }

        public Topic GetTopic(Guid id)
        {
            return context.Topics.Include(e => e.Commentaries).FirstOrDefault(e => e.Id == id);
        }

        public PagedList<Topic> GetTopics(TopicsResourceParameters topicsResourceParameters)
        {
            IQueryable<Topic> query;
            if (!string.IsNullOrEmpty(topicsResourceParameters.SearchTitle))
                query = context.Topics                    
                    .Where(e => e.IsPage == false && e.Title.Contains(topicsResourceParameters.SearchTitle))
                    .Include(e => e.Commentaries);
            else
                query = context.Topics
                    .Where(e => e.IsPage == false)
                    .Include(e => e.Commentaries);

            return PagedList<Topic>.Create(query, topicsResourceParameters.PageNumber, topicsResourceParameters.PageSize);
                
        }

        public Topic GetTopicThatisPage()
        {
            return context.Topics.FirstOrDefault(e => e.IsPage == true);
        }

        public bool Save()
        {
            if (context.SaveChanges() > 0)
                return true;
            return false;
        }

        public bool TopicExists(Guid id)
        {
            return context.Topics.FirstOrDefault(e => e.Id == id) != null;
        }
    }
}
