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

namespace HumanErrorProject.Ui.Pages.Students
{
    [Authorize(Roles = IdentityRoleConstants.Admin)]
    public class DeleteModel : PageModel
    {
        public HumanErrorProjectContext Context;
        public DbSet<Student> Students;

        public DeleteModel(HumanErrorProjectContext context)
        {
            Context = context;
            Students = Context.Set<Student>();
        }

        [FromRoute]
        public string Id { get; set; }

        public Student Student { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Student = await Students.FindAsync(Id);

            if (Student == null)
                return NotFound();


            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Student = await Students.FindAsync(Id);

            if (Student == null) return NotFound();

            Students.Remove(Student);
            await Context.SaveChangesAsync();

            return RedirectToPage("/Students/Index");
        }

    }
}