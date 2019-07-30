using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanErrorProject.Data.DataAccess;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Ui.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HumanErrorProject.Ui.Pages.Analysis
{
    [Authorize(Roles = IdentityRoleConstants.Admin)]
    public class BagOfWordsModel : PageModel
    {
        public HumanErrorProjectContext Context;
        public DbSet<Assignment> Assignments;

        public BagOfWordsModel(HumanErrorProjectContext context)
        {
            Context = context;
            Assignments = context.Set<Assignment>();
        }

        [FromRoute]
        public int Id { get; set; }


        public Assignment Assignment { get; set; }

        public IList<Snapshot> LatestSnapshots { get; set; } = new List<Snapshot>();

        public async Task<IActionResult> OnGetAsync()
        {
            Assignment = await Assignments.FindAsync(Id);
            if (Assignment == null) return NotFound();

            Context.Entry(Assignment).Reference(x => x.CourseClass).Query()
                .Load();
            Context.Entry(Assignment).Reference(x => x.Solution)
                .Query().Include(x => x.MethodDeclarations).Load();

            Context.Entry(Assignment).Collection(x => x.Snapshots).Query()
                .Include(x => x.Report)
                .Load();

            LatestSnapshots = Assignment.Snapshots.Where(x => x.Report.Type == SnapshotReport.SnapshotReportTypes.Success)
                .GroupBy(
                    s => s.StudentId,
                    s => s,
                    (key, g) => new { StudentId = key, Snapshots = g.ToList() })
                .Select(x =>
                {
                    return x.Snapshots.OrderBy(y => y.SnapshotSubmission.CreatedDateTime).Last();
                })
                .Select(x =>
                {
                    Context.Entry((SnapshotSuccessReport)x.Report).Collection(y => y.SnapshotMethods).Query()
                        .Include(y => y.CodeAnalysisMetric)
                        .ThenInclude(y => y.BagOfWordsMetric).Load();
                    return x;
                }).ToList();


            var students = LatestSnapshots.Select(x => x.Student).Distinct();
            ViewData["Students"] = new SelectList(students, "Id", "Name");
            return Page();
        }

        public IActionResult OnPost(string studentId)
        {
            return RedirectToPage("/Analysis/StudentBagOfWords",
                new { id = Id, studentId });
        }

        public SnapshotMethod GetSnapshotMethod(Snapshot snapshot, MethodDeclaration method)
        {
            return ((SnapshotSuccessReport)snapshot.Report).SnapshotMethods.First(x =>
               x.MethodDeclarationId.Equals(method.Id));
        }

        public BagOfWordsMetric GetLeastCodeAnalysisMetricOrDefault(IList<Snapshot> snapshots, MethodDeclaration method)
        {
            var methods = snapshots.Select(x => GetSnapshotMethod(x, method));
            var declaredMethods = methods.Where(x => x.Declared).ToList();
            if (!declaredMethods.Any()) return null;
            return declaredMethods
                .OrderBy(x => x.CodeAnalysisMetric.BagOfWordsMetric.Difference).First()
                .CodeAnalysisMetric.BagOfWordsMetric;
        }

        public BagOfWordsMetric GetAverageCodeAnalysisMetricOrDefault(IList<Snapshot> snapshots, MethodDeclaration method)
        {
            var methods = snapshots.Select(x => GetSnapshotMethod(x, method));
            var declaredMethods = methods.Where(x => x.Declared).ToList();
            if (!declaredMethods.Any()) return null;
            return new BagOfWordsMetric()
            {
                Difference = declaredMethods.Select(x => x.CodeAnalysisMetric.BagOfWordsMetric.Difference).Average(),
            };
        }

        public BagOfWordsMetric GetMostCodeAnalysisMetricOrDefault(IList<Snapshot> snapshots, MethodDeclaration method)
        {
            var methods = snapshots.Select(x => GetSnapshotMethod(x, method));
            var declaredMethods = methods.Where(x => x.Declared).ToList();
            if (!declaredMethods.Any()) return null;
            return declaredMethods.OrderByDescending(x => x.CodeAnalysisMetric.BagOfWordsMetric.Difference).First()
                .CodeAnalysisMetric.BagOfWordsMetric;
        }

    }
}