using Microsoft.AspNetCore.Identity;
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

    }
}
