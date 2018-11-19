using System.ComponentModel.DataAnnotations;

namespace Noter.API.Models
{
    public class TopicForCreationDto
    {
        [Required]
        [MaxLength(128)]
        public string Title { get; set; }
        [Required]
        [MaxLength(512)]
        public string Description { get; set; }
    }
}
