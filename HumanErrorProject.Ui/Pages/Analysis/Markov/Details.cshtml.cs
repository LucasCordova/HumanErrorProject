using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanErrorProject.Data.DataAccess;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Ui.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HumanErrorProject.Ui.Pages.Analysis.Markov
{
    [Authorize(Roles = IdentityRoleConstants.Admin)]
    public class DetailsModel : PageModel
    {
        public HumanErrorProjectContext Context;
        public DbSet<MarkovModel> MarkovModels;

        public DetailsModel(HumanErrorProjectContext context)
        {
            Context = context;
            MarkovModels = context.Set<MarkovModel>();
        }

        [FromRoute]
        public int Id { get; set; }

        public MarkovModel MarkovModel { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            MarkovModel = await MarkovModels.FindAsync(Id);

            if (MarkovModel == null) return NotFound();

            Context.Entry(MarkovModel).Reference(x => x.Assignment)
                .Query().Include(x => x.CourseClass)
                .Load();

            Context.Entry(MarkovModel).Collection(x => x.States)
                .Query().Include(x => x.Snapshots)
                .Include(x => x.Transitions).Load();

            return Page();
        }
    }
}