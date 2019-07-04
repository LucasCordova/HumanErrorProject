using System.Threading.Tasks;
using HumanErrorProject.Data.DataAccess;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Ui.Constants;
using HumanErrorProject.Ui.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HumanErrorProject.Ui.Pages.Assignments
{
    [Authorize(Roles = IdentityRoleConstants.Admin)]
    public class DeleteModel : PageModel
    {
        public DeleteHelper DeleteHelper;
        public HumanErrorProjectContext Context;
        public DbSet<Assignment> Assignments;
    }
}