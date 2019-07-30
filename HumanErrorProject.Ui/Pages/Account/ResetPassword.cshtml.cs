using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HumanErrorProject.Ui.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
        protected UserManager<IdentityUser> UserManager;

        public ResetPasswordModel(UserManager<IdentityUser> userManager)
        {
            UserManager = userManager;
        }

        [BindProperty]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [BindProperty]
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [BindProperty]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        public string ConfirmPassword { get; set; }

        [BindProperty]
        public string Code { get; set; }

        public IActionResult OnGet(string code = null)
        {
            if (code == null)
                return BadRequest("A code must be supplied for password reset.");
            Code = code;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!NewPassword.Equals(ConfirmPassword))
            {
                ModelState.AddModelError(string.Empty, "Confirm Password doesn't match Password.");
                return Page();
            }

            var user = await UserManager.FindByEmailAsync(Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToPage("/Account/ResetPasswordConfirmation");
            }

            var result = await UserManager.ResetPasswordAsync(user, Code, NewPassword);
            if (result.Succeeded)
            {
                return RedirectToPage("/Account/ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();
        }
    }
}