using ForumApp.Models;

namespace Assignment1.Models // Ensure namespace matches project structure
{
    public class DiscussionViewModel
    {
        public int DiscussionId { get; set; } // Matches Discussion model
        public string? Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CommentCount { get; set; }
        public string? ImageFilename { get; set; }
        public string? Content { get; set; }
        public string? ImageUrl { get; set; }
        


        public Discussion? Discussion { get; set; } // Navigation property
        public List<Comment>? Comments { get; set; }
    }
}
