using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanErrorProject.Data.DataAccess;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Ui.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HumanErrorProject.Ui.Pages.Surveys
{
    [Authorize(Roles = IdentityRoleConstants.Student)]
    public class ReviewModel : PageModel
    {
        public HumanErrorProjectContext Context;
        public DbSet<Survey> Surveys;

        public ReviewModel(HumanErrorProjectContext context)
        {
            Context = context;
            Surveys = context.Set<Survey>();
        }


        [FromRoute]
        public string Id { get; set; }

        public Survey Survey { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Survey = await Surveys.FindAsync(Id);
            if (Survey == null || !Survey.IsCompleted)
                return NotFound();

            if (!Survey.Student.Email.Equals(User.Identity.Name))
                return RedirectToPage("/Account/AccessDenied");

            return Page();
        }
    }
}