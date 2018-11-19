using System;

namespace Noter.API.Models
{
    public class TopicDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CommentariesCount { get; set; }
        public int Approval { get; set; }
    }
}
