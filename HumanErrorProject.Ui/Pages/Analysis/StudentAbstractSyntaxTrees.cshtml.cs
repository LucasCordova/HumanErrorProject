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

namespace HumanErrorProject.Ui.Pages.Analysis
{
    [Authorize(Roles = IdentityRoleConstants.Admin)]
    public class StudentAbstractSyntaxTreesModel : PageModel
    {
        public HumanErrorProjectContext Context;
        public DbSet<Assignment> Assignments;

        public StudentAbstractSyntaxTreesModel(HumanErrorProjectContext context)
        {
            Context = context;
            Assignments = context.Set<Assignment>();
        }


        [FromRoute]
        public int Id { get; set; }

        [FromRoute]
        public string StudentId { get; set; }

        public Assignment Assignment { get; set; }

        public Student Student { get; set; }

        public IList<Snapshot> Snapshots { get; set; } = new List<Snapshot>();

        public async Task<IActionResult> OnGetAsync()
        {
            Assignment = await Assignments.FindAsync(Id);
            if (Assignment == null) return NotFound();

            Context.Entry(Assignment).Collection(x => x.Snapshots)
                .Load();

            Context.Entry(Assignment).Reference(x => x.Solution)
                .Query().Include(x => x.MethodDeclarations).Load();

            Snapshots = Assignment.Snapshots.Where(x => x.StudentId.Equals(StudentId)).
                Select(x =>
                {
                    Context.Entry(x).Reference(y => y.Student).Load();
                    Context.Entry(x).Reference(y => y.Report).Load();
                    if (x.Report.Type == SnapshotReport.SnapshotReportTypes.Success)
                    {
                        Context.Entry((SnapshotSuccessReport)x.Report).Collection(y => y.SnapshotMethods).Query()
                            .Include(y => y.CodeAnalysisMetric)
                            .ThenInclude(y => y.AbstractSyntaxTreeMetric).Load();
                    }
                    return x;
                }).ToList();

            if (Snapshots.Count == 0) return NotFound();

            Student = Snapshots.First().Student;

            Context.Entry(Assignment).Reference(x => x.TestProject)
                .Query().Include(x => x.UnitTests).Load();

            return Page();
        }

        public Task<IEnumerable<Snapshot>> GetBuildSnapshots()
        {
            return Task.FromResult(Snapshots.Where(x => x.Report.Type == SnapshotReport.SnapshotReportTypes.Success));
        }

        public SnapshotMethod GetSnapshotMethod(Snapshot snapshot, MethodDeclaration method)
        {
            return ((SnapshotSuccessReport)snapshot.Report).SnapshotMethods.First(x =>
                x.MethodDeclarationId.Equals(method.Id));
        }

        public AbstractSyntaxTreeMetric GetLeastCodeAnalysisMetricOrDefault(List<Snapshot> snapshots,
            MethodDeclaration method)
        {
            var methods = snapshots.Select(x => GetSnapshotMethod(x, method));
            var declaredMethods = methods.Where(x => x.Declared).ToList();
            if (!declaredMethods.Any()) return null;
            return declaredMethods.OrderBy(x => x.CodeAnalysisMetric.AbstractSyntaxTreeMetric.Distance())
                .First().CodeAnalysisMetric.AbstractSyntaxTreeMetric;
        }

        public AbstractSyntaxTreeMetric GetLatestCodeAnalysisMetricOrDefault(List<Snapshot> snapshots, MethodDeclaration method)
        {
            var latest = snapshots.OrderByDescending(x => x.SnapshotSubmission.CreatedDateTime).First();
            var declared = GetSnapshotMethod(latest, method);
            return !declared.Declared ? null : declared.CodeAnalysisMetric.AbstractSyntaxTreeMetric;
        }
    }
}