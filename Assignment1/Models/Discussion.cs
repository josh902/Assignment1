using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ForumApp.Models
{
    public class Discussion
    {
        [Key] // Marks this as the primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-generates ID
        public int DiscussionId { get; set; } // Use this as the unique ID for discussions

        public string? Title { get; set; } // Discussion title

        public string? Content { get; set; } // Discussion content

        public DateTime CreatedAt { get; set; } = DateTime.Now; // Sets current date when created

        public List<Comment> Comments { get; set; } = new List<Comment>(); // List of comments linked to discussion

        public string? ImageUrl { get; set; } // Stores image file path or URL

        public string? ImageFilename { get; set; } // Stores only the filename of the image

        [NotMapped] // This will not be saved in the database
        public IFormFile? Image { get; set; } // Used to handle file uploads
    }
}