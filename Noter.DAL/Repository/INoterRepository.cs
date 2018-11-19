using Noter.DAL.Entities;
using Noter.DAL.Helpers;
using System;
using System.Collections.Generic;

namespace Noter.DAL.Repository
{
    public interface INoterRepository
    {
        PagedList<Topic> GetTopics(TopicsResourceParameters topicsResourceParameters);
        Topic GetTopic(Guid id);
        bool TopicExists(Guid id);
        void CreateTopic(Topic topic);
        void DeleteTopic(Guid id);
        PagedList<Commentary> GetCommentariesForTopic(Guid topicId, CommentariesResourceParameters commentariesResourceParameters);
        Commentary GetCommentaryForTopic(Guid topicId, Guid id);
        bool CommentaryExistsForTopic(Guid topicId, Guid id);
        void CreateCommentaryForTopic(Commentary commentary);
        void DeleteCommentaryForTopic(Guid topicId, Guid id);
        bool Save();
        Topic GetTopicThatisPage();
    }
}
