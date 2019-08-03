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

namespace HumanErrorProject.Ui.Pages.Snapshots
{
    [Authorize(Roles = IdentityRoleConstants.Admin)]
    public class AdminDetailsModel : PageModel
    {
        public HumanErrorProjectContext Context;
        public DbSet<Snapshot> Snapshots;

        public AdminDetailsModel(HumanErrorProjectContext context)
        {
            Context = context;
            Snapshots = context.Set<Snapshot>();
        }

        [FromRoute]
        public int Id { get; set; }

        public string ReturnUrl { get; set; }

        public Snapshot Snapshot { get; set; }


        public async Task<IActionResult> OnGetAsync(string returnUrl)
        {
            Snapshot = await Snapshots.FindAsync(Id);

            if (Snapshot == null) return NotFound();

            ReturnUrl = returnUrl;

            Context.Entry(Snapshot)
                .Reference(x => x.Student).Load();

            Context.Entry(Snapshot)
                .Reference(x => x.Assignment).Load();

            Context.Entry(Snapshot)
                .Reference(x => x.Report).Load();

            if (Snapshot.Report.Type
                != SnapshotReport.SnapshotReportTypes.Success)
                return Page();

            Context.Entry((SnapshotSuccessReport)Snapshot.Report)
                .Collection(x => x.SnapshotMethods)
                .Query().Include(x => x.MethodDeclaration).Load();
            Context.Entry((SnapshotSuccessReport)Snapshot.Report)
                .Collection(x => x.UnitTestResults)
                .Query().Include(x => x.UnitTest)
                .Load();


            return Page();
        }
    }
}