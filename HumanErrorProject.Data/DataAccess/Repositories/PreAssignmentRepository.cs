using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HumanErrorProject.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HumanErrorProject.Data.DataAccess.Repositories
{
    public class PreAssignmentRepository : Repository<PreAssignment, int>
    {
        public PreAssignmentRepository(HumanErrorProjectContext context) : base(context)
        {
        }

        public override async Task<PreAssignment> Get(int id)
        {
            var assignment = await Entities.FindAsync(id);
            if (assignment == null) return null;

            Context.Entry(assignment).Reference(x => x.Solution).Query()
                .Include(x => x.MethodDeclarations).Load();
            Context.Entry(assignment).Reference(x => x.CourseClass).Load();
            Context.Entry(assignment).Reference(x => x.TestProject).Query()
                .Include(x => x.UnitTests).Load();

            switch (assignment.PreAssignmentReport.Type)
            {
                case PreAssignmentReport.PreAssignmentReportTypes.FailTestsFailure:
                    Context.Entry((PreAssignmentFailTestsFailureReport)assignment.PreAssignmentReport)
                        .Collection(x => x.FailUnitTests).Load();
                    break;
                case PreAssignmentReport.PreAssignmentReportTypes.MissingMethodsFailure:
                    Context.Entry((PreAssignmentMissingMethodsFailureReport)assignment.PreAssignmentReport)
                        .Collection(x => x.MissingMethodDeclarations).Load();
                    break;
            }
            return assignment;
        }

    }
}
