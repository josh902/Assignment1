using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace ForumApp.Models
{
    public class Discussion
    {
        [Key] // Marks this as the primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-generates ID
        public int DiscussionId { get; set; } // Unique ID for discussions

        public string? Title { get; set; } // Discussion title

        public string? Content { get; set; } // Discussion content

        public DateTime CreatedAt { get; set; } = DateTime.Now; // Sets current date when created

        public List<Comment> Comments { get; set; } = new List<Comment>(); // List of comments

        public string? ImageUrl { get; set; } // Stores image file path or URL

        public string? ImageFilename { get; set; } // Stores only the filename of the image

        //  Fix: Explicitly define the foreign key relationship
        [Required]
        public string? UserId { get; set; } // Stores the user's ID from IdentityUser

        [ForeignKey("UserId")]
        public IdentityUser? User { get; set; } // Navigation property

        [NotMapped] // This will not be saved in the database
        public IFormFile? Image { get; set; } // Used to handle file uploads
    }
}
