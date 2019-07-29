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
    public class StudentsModel : PageModel
    {
        public HumanErrorProjectContext Context;
        public DbSet<CourseClass> CourseClasses;
        public DbSet<Student> Students;

        public StudentsModel(HumanErrorProjectContext context)
        {
            Context = context;
            CourseClasses = context.Set<CourseClass>();
            Students = context.Set<Student>();
        }

        [FromRoute]
        public int Id { get; set; }

        public CourseClass CourseClass { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            CourseClass = await CourseClasses.FindAsync(Id);

            if (CourseClass == null)
                return NotFound();

            Context.Entry(CourseClass).Collection(x => x.StudentCourseClasses)
                .Query().Include(x => x.Student)
                .ThenInclude(x => x.StudentCourseClasses)
                .ThenInclude(x => x.Class).Load();

            return Page();
        }

        public Task<IEnumerable<Student>> GetStudents()
        {
            return Task.FromResult<IEnumerable<Student>>(
                Students.Include(x => x.StudentCourseClasses)
                );
        }

        public Task<bool> IsStudentInClass(Student student)
        {
            return Task.FromResult(student.StudentCourseClasses
                        .Any(c => c.CourseClassId.Equals(Id)));
        }


        public async Task<IActionResult> OnPostAddStudentAsync(string studentId)
        {
            CourseClass = await CourseClasses.FindAsync(Id);
            if (CourseClass == null) return NotFound();

            var student = await Students.FindAsync(studentId);
            if (student == null) return NotFound();

            if (await IsStudentInClass(student)) return BadRequest();

            student.StudentCourseClasses.Add(
                new StudentCourseClass()
                {
                    Class = CourseClass,
                    Student = student
                });

            Students.Update(student);
            await Context.SaveChangesAsync();

            return RedirectToPage(new {id = Id});
        }

        public async Task<IActionResult> OnPostRemoveStudentAsync(string studentId)
        {
            CourseClass = await CourseClasses.FindAsync(Id);
            if (CourseClass == null) return NotFound();

            var student = await Students.FindAsync(studentId);
            if (student == null) return NotFound();

            if (!await IsStudentInClass(student)) return BadRequest();

            var studentClass = student.StudentCourseClasses.First(c => c.CourseClassId.Equals(CourseClass.Id));

            student.StudentCourseClasses.Remove(studentClass);
            Students.Update(student);
            await Context.SaveChangesAsync();

            return RedirectToPage(new { id = Id });
        }
    }
}