using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanErrorProject.Data.DataAccess;
using HumanErrorProject.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HumanErrorProject.Ui.Utilities
{
    public class DeleteHelper
    {
        protected HumanErrorProjectContext Context;

        public DeleteHelper(HumanErrorProjectContext context)
        {
            Context = context;
        }

        public async Task Delete(MarkovModel markovModel)
        {
            Context.Entry(markovModel).Collection(x => x.States).Query()
                .Include(x => x.Snapshots)
                .Include(x => x.Transitions).Load();

            foreach (var state in markovModel.States)
            {
                foreach (var transition in state.Transitions)
                {
                    Context.Remove(transition);
                }

                foreach (var snapshot in state.Snapshots)
                {
                    Context.Remove(snapshot);
                }

                Context.Remove(state);
            }

            Context.Remove(markovModel);
            await Context.SaveChangesAsync();
        }

        public async Task Delete(PreAssignment preAssignment)
        {
            var testProjectId = preAssignment.TestProjectId;
            var solutionId = preAssignment.AssignmentSolutionId;
            var reportId = preAssignment.PreAssignmentReportId;

            Context.PreAssignments.Remove(preAssignment);
            await Context.SaveChangesAsync();

            await Delete(await Context.TestProjects.FindAsync(testProjectId));
            await Delete(await Context.AssignmentSolutions.FindAsync(solutionId));
            await Delete(await Context.PreAssignmentReports.FindAsync(reportId));
        }

        public async Task Delete(TestProject testProject)
        {
            foreach (var test in testProject.UnitTests)
            {
                await Delete(test);
            }
            Context.TestProjects.Remove(testProject);
            await Context.SaveChangesAsync();
        }

        public async Task Delete(UnitTest test)
        {
            Context.UnitTests.Remove(test);
            await Context.SaveChangesAsync();
        }

        public async Task Delete(AssignmentSolution assignmentSolution)
        {
            while (assignmentSolution.MethodDeclarations.Count > 0)
            {
                await Delete(assignmentSolution.MethodDeclarations.First());
            }
            Context.AssignmentSolutions.Remove(assignmentSolution);
            await Context.SaveChangesAsync();
        }

        public async Task Delete(MethodDeclaration method)
        {
            Context.MethodDeclarations.Remove(method);
            await Context.SaveChangesAsync();
        }

        public async Task Delete(PreAssignmentReport preAssignmentReport)
        {
            Context.PreAssignmentReports.Remove(preAssignmentReport);
            await Context.SaveChangesAsync();
        }
    }
}
