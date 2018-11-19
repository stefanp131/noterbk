using System;
using System.ComponentModel.DataAnnotations;

namespace Noter.API.Models
{
    public class CommentaryForUpdate
    {
        [Required]
        [MaxLength(128)]
        public string Title { get; set; }
        [Required]
        [MaxLength(2048)]
        public string Content { get; set; }
        public int Approval { get; set; }
        public Guid TopicId { get; set; }
    }
}
