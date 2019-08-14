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
    public class DeleteModel : PageModel
    {
        public HumanErrorProjectContext Context;
        public DbSet<SurveyQuestion> SurveyQuestions;

        public DeleteModel(HumanErrorProjectContext context)
        {
            Context = context;
            SurveyQuestions = context.Set<SurveyQuestion>();
        }

        [FromRoute]
        public int Id { get; set; }

        public SurveyQuestion Question { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Question = await SurveyQuestions.FindAsync(Id);
            if (Question == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Question = await SurveyQuestions.FindAsync(Id);
            if (Question == null) return NotFound();

            var id = Question.CourseClassId;

            // TODO: Remove Survey Answers Related
            SurveyQuestions.Remove(Question);
            await Context.SaveChangesAsync();
            return RedirectToPage("/Questions/Index", new {id});
        }
    }
}