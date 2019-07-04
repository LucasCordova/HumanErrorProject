using System.Threading.Tasks;
using HumanErrorProject.Data.DataAccess;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Ui.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HumanErrorProject.Ui.Pages.CourseClasses
{
    [Authorize(Roles = IdentityRoleConstants.Admin)]
    public class DeleteModel : PageModel
    {
        public HumanErrorProjectContext Context;
        public DbSet<CourseClass> CourseClasses;
        public DeleteModel(HumanErrorProjectContext context)
        {
            Context = context;
            CourseClasses = context.Set<CourseClass>();
        }

        [FromRoute]
        public int Id { get; set; }

        public CourseClass CourseClass { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            CourseClass = await CourseClasses.FindAsync(Id);

            if (CourseClass == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            CourseClass = await CourseClasses.FindAsync(Id);

            if (CourseClass == null) return NotFound();

            CourseClasses.Remove(CourseClass);
            await Context.SaveChangesAsync();

            return RedirectToPage("/CourseClasses/Index");
        }
    }
}