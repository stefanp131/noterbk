using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Noter.API.Models
{
    public class TopicForUpdateDto
    {
        [Required]
        [MaxLength(128)]
        public string Title { get; set; }
        [Required]
        [MaxLength(512)]
        public string Description { get; set; }
        public int Approval { get; set; }
    }
}
