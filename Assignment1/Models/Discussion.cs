using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ForumApp.Models
{
    public class Discussion
    {
        public int DiscussionId { get; set; }

        public string? Title { get; set; }

        public string? Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;  // Automatically sets to current date and time

        public List<Comment> Comments { get; set; } = new List<Comment>();  // Navigation Property for Comments

        // Store the file path or URL of the image
        public string? ImageUrl { get; set; }  // This is optional but can be used for the full image URL

        // This property is used to store the filename of the uploaded image
        public string? ImageFilename { get; set; }  // This will be used for storing just the filename

        // This property is only for handling file uploads, do not map it to the database
        [NotMapped]
        public IFormFile? Image { get; set; }

    }
}
