using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HumanErrorProject.Ui.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        protected SignInManager<IdentityUser> SignInManager;

        public LogoutModel(SignInManager<IdentityUser> signInManager)
        {
            SignInManager = signInManager;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            await SignInManager.SignOutAsync();
            return RedirectToPage();
        }
    }
}