using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using HumanErrorProject.Engine;
using HumanErrorProject.Engine.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SendGrid.Helpers.Mail;

namespace HumanErrorProject.Ui.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        protected UserManager<IdentityUser> UserManager;
        protected IEmailService EmailService;

        public ForgotPasswordModel(UserManager<IdentityUser> userManager, IEmailService emailService)
        {
            UserManager = userManager;
            EmailService = emailService;
        }

        [BindProperty]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(Email);
                if (user == null)
                {
                    return RedirectToPage("/Account/ForgotPasswordConfirmation");
                }

                var code = await UserManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { code },
                    protocol: Request.Scheme);

                await EmailService.Send(new EmailData()
                {
                    Email = Email,
                    Name = Email,
                    Subject = "Reset Password",
                    Content = $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>."
                });
                return RedirectToPage("/Account/ForgotPasswordConfirmation");
            }
            return Page();
        }
    }
}