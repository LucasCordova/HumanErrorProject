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
    public class StudentTestResultsModel : PageModel
    {
        public HumanErrorProjectContext Context;
        public DbSet<Assignment> Assignments;

        public StudentTestResultsModel(HumanErrorProjectContext context)
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

            Context.Entry(Assignment).Collection(x => x.Snapshots)
                .Load();

            Snapshots = Assignment.Snapshots.Where(x => x.StudentId.Equals(StudentId)).
                Select(x =>
                {
                    Context.Entry(x).Reference(y => y.Student).Load();
                    Context.Entry(x).Reference(y => y.Report).Load();
                    if (x.Report.Type == SnapshotReport.SnapshotReportTypes.Success)
                    {
                        Context.Entry((SnapshotSuccessReport)x.Report).Collection(y => y.UnitTestResults).Query()
                            .Include(y => y.UnitTest).Load();
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
    }
}