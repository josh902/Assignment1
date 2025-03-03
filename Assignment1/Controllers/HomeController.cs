using ForumApp.Data;
using ForumApp.Models; // Fixed namespace to match where DiscussionViewModel is defined
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Assignment1.Models;

namespace ForumApp.Controllers // Ensure the namespace is correctly set
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Home/Index
        public async Task<IActionResult> Index()
        {
            var discussions = await _context.Discussions
                .OrderByDescending(d => d.CreatedAt) // Show newest first
                .Select(d => new DiscussionViewModel // Convert to correct model
                {
                    DiscussionId = d.DiscussionId, // Ensure consistency with model
                    Title = d.Title,
                    Content = d.Content,
                    CreatedAt = d.CreatedAt,
                    ImageUrl = d.ImageUrl // If you have images
                })
                .ToListAsync();

            return View(discussions); // Now the correct type is passed
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
