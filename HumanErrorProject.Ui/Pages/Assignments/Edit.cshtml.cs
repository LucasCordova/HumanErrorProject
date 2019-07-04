using System.Threading.Tasks;
using HumanErrorProject.Data.DataAccess;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Ui.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HumanErrorProject.Ui.Pages.Assignments
{
    [Authorize(Roles = IdentityRoleConstants.Admin)]
    public class EditModel : PageModel
    {
        public HumanErrorProjectContext Context;
        public DbSet<Assignment> Assignments;

        public EditModel(HumanErrorProjectContext context)
        {
            Context = context;
            Assignments = context.Set<Assignment>();
        }

        [FromRoute]
        public int Id { get; set; }

        [BindProperty]
        public Assignment Assignment { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Assignment = await Assignments.FindAsync(Id);

            if (Assignment == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Clear();

            if (string.IsNullOrEmpty(Assignment.Name))
            {
                ModelState.AddModelError("Assignment.Name", "Assignment's name is required.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var assignment = await Assignments.FindAsync(Id);

            if (assignment == null) return NotFound();

            assignment.Name = Assignment.Name;

            Assignments.Update(assignment);
            await Context.SaveChangesAsync();

            return RedirectToPage("/Assignments/Index", 
                new {id = assignment.CourseClassId});
        }
    }
}