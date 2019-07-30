using System;
using System.Collections.Generic;
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
    public class ProfileModel : PageModel
    {
        protected UserManager<IdentityUser> UserManager;
        protected SignInManager<IdentityUser> SignInManager;
        protected IEmailService EmailService;

        public ProfileModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            IEmailService emailService)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            EmailService = emailService;
        }

        public string Username { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            Username = await UserManager.GetUserNameAsync(user);

            return Page();
        }

    }
}