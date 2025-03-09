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

        // 🔹 Ensure correct Foreign Key Type
        [ForeignKey("ApplicationUser")]
        public string? UserId { get; set; }  // Fix: Renamed from ApplicationUserId for consistency

        public ApplicationUser? User { get; set; }

        [NotMapped]
        public IFormFile? Image { get; set; }
    }
}