using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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

namespace HumanErrorProject.Ui.Pages.Surveys
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        public HumanErrorProjectContext Context;
        public DbSet<Survey> Surveys;
        public SignInManager<IdentityUser> SignInManager;
        public UserManager<IdentityUser> UserManager;

        public IndexModel(HumanErrorProjectContext context, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            Context = context;
            SignInManager = signInManager;
            UserManager = userManager;
            Surveys = context.Set<Survey>();
        }

        [FromRoute]
        public string Id { get; set; }

        public string Email { get; set; }

        [BindProperty, Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [BindProperty, Required, DataType(DataType.Password) ]
        public string ConfirmPassword { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToPage("/Surveys/Details", new {Id});

            var survey = await Surveys.FindAsync(Id);

            if (survey == null) return NotFound();

            Context.Entry(survey).Reference(x => x.Student)
                .Load();

            if (survey.Student.IdentityUserId != null)
                return RedirectToPage("/Account/Login",
                    new {returnUrl = $"/Surveys/Details/{Id}"});

            Email = survey.Student.Email;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var survey = await Surveys.FindAsync(Id);

            if (survey == null) return NotFound();

            Context.Entry(survey).Reference(x => x.Student)
                .Load();

            Email = survey.Student.Email;

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

            survey.Student.IdentityUserId = user.Id;
            Context.Students.Update(survey.Student);
            await Context.SaveChangesAsync();

            return RedirectToPage("/Account/Login",
                new { returnUrl = $"/Surveys/Details/{Id}" });
        }


    }
}