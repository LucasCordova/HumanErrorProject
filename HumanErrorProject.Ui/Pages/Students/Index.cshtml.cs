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

namespace HumanErrorProject.Ui.Pages.Students
{
    [Authorize(Roles = IdentityRoleConstants.Admin)]
    public class IndexModel : PageModel
    {
        public DbSet<Student> Students;
        public ViewOptions Options;

        public IndexModel(HumanErrorProjectContext context, IOptions<ViewOptions> options)
        {
            Students = context.Set<Student>();
            Options = options.Value;
        }

        [FromRoute] public int Step { get; set; } = 0;

        public Task<IEnumerable<Student>> GetStudents()
        {
            return Task.FromResult<IEnumerable<Student>>(
                Students.Skip(Step * Options.StepSize)
                .Take(Options.StepSize).Include(s => s.StudentCourseClasses)
                .Include(s => s.Submissions));
        }

        public Task<bool> IsNext()
        {
            var count = Students.Count();
            return Task.FromResult(Step * Options.StepSize < count - Options.StepSize);
        }

        public Task<bool> IsPrevious() => Task.FromResult(Step > 0);
    }
}