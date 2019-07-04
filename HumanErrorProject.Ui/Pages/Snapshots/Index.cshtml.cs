using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanErrorProject.Data.DataAccess;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Ui.Constants;
using HumanErrorProject.Ui.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HumanErrorProject.Ui.Pages.Snapshots
{
    [Authorize(Roles = IdentityRoleConstants.Student)]
    public class IndexModel : PageModel
    {
        public HumanErrorProjectContext Context;
        public DbSet<Student> Students;
        public ViewOptions Options;

        public IndexModel(HumanErrorProjectContext context, IOptions<ViewOptions> options)
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


        public Task<IEnumerable<Snapshot>> GetSnapshots()
        {
            Context.Entry(Student).Collection(x => x.Snapshots)
                .Load();
            return Task.FromResult(Student.Snapshots
                .Skip(Step * Options.StepSize).Take(Options.StepSize)
                .Select(x =>
                {
                    Context.Entry(x)
                        .Reference(y => y.Assignment)
                        .Load();
                    Context.Entry(x)
                        .Reference(y => y.Report)
                        .Load();
                    return x;
                }));
        }

        public Task<bool> IsNext()
        {
            Context.Entry(Student).Collection(x => x.Snapshots)
                .Load();
            var count = Student.Snapshots.Count;
            return Task.FromResult(Step * Options.StepSize < count - Options.StepSize);
        }
        public Task<bool> IsPrevious() => Task.FromResult(Step > 0);
    }
}