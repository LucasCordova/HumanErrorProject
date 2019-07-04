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
    public class CreateModel : PageModel
    {
        public HumanErrorProjectContext Context;
        public DbSet<CourseClass> CourseClasses;

        public CreateModel(HumanErrorProjectContext context)
        {
            Context = context;
            CourseClasses = context.Set<CourseClass>();
        }

        [BindProperty]
        public CourseClass CourseClass { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            CourseClasses.Add(CourseClass);
            await Context.SaveChangesAsync();
            
            return RedirectToPage("/CourseClasses/Index");
        }
    }
}