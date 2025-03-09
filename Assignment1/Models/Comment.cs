using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ForumApp.Models;

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

        // 🔹 Make UserId nullable (Remove [Required])
        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        // Optional author field
        public string? Author { get; set; }
    }
}
