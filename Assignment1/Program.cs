using ForumApp.Data;
using ForumApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services for controllers and views
builder.Services.AddControllersWithViews();
// Enable Razor Pages (needed for Identity UI)
builder.Services.AddRazorPages();

// Register the database connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity with custom ApplicationUser
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add a dummy email sender to prevent errors
builder.Services.AddSingleton<IEmailSender, NoOpEmailSender>();

var app = builder.Build();

// Configure error handling for different environments
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Use custom error page in production
    app.UseHsts(); // Enforce HTTPS in production
}
else
{
    app.UseDeveloperExceptionPage(); // Show detailed errors in development mode
}

app.UseHttpsRedirection(); // Redirect HTTP to HTTPS
app.UseStaticFiles(); // Serve static files (CSS, JS, images, etc.)
app.UseRouting(); // Enable routing

app.UseAuthentication(); // Enable user authentication
app.UseAuthorization();  // Enable user authorization

// Map Identity pages (Register, Login, Logout, etc.)
app.MapRazorPages();

// Map default controller routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Discussions}/{action=Index}/{id?}");

app.Run();

// Dummy Email Sender Implementation (prevents email-related errors)
public class NoOpEmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        return Task.CompletedTask; // Do nothing (avoids errors when email sending is not set up)
    }
}
