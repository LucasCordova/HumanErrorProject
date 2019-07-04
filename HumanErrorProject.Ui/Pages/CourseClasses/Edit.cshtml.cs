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

namespace HumanErrorProject.Ui.Pages.CourseClasses
{
    [Authorize(Roles = IdentityRoleConstants.Admin)]
    public class EditModel : PageModel
    {
        public HumanErrorProjectContext Context;
        public DbSet<CourseClass> CourseClasses;

        public EditModel(HumanErrorProjectContext context)
        {
            Context = context;
            CourseClasses = context.Set<CourseClass>();
        }

        [FromRoute]
        public int Id { get; set; }

        [BindProperty]
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
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var course = await CourseClasses.FindAsync(Id);

            if (course == null) return NotFound();

            course.Name = CourseClass.Name;
            course.Term = CourseClass.Term;
            course.Course = CourseClass.Course;

            CourseClasses.Update(course);
            await Context.SaveChangesAsync();

            return RedirectToPage("/CourseClasses/Index");
        }
    }
}