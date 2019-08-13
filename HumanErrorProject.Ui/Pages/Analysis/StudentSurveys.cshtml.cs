using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanErrorProject.Data.DataAccess;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Ui.Constants;
using HumanErrorProject.Ui.DataVisuals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HumanErrorProject.Ui.Pages.Analysis
{
    [Authorize(Roles = IdentityRoleConstants.Admin)]
    public class StudentSurveysModel : PageModel
    {
        public HumanErrorProjectContext Context;
        public DbSet<Assignment> Assignments;
        public DbSet<SurveyQuestion> SurveyQuestions;

        public StudentSurveysModel(HumanErrorProjectContext context)
        {
            Context = context;
            Assignments = context.Set<Assignment>();
            SurveyQuestions = context.Set<SurveyQuestion>();
        }

        [FromRoute]
        public int Id { get; set; }

        [FromRoute]
        public string StudentId { get; set; }

        public Assignment Assignment { get; set; }

        public Student Student { get; set; }

        public IList<Survey> Surveys { get; set; } = new List<Survey>();

        public async Task<IActionResult> OnGetAsync()
        {
            Assignment = await Assignments.FindAsync(Id);
            if (Assignment == null) return NotFound();

            Context.Entry(Assignment).Reference(x => x.CourseClass).Query()
                .Load();

            Context.Entry(Assignment).Collection(x => x.Snapshots).Query()
                .Include(x => x.Survey)
                .Load();

            Surveys = Assignment.Snapshots.Select(x =>
            {
                Context.Entry(x.Survey).Collection(y => y.SurveyResponses).Query()
                    .Include(y => y.Answer).Include(y => y.Question).Load();
                return x.Survey;
            }).Where(x => x.StudentId.Equals(StudentId)).ToList();

            if (Surveys.Count == 0) return NotFound();

            Student = Surveys.First().Student;

            return Page();
        }

        public IList<SurveyAnswer> GetSurveyAnswers(SurveyQuestion question)
        {
            return Surveys.Where(x => x.IsCompleted)
                .SelectMany(x => x.SurveyResponses)
                .Where(x => x.SurveyQuestionId.Equals(question.Id) && x.Question.CourseClassId.Equals(Assignment.CourseClassId))
                .Select(x => x.Answer).ToList();
        }

        public SurveyAnswer GetLatestSurveyAnswerOrDefault(SurveyQuestion question)
        {
            return Surveys.OrderBy(x => x.PostedTime)
                .Where(x => x.IsCompleted).OrderByDescending(x => x.PostedTime)
                .FirstOrDefault()?.SurveyResponses
                .FirstOrDefault(x => x.SurveyQuestionId.Equals(question.Id) && x.Question.CourseClassId.Equals(Assignment.CourseClassId))
                ?.Answer;
        }

        public DonutChart GetOverallSurveyChart()
        {
            var complete = Surveys.Count(x => x.IsCompleted);
            var incomplete = Surveys.Count(x => !x.IsCompleted);
            return new DonutChart()
            {
                Id = "overall_survey_chart",
                Colors = new List<string>()
                {
                    DataVisualConstants.PassedColor,
                    DataVisualConstants.FailedColor,
                },
                Values = new List<int>()
                {
                    complete,
                    incomplete
                },
                Text = $"{Math.Floor((double)complete / (complete + incomplete) * 100):F0}%"
            };
        }

        public BoxAndWhiskerChart GetBoxAndWhiskerChart(SurveyQuestionRate question, IList<SurveyAnswerRate> answers)
        {
            return new BoxAndWhiskerChart()
            {
                Color = DataVisualConstants.PassedColor,
                Id = $"question_{question.Id}",
                Values = answers.Select(x => x.Selection).ToList()
            };
        }
    }
}