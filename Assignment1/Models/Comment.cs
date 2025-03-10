using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForumApp.Models
{
    public class Comment
    {
        public int CommentId { get; set; }

        public string? Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int DiscussionId { get; set; }

        [ForeignKey("DiscussionId")]
        public Discussion? Discussion { get; set; }

        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; } // Ensure user reference exists

        // Optional author field (Can be removed if not used)
        public string? Author { get; set; }
    }
}
