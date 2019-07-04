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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HumanErrorProject.Ui.Pages.PendingAssignments
{
    [Authorize(Roles = IdentityRoleConstants.Admin)]
    public class IndexModel : PageModel
    {
        public HumanErrorProjectContext Context;
        public DbSet<CourseClass> CourseClasses;
        public ViewOptions Options;

        public IndexModel(HumanErrorProjectContext context, IOptions<ViewOptions> options)
        {
            Context = context;
            CourseClasses = context.Set<CourseClass>();
            Options = options.Value;
        }

        [FromRoute]
        public int Id { get; set; }

        [FromRoute] public int Step { get; set; } = 0;

        public CourseClass CourseClass { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            CourseClass = await CourseClasses.FindAsync(Id);

            if (CourseClass == null)
                return NotFound();

            Context.Entry(CourseClass)
                .Collection(x => x.PreAssignments)
                .Query().Include(x => x.Solution)
                .Include(x => x.PreAssignmentReport)
                .Load();

            return Page();
        }

        public Task<IEnumerable<PreAssignment>> GetAssignments()
        {
            return Task.FromResult(
                CourseClass
                    .PreAssignments.Skip(Step * Options.StepSize)
                    .Take(Options.StepSize));
        }

        public Task<bool> IsNext()
        {
            var count = CourseClass.PreAssignments.Count;
            return Task.FromResult(Step * Options.StepSize < count - Options.StepSize);
        }

        public Task<bool> IsPrevious() => Task.FromResult(Step > 0);
    }
}