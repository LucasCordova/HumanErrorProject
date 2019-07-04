using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanErrorProject.Data.DataAccess;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Ui.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HumanErrorProject.Ui.Pages.Assignments
{
    [Authorize(Roles = IdentityRoleConstants.Student)]
    public class ProgressModel : PageModel
    {
        public HumanErrorProjectContext Context;
        public DbSet<Student> Students;
        public DbSet<Assignment> Assignments;

        public ProgressModel(HumanErrorProjectContext context)
        {
            Context = context;
            Students = context.Set<Student>();
            Assignments = context.Set<Assignment>();
        }

        [FromRoute]
        public int Id { get; set; }

        public Student Student { get; set; }

        public Assignment Assignment { get; set; }

        public IList<Snapshot> Snapshots { get; set; } = new List<Snapshot>();

        public async Task<IActionResult> OnGetAsync()
        {
            Student = await Students.SingleOrDefaultAsync(x => x.Email.Equals(User.Identity.Name));
            if (Student == null) return NotFound();

            Assignment = await Assignments.FindAsync(Id);
            if (Assignment == null) return NotFound();

            Snapshots = Student.Snapshots.Where(x => x.AssignmentId.Equals(Id))
                .Select(x =>
                {
                    Context.Entry(x).Reference(y => y.Report).Load();
                    if (x.Report.Type != SnapshotReport.SnapshotReportTypes.Success)
                        return x;

                    Context.Entry((SnapshotSuccessReport)x.Report)
                        .Collection(y => y.SnapshotMethods)
                        .Query().Include(y => y.MethodDeclaration).Load();
                    Context.Entry((SnapshotSuccessReport)x.Report)
                        .Collection(y => y.UnitTestResults)
                        .Query().Include(y => y.UnitTest)
                        .Load();

                    return x;
                })
                .OrderByDescending(x => x.SnapshotSubmission.CreatedDateTime)
                .ToList();
            return Page();
        }

    }
}