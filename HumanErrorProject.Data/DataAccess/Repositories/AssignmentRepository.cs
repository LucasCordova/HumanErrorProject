using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using HumanErrorProject.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HumanErrorProject.Data.DataAccess.Repositories
{
    public class AssignmentRepository : Repository<Assignment, int>
    {
        public AssignmentRepository(HumanErrorProjectContext context) : base(context)
        {
        }

        public override async Task<Assignment> Get(int id)
        {
            var assignment = await Entities.FindAsync(id);
            if (assignment == null) return null;

            Context.Entry(assignment).Reference(x => x.CourseClass)
                .Load();
            Context.Entry(assignment).Reference(x => x.Solution)
                .Query().Include(x => x.MethodDeclarations).Load();
            Context.Entry(assignment).Reference(x => x.TestProject)
                .Query().Include(x => x.UnitTests).Load();
            Context.Entry(assignment).Collection(x => x.Snapshots)
                .Query()
                .Include(x => x.SnapshotSubmission)
                .Include(x => x.Report).Load();

            assignment.Snapshots.ToList()
                .ForEach(x =>
                {
                    if (x.Report.Type == SnapshotReport.SnapshotReportTypes.Success)
                    {
                        Context.Entry((SnapshotSuccessReport) x.Report)
                            .Collection(y => y.UnitTestResults)
                            .Query().Include(y => y.UnitTest).Load();
                    }
                });


            return assignment;
        }
    }
}
