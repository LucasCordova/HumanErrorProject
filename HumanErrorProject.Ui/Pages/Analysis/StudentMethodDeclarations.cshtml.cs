using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanErrorProject.Data.DataAccess;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Ui.Constants;
using HumanErrorProject.Ui.DataVisuals;
using HumanErrorProject.Ui.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HumanErrorProject.Ui.Pages.Analysis
{
    [Authorize(Roles = IdentityRoleConstants.Admin)]
    public class StudentMethodDeclarationsModel : PageModel
    {
        public HumanErrorProjectContext Context;
        public DbSet<Assignment> Assignments;

        public StudentMethodDeclarationsModel(HumanErrorProjectContext context)
        {
            Context = context;
            Assignments = context.Set<Assignment>();
        }

        [FromRoute]
        public int Id { get; set; }

        [FromRoute]
        public int StudentId { get; set; }

        public Assignment Assignment { get; set; }

        public Student Student { get; set; }

        public IList<Snapshot> Snapshots { get; set; } = new List<Snapshot>();

        public async Task<IActionResult> OnGetAsync()
        {
            Assignment = await Assignments.FindAsync(Id);
            if (Assignment == null) return NotFound();

            Snapshots = Assignment.Snapshots.Where(x => x.StudentId.Equals(StudentId)).
                Select(x =>
                {
                    Context.Entry(x).Reference(y => y.Student).Load();
                    Context.Entry(x).Reference(y => y.Report).Load();
                    if (x.Report.Type == SnapshotReport.SnapshotReportTypes.Success)
                    {
                        Context.Entry((SnapshotSuccessReport)x.Report).Collection(y => y.SnapshotMethods).Query()
                            .Include(y => y.MethodDeclaration).Load();
                    }
                    return x;
                }).ToList();

            if (Snapshots.Count == 0) return NotFound();

            Student = Snapshots.First().Student;

            Context.Entry(Assignment).Reference(x => x.Solution)
                .Query().Include(x => x.MethodDeclarations).Load();

            return Page();
        }

        public Task<IEnumerable<Snapshot>> GetBuildSnapshots()
        {
            return Task.FromResult(Snapshots.Where(x => x.Report.Type == SnapshotReport.SnapshotReportTypes.Success));
        }


        public BarChart GetOverallMethodDeclarationsChart(IList<Snapshot> snapshots)
        {
            return new BarChart()
            {
                Id = "overall_method_declaration",
                Minimum = 0,
                Colors = new List<string>()
                {
                    DataVisualConstants.PassedColor,
                    DataVisualConstants.PrimaryColor
                },
                Labels = new List<string>()
                {
                    "Best",
                    "Latest"
                },
                Values = new List<int>()
                {
                    snapshots.Select(x => ((SnapshotSuccessReport)x.Report).SnapshotMethods.Count(y => y.Declared)).Max(),
                    ((SnapshotSuccessReport)snapshots.Last().Report).SnapshotMethods.Count(y => y.Declared),
                }
            };
        }

    }
}