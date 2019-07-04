using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Hangfire;
using HumanErrorProject.Data.DataAccess;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Ui.Constants;
using HumanErrorProject.Ui.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HumanErrorProject.Ui.Pages.PendingAssignments
{
    [Authorize(Roles = IdentityRoleConstants.Admin)]
    public class DetailsModel : PageModel
    {
        public IEngineService EngineService;
        public HumanErrorProjectContext Context;
        public DbSet<PreAssignment> PreAssignments;
        public DbSet<Assignment> Assignments;

        public DetailsModel(IEngineService engineService, HumanErrorProjectContext context)
        {
            EngineService = engineService;
            Context = context;
            PreAssignments = context.Set<PreAssignment>();
            Assignments = context.Set<Assignment>();
        }

        [FromRoute]
        public int Id { get; set; }

        public PreAssignment Assignment { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Assignment = await PreAssignments.FindAsync(Id);

            if (Assignment == null)
                return NotFound();

            Context.Entry(Assignment).Reference(x => x.Solution).Query()
                .Include(x => x.MethodDeclarations).Load();
            Context.Entry(Assignment).Reference(x => x.CourseClass).Load();
            Context.Entry(Assignment).Reference(x => x.TestProject).Query()
                .Include(x => x.UnitTests).Load();
            

            Context.Entry(Assignment).Reference(x => x.PreAssignmentReport).Load();

            switch (Assignment.PreAssignmentReport.Type)
            {
                case PreAssignmentReport.PreAssignmentReportTypes.FailTestsFailure:
                    Context.Entry((PreAssignmentFailTestsFailureReport)Assignment.PreAssignmentReport)
                        .Collection(x => x.FailUnitTests).Load();
                    break;
                case PreAssignmentReport.PreAssignmentReportTypes.MissingMethodsFailure:
                    Context.Entry((PreAssignmentMissingMethodsFailureReport)Assignment.PreAssignmentReport)
                        .Collection(x => x.MissingMethodDeclarations).Load();
                    break;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Assignment = await PreAssignments.FindAsync(Id);
            if (Assignment == null)
                return NotFound();

            Context.Entry(Assignment).Reference(x => x.PreAssignmentReport).Load();

            if (Assignment.PreAssignmentReport.Type == PreAssignmentReport.PreAssignmentReportTypes.Pending)
            {
                BackgroundJob.Enqueue(() => EngineService.RunPreAssignment(Id));
            }
            else if (Assignment.PreAssignmentReport.Type == PreAssignmentReport.PreAssignmentReportTypes.Success)
            {
                Context.Entry(Assignment).Reference(x => x.Solution).Query()
                    .Include(x => x.MethodDeclarations).Load();
                Context.Entry(Assignment).Reference(x => x.CourseClass).Query().Load();
                Context.Entry(Assignment).Reference(x => x.TestProject).Query()
                    .Include(x => x.UnitTests).Load();

                var assignment = new Assignment()
                {
                    CourseClass = Assignment.CourseClass,
                    Filename = Assignment.Filename,
                    Name = Assignment.Name,
                    Solution = Assignment.Solution,
                    TestProject = Assignment.TestProject,
                };
                Assignments.Add(assignment);
                PreAssignments.Remove(Assignment);
                await Context.SaveChangesAsync();
                return RedirectToPage("/Assignments/Details", new {assignment.Id});
            }
            else
            {
                return BadRequest();
            }
            return RedirectToPage(new {Id});
        }

    }
}