using System.Collections;
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

namespace HumanErrorProject.Ui.Pages.CourseClasses
{
    [Authorize(Roles = IdentityRoleConstants.Admin)]
    public class IndexModel : PageModel
    {
        public DbSet<CourseClass> CourseClasses;
        public ViewOptions Options;

        public IndexModel(HumanErrorProjectContext context, IOptions<ViewOptions> options)
        {
            CourseClasses = context.Set<CourseClass>();
            Options = options.Value;
        }

        [FromRoute]
        public int Step { get; set; } = 0;


        public Task<IEnumerable<CourseClass>> GetCourseClasses()
        {
            return Task.FromResult<IEnumerable<CourseClass>>(
                CourseClasses.Skip(Step * Options.StepSize).Take(Options.StepSize)
                .Include(x => x.Assignments)
                .Include(x => x.PreAssignments)
                .Include(x => x.StudentCourseClasses));
        }

        public Task<bool> IsNext()
        {
            var count = CourseClasses.Count();
            return Task.FromResult(Step * Options.StepSize < count - Options.StepSize);
        }

        public Task<bool> IsPrevious() => Task.FromResult(Step > 0);
    }
}