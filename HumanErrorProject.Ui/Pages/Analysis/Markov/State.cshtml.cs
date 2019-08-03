using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanErrorProject.Data.DataAccess;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Ui.Constants;
using HumanErrorProject.Ui.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HumanErrorProject.Ui.Pages.Analysis.Markov
{
    [Authorize(Roles = IdentityRoleConstants.Admin)]
    public class StateModel : PageModel
    {
        public HumanErrorProjectContext Context;
        public DbSet<MarkovModel> MarkovModels;
        public DbSet<Snapshot> Snapshots;
        public ColorHelper ColorHelper;

        public StateModel(HumanErrorProjectContext context, ColorHelper colorHelper)
        {
            Context = context;
            MarkovModels = context.Set<MarkovModel>();
            Snapshots = context.Set<Snapshot>();
            ColorHelper = colorHelper;
        }

        [FromRoute]
        public int Id { get; set; }

        [FromRoute]
        public int State { get; set; }

        public MarkovModel MarkovModel { get; set; }
        public MarkovModelState MarkovModelState { get; set; }

        public string ReturnUrl { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            MarkovModel = await MarkovModels.FindAsync(Id);

            if (MarkovModel == null || !MarkovModel.Finished) return NotFound();

            Context.Entry(MarkovModel).Reference(x => x.Assignment)
                .Query().Include(x => x.CourseClass)
                .Load();

            Context.Entry(MarkovModel).Collection(x => x.States)
                .Query().Include(x => x.Snapshots)
                .Include(x => x.Transitions).Load();

            MarkovModelState = MarkovModel.States.FirstOrDefault(x => x.Number.Equals(State));

            if (MarkovModelState == null) return NotFound();

            ReturnUrl = Url.Page("/Analysis/Markov/State", new
            {
                Id,
                State,
            });



            return Page();
        }

    }
}