using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ForumApp.Data;
using ForumApp.Models;

namespace Assignment1.Controllers
{
    public class DiscussionsController : Controller
    {
        private readonly ApplicationDbContext _context; // Database connection
        private readonly string _imageFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images"); // Path for storing images

        public DiscussionsController(ApplicationDbContext context)
        {
            _context = context; // Initialize database context
        }

        // GET: Discussions (Show all discussions)
        public async Task<IActionResult> Index()
        {
            return View(await _context.Discussions.ToListAsync()); // Get all discussions from database
        }

        // GET: Discussions/Details/5 (View details of a discussion)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound(); // If no ID is provided, return 404

            var discussion = await _context.Discussions
                .FirstOrDefaultAsync(m => m.DiscussionId == id); // Find discussion by ID

            if (discussion == null) return NotFound(); // If discussion not found, return 404

            return View(discussion); // Show discussion details
        }

        // GET: Discussions/Create (Show form to create a new discussion)
        public IActionResult Create()
        {
            return View(new Discussion()); // Load an empty model for new discussion
        }

        // POST: Discussions/Create (Handle form submission for new discussion)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DiscussionId,Title,Content,CreatedAt,ImageUrl")] Discussion discussion, IFormFile imageFile)
        {
            if (ModelState.IsValid) // Check if input is valid
            {
                // Handle image upload
                if (imageFile != null && imageFile.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(_imageFolderPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream); // Save image file
                    }

                    discussion.ImageUrl = "/images/" + fileName; // Store image path in database
                }

                _context.Add(discussion); // Save discussion to database
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Redirect to discussion list
            }

            return View(discussion); // Return view if model is not valid
        }

        // GET: Discussions/Edit/5 (Show form to edit a discussion)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound(); // If no ID, return 404

            var discussion = await _context.Discussions.FindAsync(id);
            if (discussion == null) return NotFound(); // If discussion not found, return 404

            return View(discussion); // Show edit form
        }

        // POST: Discussions/Edit/5 (Handle form submission for editing)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DiscussionId,Title,Content,CreatedAt,ImageUrl")] Discussion discussion, IFormFile imageFile)
        {
            if (id != discussion.DiscussionId) return NotFound(); // Ensure correct discussion ID

            if (ModelState.IsValid)
            {
                try
                {
                    // Handle image update
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                        var filePath = Path.Combine(_imageFolderPath, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream); // Save new image
                        }

                        discussion.ImageUrl = "/images/" + fileName; // Update image path
                    }

                    _context.Update(discussion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Discussions.Any(e => e.DiscussionId == discussion.DiscussionId))
                    {
                        return NotFound(); // If discussion no longer exists, return 404
                    }
                    else
                    {
                        throw; // Handle other database errors
                    }
                }
                return RedirectToAction(nameof(Index)); // Redirect to discussions list
            }
            return View(discussion); // Return form if invalid
        }

        // GET: Discussions/Delete/5 (Show delete confirmation page)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound(); // If no ID, return 404

            var discussion = await _context.Discussions
                .FirstOrDefaultAsync(m => m.DiscussionId == id);
            if (discussion == null) return NotFound(); // If discussion not found, return 404

            return View(discussion); // Show delete confirmation page
        }

        // POST: Discussions/Delete/5 (Delete discussion)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var discussion = await _context.Discussions.FindAsync(id);
            if (discussion != null)
            {
                // Delete image only if no other discussions use it
                if (!string.IsNullOrEmpty(discussion.ImageUrl))
                {
                    bool isImageUsedByOthers = _context.Discussions.Any(d => d.ImageUrl == discussion.ImageUrl && d.DiscussionId != discussion.DiscussionId);
                    if (!isImageUsedByOthers)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", discussion.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath); // Delete image from server
                        }
                    }
                }

                _context.Discussions.Remove(discussion); // Remove discussion from database
                await _context.SaveChangesAsync(); // Save changes
            }

            return RedirectToAction(nameof(Index)); // Redirect to discussion list
        }
    }
}
