using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Hangfire;
using HumanErrorProject.Data.DataAccess;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Ui.Constants;
using HumanErrorProject.Ui.Services;
using HumanErrorProject.Ui.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HumanErrorProject.Ui.Pages.Assignments
{
    [Authorize(Roles = IdentityRoleConstants.Admin)]
    public class CreateModel : PageModel
    {
        public HumanErrorProjectContext Context;
        public DbSet<CourseClass> CourseClasses;
        public IEngineService EngineService;

        public CreateModel(HumanErrorProjectContext context, IEngineService engineService)
        {
            Context = context;
            CourseClasses = context.Set<CourseClass>();
            EngineService = engineService;
        }

        [FromRoute]
        public int Id { get; set; }

        [BindProperty] public PreAssignment PreAssignment { get; set; } = new PreAssignment();

        [BindProperty,Required,DisplayName("Solution Files"),
         Annotations.FileExtensions(fileExtensions: "zip", ErrorMessage = "The file must be a ZIP.")]
        public IFormFile SolutionFiles { get; set; }

        [BindProperty,Required,DisplayName("Test Project Files"),
         Annotations.FileExtensions(fileExtensions: "zip", ErrorMessage = "The file must be a ZIP.")]
        public IFormFile TestProjectFiles { get; set; }

        public MethodDeclaration MethodDeclaration { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var course = await CourseClasses.FindAsync(Id);

            if (course == null)
                return NotFound();

            ViewData["Types"] = new SelectList(MethodDeclarationConstants.AstTypes);
            return Page();
        }

        public IActionResult OnPostAddMethod([FromForm] MethodDeclaration methodDeclaration)
        {
            ModelState.Clear();

            if (string.IsNullOrEmpty(methodDeclaration.AstType))
            {
                ModelState.AddModelError("methodDeclaration.AstType", "Ast Type is required.");
            }

            if (string.IsNullOrEmpty(methodDeclaration.PreprocessorDirective))
            {
                ModelState.AddModelError("MethodDeclaration.PreprocessorDirective", "Preprocessor Directive is required.");
            }

            if (string.IsNullOrEmpty(methodDeclaration.AstMethodRegexExpression))
            {
                ModelState.AddModelError("MethodDeclaration.AstMethodRegexExpression", "Ast Method is required.");
            }

            if (string.IsNullOrEmpty(methodDeclaration.AstMethodParameterRegexExpression))
            {
                ModelState.AddModelError("MethodDeclaration.AstMethodParameterRegexExpression", "Ast Method Parameter is required.");
            }

            try
            {
                new Regex(methodDeclaration.AstMethodRegexExpression);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("MethodDeclaration.AstMethodRegexExpression", $"Regex Exception: {e.Message}");
            }
            
            try
            {
                new Regex(methodDeclaration.AstMethodParameterRegexExpression);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("MethodDeclaration.AstMethodParameterRegexExpression", $"Regex Exception: {e.Message}");
            }

            if (!ModelState.IsValid)
            {
                MethodDeclaration = new MethodDeclaration()
                {
                    AstType = methodDeclaration.AstType,
                    PreprocessorDirective = methodDeclaration.PreprocessorDirective,
                    AstMethodRegexExpression = methodDeclaration.AstMethodRegexExpression,
                    AstMethodParameterRegexExpression = methodDeclaration.AstMethodParameterRegexExpression,
                };
                ViewData["Types"] = new SelectList(MethodDeclarationConstants.AstTypes);
                return Page();
            }

            PreAssignment.Solution.MethodDeclarations.Add(new MethodDeclaration()
            {
                AstType = methodDeclaration.AstType,
                PreprocessorDirective = methodDeclaration.PreprocessorDirective,
                AstMethodRegexExpression = methodDeclaration.AstMethodRegexExpression,
                AstMethodParameterRegexExpression = methodDeclaration.AstMethodParameterRegexExpression
            });

            ViewData["Types"] = new SelectList(MethodDeclarationConstants.AstTypes);
            return Page();
        }

        public IActionResult OnPostRemoveMethod(int index)
        {
            if (index < 0 || index >= PreAssignment.Solution.MethodDeclarations.Count)
                return BadRequest();

            PreAssignment.Solution.MethodDeclarations.RemoveAt(index);
            ModelState.Clear();
            ViewData["Types"] = new SelectList(MethodDeclarationConstants.AstTypes);
            return Page();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var course = await CourseClasses.FindAsync(Id);

            if (course == null)
                return NotFound();

            Context.Entry(course).Collection(x => x.PreAssignments).Load();

            PreAssignment.PreAssignmentReport = new PreAssignmentPendingReport();

            var testProject = FileHelper.ProcessFormFile(TestProjectFiles, ModelState);
            var solution = FileHelper.ProcessFormFile(SolutionFiles, ModelState);

            if (!ModelState.IsValid)
                return Page();

            PreAssignment.TestProject.Files = testProject;
            PreAssignment.Solution.Files = solution;

            course.PreAssignments.Add(PreAssignment);
            CourseClasses.Update(course);
            await Context.SaveChangesAsync();

            var id = PreAssignment.Id;
            EngineService.RunPreAssignment(id);

            return RedirectToPage("/PendingAssignments/Index", new {Id});
        }

    }
}