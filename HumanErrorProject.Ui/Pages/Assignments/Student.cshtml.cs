using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanErrorProject.Data.DataAccess;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Ui.Constants;
using HumanErrorProject.Ui.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HumanErrorProject.Ui.Pages.Assignments
{
    [Authorize(Roles = IdentityRoleConstants.Student)]
    public class StudentModel : PageModel
    {
        public HumanErrorProjectContext Context;
        public DbSet<Student> Students;
        public ViewOptions Options;

        public StudentModel(HumanErrorProjectContext context, IOptions<ViewOptions> options)
        {
            Context = context;
            Students = context.Set<Student>();
            Options = options.Value;
        }

        [FromRoute] public int Step { get; set; } = 0;

        public Student Student { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Student = await Students.SingleOrDefaultAsync(x => x.Email.Equals(User.Identity.Name));
            if (Student == null) return NotFound();

            return Page();
        }

        public Task<IEnumerable<Assignment>> GetAssignments()
        {
            Context.Entry(Student).Collection(x => x.StudentCourseClasses)
                .Query().Include(x => x.Class)
                .ThenInclude(x => x.Assignments)
                .ThenInclude(x => x.Solution).Load();

            var assignments = Student.StudentCourseClasses.SelectMany(
                x => x.Class.Assignments)
                .Skip(Step * Options.StepSize)
                .Take(Options.StepSize);
            return Task.FromResult(assignments);
        }

        public Task<bool> IsNext()
        {
            Context.Entry(Student).Collection(x => x.StudentCourseClasses)
                .Query().Include(x => x.Class)
                .ThenInclude(x => x.Assignments)
                .ThenInclude(x => x.Solution).Load();

            var assignments = Student.StudentCourseClasses.SelectMany(
                x => x.Class.Assignments);
            var count = assignments.Count();
            return Task.FromResult(Step * Options.StepSize < count - Options.StepSize);
        }

        public Task<bool> IsPrevious() => Task.FromResult(Step > 0);


    }
}