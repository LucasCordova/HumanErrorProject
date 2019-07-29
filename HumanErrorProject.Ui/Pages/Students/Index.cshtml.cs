using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using HumanErrorProject.Data.DataAccess;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Engine;
using HumanErrorProject.Engine.Data;
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
        public IEmailService EmailService;

        public IndexModel(HumanErrorProjectContext context, IOptions<ViewOptions> options, IEmailService emailService)
        {
            Students = context.Set<Student>();
            Options = options.Value;
            EmailService = emailService;
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

        public async Task<IActionResult> OnPostSendLinkAsync(string id)
        {
            var student = await Students.FindAsync(id);

            if (student == null)
                return NotFound();

            var url = Url.Page("/Account/Register",
                pageHandler: null,
                values: new {Id = student.Id},
                protocol: Request.Scheme);
            
            await EmailService.Send(new EmailData(student)
            {
                Subject = "Data Structures' Setup Link",
                Content = $"Please setup your account by <a href='{HtmlEncoder.Default.Encode(url)}'>clicking here</a>."
            });

            return RedirectToPage();
        }
    }
}