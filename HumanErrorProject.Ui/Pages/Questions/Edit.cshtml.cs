using System;
using HumanErrorProject.Data.DataAccess;
using HumanErrorProject.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using HumanErrorProject.Ui.Constants;

namespace HumanErrorProject.Ui.Pages.Questions
{
    [Authorize(Roles = IdentityRoleConstants.Admin)]
    public class EditModel : PageModel
    {
        public HumanErrorProjectContext Context;
        public DbSet<SurveyQuestion> SurveyQuestions;

        public EditModel(HumanErrorProjectContext context)
        {
            Context = context;
            SurveyQuestions = context.Set<SurveyQuestion>();
        }

        [FromRoute]
        public int Id { get; set; }

        [BindProperty]
        public SurveyQuestion Question { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Question = await SurveyQuestions.FindAsync(Id);
            if (Question == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            SurveyQuestions.Update(Question);
            await Context.SaveChangesAsync();
            return RedirectToPage("/Questions/Index");
        }
    }
}