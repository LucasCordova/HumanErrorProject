using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HumanErrorProject.Engine;
using HumanErrorProject.Ui.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HumanErrorProject.Ui.Pages.Account
{
    [Authorize(Roles = IdentityRoleConstants.Both)]
    public class ChangePasswordModel : PageModel
    {
        protected UserManager<IdentityUser> UserManager;
        protected SignInManager<IdentityUser> SignInManager;
        protected IEmailService EmailService;

        public ChangePasswordModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            IEmailService emailService)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            EmailService = emailService;
        }

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

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

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }
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

            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await UserManager.ChangePasswordAsync(user, OldPassword, NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }
            await SignInManager.RefreshSignInAsync(user);

            return RedirectToPage("/Account/Profile");
        }

    }
}