using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ForumApp.Models
{
    public class Discussion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DiscussionId { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public List<Comment> Comments { get; set; } = new List<Comment>();

        public string? ImageUrl { get; set; }

        public string? ImageFilename { get; set; }

        [ForeignKey("ApplicationUser")]
        public string? UserId { get; set; }

        // Ensure navigation property exists for retrieving user details
        public ApplicationUser? User { get; set; }

        [NotMapped]
        public IFormFile? Image { get; set; }
    }
}
