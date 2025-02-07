using ForumApp.Models;

namespace Assignment1.Models
{
    public class DiscussionViewModel
    {
        public int DiscussionId { get; set; }
        public string? Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CommentCount { get; set; }
        public string? ImageFilename { get; set; }

        public Discussion? Discussion { get; set; }
        public List<Comment>? Comments { get; set; }
    }

}
