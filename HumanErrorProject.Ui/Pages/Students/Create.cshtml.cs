using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanErrorProject.Data.DataAccess;
using HumanErrorProject.Data.DataAccess.Repositories;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Ui.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HumanErrorProject.Ui.Pages.Students
{
    [Authorize(Roles = IdentityRoleConstants.Admin)]
    public class CreateModel : PageModel
    {
        public HumanErrorProjectContext Context;
        public DbSet<Student> Students;

        public CreateModel(HumanErrorProjectContext context)
        {
            Context = context;
            Students = context.Set<Student>();
        }

        [BindProperty]
        public Student Student { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Students.Add(Student);
            await Context.SaveChangesAsync();

            return RedirectToPage("/Students/Index");
        }
    }
}