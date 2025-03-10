using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // 🔹 Added for authentication
using Microsoft.AspNetCore.Identity;
using ForumApp.Data;
using ForumApp.Models;
using System.Threading.Tasks;

namespace Assignment1.Controllers
{
    [Authorize] // 🔹 Restrict access to authenticated users
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager; // 🔹 Added to manage users

        public CommentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager; // 🔹 Initialize UserManager
        }

        // GET: Comments/Create
        public IActionResult Create(int discussionId)
        {
            ViewBag.DiscussionId = discussionId;
            return View();
        }

        // POST: Comments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Content, DiscussionId")] Comment comment)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized(); // 🔹 Ensure user is logged in
            }

            if (ModelState.IsValid)
            {
                comment.CreatedAt = DateTime.Now;
                comment.UserId = user.Id; // 🔹 Store logged-in user's ID

                _context.Add(comment);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "Discussions", new { id = comment.DiscussionId });
            }

            return View(comment);
        }

        // Handle deleting a comment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            if (comment.UserId != userId) // 🔹 Restrict delete access
            {
                return Forbid(); // User is not the owner
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Discussions", new { id = comment.DiscussionId });
        }
    }
}
