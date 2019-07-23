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
    public class DeleteModel : PageModel
    {
        public HumanErrorProjectContext Context;
        public DbSet<MarkovModel> MarkovModels;
        public DeleteHelper DeleteHelper;

        public DeleteModel(HumanErrorProjectContext context, DeleteHelper deleteHelper)
        {
            Context = context;
            DeleteHelper = deleteHelper;
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
                .Query().Load();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            MarkovModel = await MarkovModels.FindAsync(Id);

            if (MarkovModel == null) return NotFound();

            await DeleteHelper.Delete(MarkovModel);

            return RedirectToPage("/Analysis/Markov/Index", new { Id = MarkovModel.AssignmentId});
        }


    }
}