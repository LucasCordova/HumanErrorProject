using System.Linq;
using System.Threading.Tasks;
using HumanErrorProject.Data.DataAccess;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Engine.Options;
using HumanErrorProject.Ui.Constants;
using HumanErrorProject.Ui.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HumanErrorProject.Ui.Pages.Analysis.Markov
{
    [Authorize(Roles = IdentityRoleConstants.Admin)]
    public class CreateModel : PageModel
    {
        public HumanErrorProjectContext Context;
        public DbSet<Assignment> Assignments;
        public IEngineService EngineService;

        public CreateModel(HumanErrorProjectContext context, IEngineService engineService)
        {
            Context = context;
            Assignments = context.Set<Assignment>();
            EngineService = engineService;
        }

        [FromRoute]
        public int Id { get; set; }

        public Assignment Assignment { get; set; }

        [BindProperty]
        public MarkovModelOptions Options { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Assignment = await Assignments.FindAsync(Id);
            if (Assignment == null) return NotFound();
            Context.Entry(Assignment).Collection(x => x.Snapshots)
                .Query().Include(x => x.Report).Load();
            Options = new MarkovModelOptions();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Assignment = await Assignments.FindAsync(Id);
            if (Assignment == null) return NotFound();

            Context.Entry(Assignment).Collection(x => x.Snapshots).Load();

            if (!ModelState.IsValid)
                return Page();

            var snapshots = Assignment.Snapshots.ToList();
            if (Options.BuildOnly)
                snapshots = snapshots.Where(x => x.Report.Type == SnapshotReport.SnapshotReportTypes.Success).ToList();


            if (snapshots.Count < 2)
            {
                ModelState.AddModelError("Options.NumberOfStates", "Not enough snapshots");
            }

            if (Options.NumberOfStates >= snapshots.Count)
            {
                ModelState.AddModelError("Options.NumberOfStates", "Too many states, not enough snapshots");
            }

            if (!ModelState.IsValid)
                return Page();

            EngineService.RunMarkovModel(Id, Options);


            return RedirectToPage("/Analysis/Markov/Index", new {Id});
        }

    }
}