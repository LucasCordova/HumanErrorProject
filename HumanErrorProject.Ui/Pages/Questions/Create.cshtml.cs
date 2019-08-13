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
    public class CreateModel : PageModel
    {
        public HumanErrorProjectContext Context;
        public DbSet<SurveyQuestion> SurveyQuestions;
        public DbSet<CourseClass> CourseClasses;

        public CreateModel(HumanErrorProjectContext context)
        {
            Context = context;
            CourseClasses = context.Set<CourseClass>();
            SurveyQuestions = context.Set<SurveyQuestion>();
        }

        [FromRoute]
        public int Id { get; set; }

        [FromRoute]
        public int Type { get; set; }

        [BindProperty]
        public SurveyQuestion Question { get; set; }

        public CourseClass CourseClass { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            CourseClass = await CourseClasses.FindAsync(Id);

            if (CourseClass == null) return NotFound();

            var type = (SurveyQuestion.SurveyQuestionTypes)Type;
            switch (type)
            {
                case SurveyQuestion.SurveyQuestionTypes.Qualitative:
                    Question = new SurveyQuestionQualitative();
                    break;
                case SurveyQuestion.SurveyQuestionTypes.Rate:
                    Question = new SurveyQuestionRate();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            CourseClass = await CourseClasses.FindAsync(Id);

            if (CourseClass == null) return NotFound();

            if (!ModelState.IsValid)
                return Page();

            Context.Entry(CourseClass)
                .Collection(x => x.SurveyQuestions)
                .Load();

            CourseClass.SurveyQuestions.Add(Question);
            CourseClasses.Update(CourseClass);
            await Context.SaveChangesAsync();

            return RedirectToPage("/Questions/Index", new { Id });
        }
    }
}