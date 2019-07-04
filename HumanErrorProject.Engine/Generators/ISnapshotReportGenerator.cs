using System.Threading.Tasks;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Engine.Analysis.AbstractSyntaxTree;
using HumanErrorProject.Engine.Data;

namespace HumanErrorProject.Engine.Generators
{
    public interface ISnapshotReportGenerator
    {
        Task<SnapshotReport> Generate(SubmissionData data, string snapshot, Assignment assignment, AbstractSyntaxTreeNode solution);
    }
}
