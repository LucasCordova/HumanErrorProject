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

        public CreateModel(HumanErrorProjectContext context)
        {
            Context = context;
            SurveyQuestions = context.Set<SurveyQuestion>();
        }

        [FromRoute]
        public int Type { get; set; }

        [BindProperty]
        public SurveyQuestion Question { get; set; }

        public IActionResult OnGetAsync()
        {
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
            if (!ModelState.IsValid)
                return Page();

            SurveyQuestions.Add(Question);
            await Context.SaveChangesAsync();
            return RedirectToPage("/Questions/Index");
        }
    }
}