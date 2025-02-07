using Assignment1.Models;
using ForumApp.Data;
using ForumApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var discussions = await _context.Discussions
            .OrderByDescending(d => d.CreatedAt)
            .Select(d => new DiscussionViewModel
            {
                DiscussionId = d.DiscussionId,
                Title = d.Title,
                CreatedAt = d.CreatedAt,
                CommentCount = d.Comments.Count,
                ImageFilename = d.ImageFilename
            })
            .ToListAsync();

        return View(discussions);
    }

    public async Task<IActionResult> GetDiscussion(int id)
    {
        var discussion = await _context.Discussions
            .Include(d => d.Comments)
            .FirstOrDefaultAsync(d => d.DiscussionId == id);

        if (discussion == null)
        {
            return NotFound();
        }

        return View(discussion);
    }
}
