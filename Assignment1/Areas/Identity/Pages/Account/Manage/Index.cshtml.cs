using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ForumApp.Models;
using Microsoft.AspNetCore.Http;

namespace Assignment1.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Full Name")]
            public string Name { get; set; }

            [Display(Name = "Location")]
            public string Location { get; set; }

            [Display(Name = "Profile Picture")]
            public IFormFile ImageFile { get; set; }

            public string ImageFilename { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("Unable to load user.");
            }

            Input = new InputModel
            {
                Name = user.Name,
                Location = user.Location,
                ImageFilename = user.ImageFilename ?? "default-profile.png"
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("Unable to load user.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            user.Name = Input.Name;
            user.Location = Input.Location;

            // Handle Profile Picture Upload
            if (Input.ImageFile != null)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(Input.ImageFile.FileName)}";
                var filePath = Path.Combine("wwwroot/images", fileName);

                Directory.CreateDirectory("wwwroot/images");
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Input.ImageFile.CopyToAsync(stream);
                }

                user.ImageFilename = fileName;
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Failed to update profile.");
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            return RedirectToPage();
        }
    }
}