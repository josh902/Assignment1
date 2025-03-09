using Microsoft.AspNetCore.Mvc;
using ForumApp.Data;
using ForumApp.Models;
using System.Threading.Tasks;


namespace Assignment1.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context; // Database connection

        public CommentsController(ApplicationDbContext context)
        {
            _context = context; // Initialize the database context
        }

        // GET: Comments/Create
        public IActionResult Create(int discussionId)
        {
            ViewBag.DiscussionId = discussionId;  // Store discussion ID for the form
            return View(); // Show the create comment form
        }

        // POST: Comments/Create
        [HttpPost]
        [ValidateAntiForgeryToken] // Security to prevent CSRF attacks
        public async Task<IActionResult> Create([Bind("Content, DiscussionId")] Comment comment)
        {
            if (ModelState.IsValid) // Check if input is valid
            {
                // Set the CreatedAt time for the new comment
                comment.CreatedAt = DateTime.Now;

                _context.Add(comment);  // Add the new comment to the database
                await _context.SaveChangesAsync(); // Save changes to the database

                // Redirect back to the correct discussion details page
                return RedirectToAction("Details", "Discussions", new { id = comment.DiscussionId });
            }

            // If input is invalid, return to the form with the current data
            return View(comment);
        }
    }
}
