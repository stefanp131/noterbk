using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Noter.DAL.Entities
{
    public class Topic
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(128)]
        public string Title { get; set; }
        [Required]
        [MaxLength(512)]
        public string Description { get; set; }
        public Boolean IsPage { get; set; }
        public int Approval { get; set; }
        public ICollection<Commentary> Commentaries { get; set; }
    }
}
