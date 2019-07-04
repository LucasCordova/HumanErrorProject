using System.Threading.Tasks;
using HumanErrorProject.Data.DataAccess;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Ui.Constants;
using HumanErrorProject.Ui.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HumanErrorProject.Ui.Pages.PendingAssignments
{
    [Authorize(Roles = IdentityRoleConstants.Admin)]
    public class DeleteModel : PageModel
    {
        public DeleteHelper DeleteHelper;
        public HumanErrorProjectContext Context;
        public DbSet<PreAssignment> PreAssignments;

        public DeleteModel(DeleteHelper deleteHelper, HumanErrorProjectContext context)
        {
            DeleteHelper = deleteHelper;
            Context = context;
            PreAssignments = context.Set<PreAssignment>();
        }

        [FromRoute]
        public int Id { get; set; }

        public PreAssignment Assignment { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Assignment = await PreAssignments.FindAsync(Id);

            if (Assignment == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Assignment = await PreAssignments.FindAsync(Id);

            if (Assignment == null) return NotFound();

            var courseId = Assignment.CourseClassId;

            await DeleteHelper.Delete(Assignment);

            return RedirectToPage("/CourseClasses/Details", new {id = courseId});
        }
    }
}