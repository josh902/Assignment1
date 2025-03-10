using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ForumApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; } = string.Empty; // Required Name field

        public string? Location { get; set; } // Optional Location field

        public string? ImageFilename { get; set; } // Stores uploaded profile image filename

        [NotMapped] // Prevents storing ImageFile in the database
        public IFormFile? ImageFile { get; set; }

        // Fixed Profile Picture URL property to ensure correct image paths
        [NotMapped]
        public string ProfilePictureUrl
        {
            get
            {
                return string.IsNullOrEmpty(ImageFilename)
                    ? "/images/placeholder.png" // Default placeholder if no image
                    : $"/images/{ImageFilename}";
            }
        }

        // Added Discussions List to Fix "ApplicationUser Does Not Contain a Definition for 'Discussions'" Error
        public List<Discussion> Discussions { get; set; } = new List<Discussion>();
    }
}
