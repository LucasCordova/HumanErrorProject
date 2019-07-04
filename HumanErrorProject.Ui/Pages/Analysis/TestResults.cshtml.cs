using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanErrorProject.Data.DataAccess;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Ui.Constants;
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
    public class TestResultsModel : PageModel
    {
        public HumanErrorProjectContext Context;
        public DbSet<Assignment> Assignments;

        public TestResultsModel(HumanErrorProjectContext context)
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
            Context.Entry(Assignment).Reference(x => x.TestProject)
                .Query().Include(x => x.UnitTests).Load();

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
                    Context.Entry((SnapshotSuccessReport)x.Report).Collection(y => y.UnitTestResults).Query()
                        .Include(y => y.UnitTest).Load();
                    return x;
                }).ToList();


            var students = LatestSnapshots.Select(x => x.Student).Distinct();
            ViewData["Students"] = new SelectList(students, "Id", "Name");
            return Page();
        }

        public IActionResult OnPost(int studentId)
        {
            return RedirectToPage("/Analysis/StudentTestResults",
                new {id = Id, studentId});
        }

        
    }
}