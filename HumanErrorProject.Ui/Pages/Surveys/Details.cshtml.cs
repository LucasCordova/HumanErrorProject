using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using HumanErrorProject.Data.DataAccess;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Ui.Constants;
using HumanErrorProject.Ui.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HumanErrorProject.Ui.Pages.Surveys
{
    [Authorize(Roles = IdentityRoleConstants.Student)]
    public class DetailsModel : PageModel
    {
        public HumanErrorProjectContext Context;
        public DbSet<Survey> Surveys;
        public ViewOptions Options;

        public DetailsModel(HumanErrorProjectContext context, IOptions<ViewOptions> options)
        {
            Context = context;
            Surveys = context.Set<Survey>();
            Options = options.Value;
        }

        [FromRoute]
        public string Id { get; set; }

        [FromRoute] public int Step { get; set; } = 0;

        public Survey Survey { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Survey = await Surveys.FindAsync(Id);
            if (Survey == null) return NotFound();

            Context.Entry(Survey).Reference(x => x.Student).Load();

            if (!Survey.Student.Email.Equals(User.Identity.Name))
                return RedirectToPage("/Account/AccessDenied");

            return Page();
        }

        public Task<IEnumerable<Snapshot>> GetSnapshots()
        {
            Context.Entry(Survey).Collection(x => x.Snapshots)
                .Load();

            var asssignmentGroups = Survey.Snapshots
                .GroupBy(x => x.AssignmentId)
                .Skip(Step * Options.StepSize).Take(Options.StepSize)
                .Select(x => new
                {
                    AssignmentId = x.Key,
                    Snapshots = x.OrderByDescending(y => y.SnapshotSubmission.CreatedDateTime).ToList(),
                })
                .ToList();

            return Task.FromResult(asssignmentGroups
                .Select(a => a.Snapshots.First()));
        }

        public Task<bool> IsNext()
        {
            Context.Entry(Survey).Collection(x => x.Snapshots).Load();

            var asssignmentGroups = Survey.Snapshots
                .GroupBy(x => x.AssignmentId)
                .Select(x => new
                {
                    AssignmentId = x.Key,
                    Snapshots = x.OrderBy(y => y.SnapshotSubmission.CreatedDateTime).ToList(),
                })
                .ToList();
            var count = asssignmentGroups.Count();
            return Task.FromResult(Step * Options.StepSize < count - Options.StepSize);
        }

        public Task<bool> IsPrevious() => Task.FromResult(Step > 0);
    }
}