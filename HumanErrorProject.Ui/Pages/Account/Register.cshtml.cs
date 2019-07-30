using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using HumanErrorProject.Data.DataAccess;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Ui.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HumanErrorProject.Ui.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        public HumanErrorProjectContext Context;
        public DbSet<Student> Students;
        public SignInManager<IdentityUser> SignInManager;
        public UserManager<IdentityUser> UserManager;

        public RegisterModel(HumanErrorProjectContext context, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            Context = context;
            SignInManager = signInManager;
            UserManager = userManager;
            Students = Context.Set<Student>();
        }

        [FromRoute]
        public string Id { get; set; }

        public string Email { get; set; }

        [BindProperty, Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [BindProperty, Required, DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var student = await Students.FindAsync(Id);

            if (student == null) return NotFound();

            if (await UserManager.FindByEmailAsync(student.Email) != null)
            {
                return RedirectToPage("/Account/Login");
            }

            Email = student.Email;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var student = await Students.FindAsync(Id);

            if (student == null) return NotFound();

            if (await UserManager.FindByEmailAsync(student.Email) != null)
            {
                return RedirectToPage("/Account/Login");
            }

            Email = student.Email;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!Password.Equals(ConfirmPassword))
            {
                ModelState.AddModelError(string.Empty, "Confirm Password doesn't match Password.");
                return Page();
            }

            var user = new IdentityUser()
            {
                UserName = Email,
                Email = Email,
            };

            var result = await UserManager.CreateAsync(user, Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            await UserManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, IdentityRoleConstants.Student));
            await UserManager.AddToRoleAsync(user, IdentityRoleConstants.Student);

            return RedirectToPage("/Account/Login");
        }

    }
}