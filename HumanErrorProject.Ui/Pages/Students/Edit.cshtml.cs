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
    public class EditModel : PageModel
    {
        public HumanErrorProjectContext Context;
        public DbSet<Student> Students;

        public EditModel(HumanErrorProjectContext context)
        {
            Context = context;
            Students = context.Set<Student>();
        }

        [FromRoute]
        public int Id { get; set; }

        [BindProperty]
        public Student Student { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Student = await Students.FindAsync(Id);

            if (Student == null) return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var student = await Students.FindAsync(Student.Id);

            if (student == null) return NotFound();

            student.Name = Student.Name;
            student.Email = Student.Email;
            Students.Update(student);
            await Context.SaveChangesAsync();

            return RedirectToPage("/Students/Index");
        }

    }
}