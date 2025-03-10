using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ForumApp.Data;
using ForumApp.Models;

namespace Assignment1.Controllers
{
    [Authorize] // Restrict access to authenticated users
    public class DiscussionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly string _imageFolderPath;

        public DiscussionsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _imageFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
        }

        // Show a list of all discussions
        public async Task<IActionResult> Index()
        {
            var discussions = await _context.Discussions
                .Include(d => d.User) // Ensure user details are included
                .ToListAsync();

            return View(discussions);
        }

        // Show details of a specific discussion
        public async Task<IActionResult> Details(int id)
        {
            var discussion = await _context.Discussions
                .Include(d => d.Comments)
                .FirstOrDefaultAsync(d => d.DiscussionId == id);

            return discussion == null ? NotFound() : View(discussion);
        }

        // Show the form to create a new discussion
        public IActionResult Create()
        {
            return View(new Discussion());
        }

        // Handle creating a new discussion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Discussion discussion, IFormFile? imageFile)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized(); // Ensure user is logged in
            }

            discussion.UserId = user.Id; // Assign logged-in user
            discussion.CreatedAt = DateTime.Now;

            ModelState.Clear();
            TryValidateModel(discussion);

            if (!ModelState.IsValid)
            {
                return View(discussion);
            }

            try
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    discussion.ImageUrl = await SaveImageAsync(imageFile);
                }

                _context.Add(discussion);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while creating the discussion. Try again.");
                Console.WriteLine("Error creating discussion: " + ex.Message);
                return View(discussion);
            }
        }

        // Show "My Threads" page for the logged-in user
        public async Task<IActionResult> MyThreads()
        {
            var userId = _userManager.GetUserId(User);
            var userThreads = await _context.Discussions
                .Where(d => d.UserId == userId)
                .ToListAsync();

            return View(userThreads);
        }

        // Show the form to edit an existing discussion
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var discussion = await _context.Discussions.FindAsync(id);
            if (discussion == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            if (discussion.UserId != userId) // 🔹 Restrict edit access
            {
                return Forbid(); // User is not the owner
            }

            return View(discussion);
        }

        // Handle updating a discussion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Discussion discussion, IFormFile? imageFile)
        {
            if (id != discussion.DiscussionId) return NotFound();

            var existingDiscussion = await _context.Discussions.AsNoTracking().FirstOrDefaultAsync(d => d.DiscussionId == id);
            if (existingDiscussion == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            if (existingDiscussion.UserId != userId) // 🔹 Restrict edit access
            {
                return Forbid(); // User is not the owner
            }

            discussion.UserId = existingDiscussion.UserId; // Preserve UserId

            if (imageFile != null && imageFile.Length > 0)
            {
                DeleteImage(existingDiscussion.ImageUrl);
                discussion.ImageUrl = await SaveImageAsync(imageFile);
            }
            else
            {
                discussion.ImageUrl = existingDiscussion.ImageUrl;
            }

            try
            {
                _context.Update(discussion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Discussions.Any(e => e.DiscussionId == discussion.DiscussionId))
                    return NotFound();
                else
                    throw;
            }
        }

        // Show the delete confirmation page
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var discussion = await _context.Discussions.FindAsync(id);
            if (discussion == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            if (discussion.UserId != userId) // 🔹 Restrict delete access
            {
                return Forbid(); // User is not the owner
            }

            return View(discussion);
        }

        // Handle deleting a discussion
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var discussion = await _context.Discussions.FindAsync(id);
            if (discussion != null)
            {
                var userId = _userManager.GetUserId(User);
                if (discussion.UserId != userId) // 🔹 Restrict delete access
                {
                    return Forbid(); // User is not the owner
                }

                DeleteImage(discussion.ImageUrl);
                _context.Discussions.Remove(discussion);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Helper function to save an image
        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(_imageFolderPath, fileName);

            Directory.CreateDirectory(_imageFolderPath);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return "/images/" + fileName;
        }

        // Helper function to delete an image
        private void DeleteImage(string? imageUrl)
        {
            if (!string.IsNullOrEmpty(imageUrl) && imageUrl != "/images/placeholder.png")
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imageUrl.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
        }
    }
}
