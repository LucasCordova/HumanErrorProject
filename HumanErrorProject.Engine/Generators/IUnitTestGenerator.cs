using System.Collections.Generic;
using System.Threading.Tasks;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Engine.Data;
using HumanErrorProject.Engine.Utilities;

namespace HumanErrorProject.Engine.Generators
{
    public interface IUnitTestGenerator
    {
        Task<ICollection<UnitTestResult>> GenerateResults(SubmissionData data, string snapshot, Assignment assignment,
            ICollection<SnapshotMethod> snapshotMethods);

        Task<ICollection<UnitTest>> GenerateResults(PreAssignment assignment, DirectoryHandler handler, string root);
    }
}
