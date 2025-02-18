
using System;

namespace ForumApp.Models
{
    public class Comment
    {
        public int CommentId { get; set; }  // Primary key for Comment
        public string? Author { get; set; }  // The author of the comment
        public string? Content { get; set; }  // Content of the comment
        public DateTime CreatedAt { get; set; } = DateTime.Now;  // Automatically sets to current date and time
        public int DiscussionId { get; set; }  // Foreign Key to Discussion
        public Discussion? Discussion { get; set; }  // Navigation Property (the Discussion this comment belongs to)
    }
}
