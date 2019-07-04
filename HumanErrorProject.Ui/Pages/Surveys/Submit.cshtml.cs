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
    public class SubmitModel : PageModel
    {
        public HumanErrorProjectContext Context;
        public DbSet<SurveyQuestion> SurveyQuestionsSet;
        public DbSet<Survey> Surveys;

        public SubmitModel(HumanErrorProjectContext context)
        {
            Context = context;
            SurveyQuestionsSet = context.Set<SurveyQuestion>();
            Surveys = context.Set<Survey>();
        }

        [FromRoute]
        public string Id { get; set; }


        [BindProperty]
        public IList<SurveyResponse> SurveyResponses { get; set; } = new List<SurveyResponse>();

        public async Task<IActionResult> OnGetAsync()
        {
            var survey = await Surveys.FindAsync(Id);
            if (survey == null || survey.IsCompleted)
                return NotFound();

            Context.Entry(survey).Reference(x => x.Student).Load();

            if (!survey.Student.Email.Equals(User.Identity.Name))
                return RedirectToPage("/Account/AccessDenied");

            var questions = await SurveyQuestionsSet.ToListAsync();

            foreach (var question in questions)
            {
                switch (question.Type)
                {
                    case SurveyQuestion.SurveyQuestionTypes.Qualitative:
                        SurveyResponses.Add(new SurveyResponse()
                        {
                            SurveyQuestionId = question.Id,
                            Question = question,
                            Answer = new SurveyAnswerQualitative(),
                        });
                        break;
                    case SurveyQuestion.SurveyQuestionTypes.Rate:
                        SurveyResponses.Add(new SurveyResponse()
                        {
                            SurveyQuestionId = question.Id,
                            Question = question,
                            Answer = new SurveyAnswerRate(),
                        });
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            for (var i = 0; i < SurveyResponses.Count; i++)
            {
                SurveyResponses[i].Question = await SurveyQuestionsSet.FindAsync(SurveyResponses[i].Question.Id);
                if (SurveyResponses[i].Question == null) return NotFound();
                switch (SurveyResponses[i].Question.Type)
                {
                    case SurveyQuestion.SurveyQuestionTypes.Qualitative:
                        ValidateQuestion((SurveyAnswerQualitative) SurveyResponses[i].Answer,
                            (SurveyQuestionQualitative) SurveyResponses[i].Question, i);
                        break;
                    case SurveyQuestion.SurveyQuestionTypes.Rate:
                        ValidateQuestion((SurveyAnswerRate)SurveyResponses[i].Answer,
                            (SurveyQuestionRate)SurveyResponses[i].Question, i);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var survey = await Surveys.FindAsync(Id);
            if (survey == null || survey.IsCompleted)
                return NotFound();

            Context.Entry(survey).Reference(x => x.Student).Load();

            if (!survey.Student.Email.Equals(User.Identity.Name))
                return RedirectToPage("/Account/AccessDenied");

            Context.Entry(survey).Collection(x => x.SurveyResponses).Load();
            foreach (var response in SurveyResponses)
            {
                survey.SurveyResponses.Add(response);
            }
            survey.IsCompleted = true;
            Surveys.Update(survey);
            await Context.SaveChangesAsync();

            return RedirectToPage("/Surveys/Details", new {Id});
        }

        private void ValidateQuestion(SurveyAnswerRate answer, SurveyQuestionRate question, int i)
        {
            if (question.Required)
            {
                if (answer.Selection == 0)
                {
                    ModelState.AddModelError($"SurveyResponses[{i}].Answer.Selection", "Answer is required.");
                }
            }

            if (answer.Selection < 1 || answer.Selection > question.Range)
            {
                ModelState.AddModelError($"SurveyResponses[{i}].Answer.Selection", $"Answer must be between 1 and {question.Range}");
            }
        }

        private void ValidateQuestion(SurveyAnswerQualitative answer, SurveyQuestionQualitative question, int i)
        {
            if (question.Required)
            {
                if (string.IsNullOrEmpty(answer.Response))
                {
                    ModelState.AddModelError($"SurveyResponses[{i}].Answer.Response", "Answer is required.");
                }
            }
        }
    }
}