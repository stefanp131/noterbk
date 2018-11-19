using System;

namespace Noter.API.Models
{
    public class CommentaryDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Approval { get; set; }
        public Guid TopicId { get; set; }
        public string Created { get; set; }
    }
}
