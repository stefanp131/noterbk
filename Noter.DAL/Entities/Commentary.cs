using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Noter.DAL.Entities
{
    public class Commentary
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(128)]
        public string Title { get; set; }
        [Required]
        [MaxLength(2048)]
        public string Content { get; set; }
        public int Approval { get; set; }
        public DateTime Created { get; set; }
        [ForeignKey("TopicId")]
        public Topic Topic { get; set; }
        public Guid TopicId { get; set; }
    }
}
