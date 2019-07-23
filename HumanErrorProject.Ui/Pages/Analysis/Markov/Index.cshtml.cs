using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanErrorProject.Data.DataAccess;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Ui.Constants;
using HumanErrorProject.Ui.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HumanErrorProject.Ui.Pages.Analysis.Markov
{
    [Authorize(Roles = IdentityRoleConstants.Admin)]
    public class IndexModel : PageModel
    {
        public HumanErrorProjectContext Context;
        public DbSet<Assignment> Assignments;
        public DbSet<MarkovModel> Models;
        public ViewOptions Options;

        public IndexModel(HumanErrorProjectContext context, IOptions<ViewOptions> options)
        {
            Context = context;
            Assignments = context.Set<Assignment>();
            Models = context.Set<MarkovModel>();
            Options = options.Value;
        }

        [FromRoute]
        public int Id { get; set; }

        [FromRoute] public int Step { get; set; } = 0;

        public Assignment Assignment { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Assignment = await Assignments.FindAsync(Id);
            if (Assignment == null) return NotFound();

            return Page();
        }

        public Task<IEnumerable<MarkovModel>> GetModels()
        {
            var models = Models.Where(x => x.AssignmentId.Equals(Assignment.Id))
                .Skip(Step * Options.StepSize).Take(Options.StepSize).ToList();
            return Task.FromResult<IEnumerable<MarkovModel>>(models);
        }

        public Task<bool> IsNext()
        {
            var count = Models.Count(x => x.AssignmentId.Equals(Assignment.Id));
            return Task.FromResult(Step * Options.StepSize < count - Options.StepSize);
        }

        public Task<bool> IsPrevious() => Task.FromResult(Step > 0);

    }
}