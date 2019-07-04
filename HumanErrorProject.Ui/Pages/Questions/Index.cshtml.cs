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

namespace HumanErrorProject.Ui.Pages.Questions
{
    [Authorize(Roles = IdentityRoleConstants.Admin)]
    public class IndexModel : PageModel
    {
        public DbSet<SurveyQuestion> Questions;
        public ViewOptions Options;

        public IndexModel(HumanErrorProjectContext context, IOptions<ViewOptions> options)
        {
            Questions = context.Set<SurveyQuestion>();
            Options = options.Value;
        }

        [FromRoute]
        public int Step { get; set; } = 0;


        public Task<IEnumerable<SurveyQuestion>> GetQuestions()
        {
            return Task.FromResult<IEnumerable<SurveyQuestion>>(
                Questions.Skip(Step * Options.StepSize).Take(Options.StepSize));
        }

        public Task<bool> IsNext()
        {
            var count = Questions.Count();
            return Task.FromResult(Step * Options.StepSize < count - Options.StepSize);
        }

        public Task<bool> IsPrevious() => Task.FromResult(Step > 0);
    }
}