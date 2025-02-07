using Microsoft.AspNetCore.Mvc;
using ForumApp.Data;
using ForumApp.Models;
using System.Threading.Tasks;

namespace Assignment1.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Comments/Create
        public IActionResult Create(int discussionId)
        {
            ViewBag.DiscussionId = discussionId;  // Pass the DiscussionId to the view for the form
            return View();
        }

        // POST: Comments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Content, DiscussionId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                // Set the CreatedAt to the current time
                comment.CreatedAt = DateTime.Now;

                _context.Add(comment);  // Add the new comment to the database
                await _context.SaveChangesAsync();

                // Redirect to the Discussion details page (you will create this page later)
                return RedirectToAction("Details", "Discussions", new { id = comment.DiscussionId });
            }

            // If the model is not valid, return to the view with the error
            return View(comment);
        }
    }
}
