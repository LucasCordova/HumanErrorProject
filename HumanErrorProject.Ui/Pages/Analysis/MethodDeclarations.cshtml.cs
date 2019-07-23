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
    public class MethodDeclarationsModel : PageModel
    {
        public HumanErrorProjectContext Context;
        public DbSet<Assignment> Assignments;

        public MethodDeclarationsModel(HumanErrorProjectContext context)
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
                        .Include(y => y.MethodDeclaration).Load();
                    return x;
                }).ToList();


            var students = LatestSnapshots.Select(x => x.Student).Distinct();
            ViewData["Students"] = new SelectList(students, "Id", "Name");
            return Page();
        }

        public BarChart GetOverallMethodDeclarationChart()
        {
            return new BarChart()
            {
                Id = "overall_method_declaration",
                Minimum = 0,
                Colors = new List<string>()
                {
                    DataVisualConstants.FailedColor,
                    DataVisualConstants.PassedColor,
                    DataVisualConstants.PrimaryColor
                },
                Labels = new List<string>()
                {
                    "Worst",
                    "Best",
                    "Average"
                },
                Values = new List<int>()
                {
                    LatestSnapshots.Select(x => ((SnapshotSuccessReport)x.Report).SnapshotMethods.Count(y => y.Declared)).Min(),
                    LatestSnapshots.Select(x => ((SnapshotSuccessReport)x.Report).SnapshotMethods.Count(y => y.Declared)).Max(),
                    (int)LatestSnapshots.Select(x => ((SnapshotSuccessReport)x.Report).SnapshotMethods.Count(y => y.Declared)).Average(),
                }
            };
        }

        public IActionResult OnPost(int studentId)
        {
            return RedirectToPage("/Analysis/StudentMethodDeclarations",
                new { id = Id, studentId });
        }

    }
}