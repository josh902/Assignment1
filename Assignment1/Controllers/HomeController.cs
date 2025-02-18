using ForumApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context; // Database connection

    public HomeController(ApplicationDbContext context)
    {
        _context = context; // Initialize database
    }

    // GET: Home/Index (Show homepage with discussions)
    public async Task<IActionResult> Index()
    {
        var discussions = await _context.Discussions
            .OrderByDescending(d => d.CreatedAt) // Show newest first
            .ToListAsync();

        return View(discussions); // Send data to the homepage view
    }

    // GET: Home/Privacy (Show privacy page)
    public IActionResult Privacy()
    {
        return View();
    }

    // Handle errors
    public IActionResult Error()
    {
        return View();
    }
}
