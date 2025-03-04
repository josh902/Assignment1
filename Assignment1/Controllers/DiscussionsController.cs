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
        private readonly ApplicationDbContext _context;
        private readonly string _imageFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

        public DiscussionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Discussions
        public async Task<IActionResult> Index()
        {
            return View(await _context.Discussions.ToListAsync());
        }

        // GET: Discussions/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            var discussion = await _context.Discussions
                .Include(d => d.Comments) // Include related comments
                .FirstOrDefaultAsync(d => d.DiscussionId == id);

            if (discussion == null)
            {
                return NotFound();
            }

            return View(discussion);
        }

        
        // GET: Discussions/Create
        public IActionResult Create()
        {
            return View(new Discussion());
        }

        // POST: Discussions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DiscussionId,Title,Content,CreatedAt,ImageUrl")] Discussion discussion, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(_imageFolderPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    discussion.ImageUrl = "/images/" + fileName;
                }

                _context.Add(discussion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(discussion);
        }

        // GET: Discussions/Edit/{id}
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var discussion = await _context.Discussions.FirstOrDefaultAsync(d => d.DiscussionId == id);
            if (discussion == null) return NotFound();

            return View(discussion);
        }

        // POST: Discussions/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DiscussionId,Title,Content,CreatedAt,ImageUrl")] Discussion discussion, IFormFile? imageFile)
        {
            if (id != discussion.DiscussionId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                        var filePath = Path.Combine(_imageFolderPath, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        discussion.ImageUrl = "/images/" + fileName;
                    }

                    _context.Update(discussion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Discussions.Any(e => e.DiscussionId == discussion.DiscussionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(discussion);
        }

        // GET: Discussions/Delete/{id}
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var discussion = await _context.Discussions
                .FirstOrDefaultAsync(m => m.DiscussionId == id);
            if (discussion == null) return NotFound();

            return View(discussion);
        }

        // POST: Discussions/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var discussion = await _context.Discussions.FindAsync(id);
            if (discussion != null)
            {
                if (!string.IsNullOrEmpty(discussion.ImageUrl))
                {
                    bool isImageUsedByOthers = _context.Discussions.Any(d => d.ImageUrl == discussion.ImageUrl && d.DiscussionId != discussion.DiscussionId);
                    if (!isImageUsedByOthers)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", discussion.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                }

                _context.Discussions.Remove(discussion);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
