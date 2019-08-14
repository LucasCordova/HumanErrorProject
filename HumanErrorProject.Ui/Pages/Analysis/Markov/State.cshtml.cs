using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanErrorProject.Data.DataAccess;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Ui.Constants;
using HumanErrorProject.Ui.DataVisuals;
using HumanErrorProject.Ui.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HumanErrorProject.Ui.Pages.Analysis.Markov
{
    [Authorize(Roles = IdentityRoleConstants.Admin)]
    public class StateModel : PageModel
    {
        public HumanErrorProjectContext Context;
        public DbSet<MarkovModel> MarkovModels;
        public DbSet<Snapshot> Snapshots;
        public DbSet<SurveyQuestion> SurveyQuestions;
        public ColorHelper ColorHelper;

        public StateModel(HumanErrorProjectContext context, ColorHelper colorHelper)
        {
            Context = context;
            MarkovModels = context.Set<MarkovModel>();
            Snapshots = context.Set<Snapshot>();
            SurveyQuestions = context.Set<SurveyQuestion>();
            ColorHelper = colorHelper;
        }

        [FromRoute]
        public int Id { get; set; }

        [FromRoute]
        public int State { get; set; }

        public MarkovModel MarkovModel { get; set; }
        public MarkovModelState MarkovModelState { get; set; }

        public string ReturnUrl { get; set; }

        public IList<Snapshot> MarkovSnapshots { get; set; } = new List<Snapshot>();
        public int DeletedSnapshots { get; set; }

        public IList<Snapshot> SuccessfulSnapshots { get; set; } = new List<Snapshot>(); 

        public IList<Survey> Surveys { get; set; } = new List<Survey>();

        public async Task<IActionResult> OnGetAsync()
        {
            MarkovModel = await MarkovModels.FindAsync(Id);

            if (MarkovModel == null || !MarkovModel.Finished) return NotFound();

            Context.Entry(MarkovModel).Reference(x => x.Assignment)
                .Query().Include(x => x.CourseClass)
                .Include(x => x.TestProject).ThenInclude(x => x.UnitTests)
                .Load();

            Context.Entry(MarkovModel).Collection(x => x.States)
                .Query().Include(x => x.Snapshots)
                .Include(x => x.Transitions).Load();

            MarkovModelState = MarkovModel.States.FirstOrDefault(x => x.Number.Equals(State));

            if (MarkovModelState == null) return NotFound();

            ReturnUrl = Url.Page("/Analysis/Markov/State", new
            {
                Id,
                State,
            });

            MarkovModelState.Snapshots.ToList()
                .ForEach(x => MarkovSnapshots
                    .Add(Snapshots
                        .Include(y => y.SnapshotSubmission)
                        .Include(y => y.Student)
                        .Include(y => y.Survey)
                        .Include(y => y.Report)
                        .FirstOrDefault(y => y.Id.Equals(x.SnapshotId))));

            DeletedSnapshots =
                MarkovSnapshots.Count(x => 
                    MarkovModelState
                        .Snapshots.All(y => !y.SnapshotId.Equals(x.Id)));

            Surveys = MarkovSnapshots.Select(x =>
            {
                Context.Entry(x.Survey).Collection(y => y.SurveyResponses).Query()
                    .Include(y => y.Answer).Include(y => y.Question).Load();
                return x.Survey;
            }).ToList();

            SuccessfulSnapshots = MarkovSnapshots.Where(x => x.Report.Type.Equals(SnapshotReport.SnapshotReportTypes.Success))
                .Select(x =>
                {
                    Context.Entry((SnapshotSuccessReport)x.Report).Collection(y => y.UnitTestResults).Query()
                        .Include(y => y.UnitTest).Load();
                    return x;
                }).ToList();

            return Page();
        }

        public IList<SurveyAnswer> GetSurveyAnswers(SurveyQuestion question)
        {
            return Surveys.Where(x => x.IsCompleted)
                .SelectMany(x => x.SurveyResponses)
                .Where(x => x.SurveyQuestionId.Equals(question.Id) && x.Question.CourseClassId.Equals(MarkovModel.Assignment.CourseClassId))
                .Select(x => x.Answer).ToList();
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

        public DonutChart GetOverallBuildChart()
        {
            var success = MarkovSnapshots.Count(x =>
                x.Report.Type == Data.Models.SnapshotReport.SnapshotReportTypes.Success);
            var failure = MarkovSnapshots.Count(x =>
                x.Report.Type == Data.Models.SnapshotReport.SnapshotReportTypes.Failure);
            return new DonutChart()
            {
                Id = "assignment_overall_builds",
                Colors = new List<string>()
                {
                    Constants.DataVisualConstants.PassedColor,
                    Constants.DataVisualConstants.FailedColor
                },
                Values = new List<int>()
                {
                    success,
                    failure
                },
                Text = $"{Math.Floor((double)success / (success + failure) * 100):F0}%"
            };
        }


    }
}