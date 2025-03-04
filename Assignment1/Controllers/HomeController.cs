using ForumApp.Data;
using ForumApp.Models; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Allows working with the database using Entity Framework
using System.Linq; 
using System.Threading.Tasks; 
using System.Collections.Generic;
using Assignment1.Models; // Brings in additional models like DiscussionViewModel

namespace ForumApp.Controllers // Defines this as part of the ForumApp namespace
{
    public class HomeController : Controller // This class handles page requests for the Home section
    {
        private readonly ApplicationDbContext _context; // Connects to the database

        public HomeController(ApplicationDbContext context) // Constructor to initialize the database connection
        {
            _context = context;
        }

        // GET: Home/Index (Shows the list of discussions on the homepage)
        public async Task<IActionResult> Index()
        {
            var discussions = await _context.Discussions
                .Include(d => d.Comments) // Include comments to count them
                .OrderByDescending(d => d.CreatedAt) // Orders discussions by newest first
                .Select(d => new DiscussionViewModel // Converts database data into a view model
                {
                    DiscussionId = d.DiscussionId, // Assigns ID from database
                    Title = d.Title, // Assigns the discussion title
                    Content = d.Content, // Assigns the discussion content
                    CreatedAt = d.CreatedAt, // Assigns the discussion creation time
                    ImageUrl = d.ImageUrl, // Assigns the discussion image URL if available
                    CommentCount = d.Comments.Count //  This line counts comments
                })
                .ToListAsync(); // Converts the result to a list

            return View(discussions); // Passes discussions to the Index view
        }

        // Redirect users to Discussions/Details/{id} instead of looking for a Home/Details view
        public async Task<IActionResult> GetDiscussion(int id)
        {
            var discussion = await _context.Discussions
                .Include(d => d.Comments) // Fetch related comments
                .FirstOrDefaultAsync(d => d.DiscussionId == id);

            if (discussion == null)
            {
                return NotFound(); // Return 404 if no discussion exists
            }

            return RedirectToAction("Details", "Discussions", new { id = discussion.DiscussionId });
        }




        // GET: Home/Privacy (Loads the Privacy Policy page)
        public IActionResult Privacy()
        {
            return View(); // Returns the Privacy view
        }

        // GET: Home/Error (Handles errors)
        public IActionResult Error()
        {
            return View(); // Returns the Error view
        }
    }
}
