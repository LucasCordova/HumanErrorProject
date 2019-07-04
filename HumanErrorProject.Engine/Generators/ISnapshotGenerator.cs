using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Engine.Data;

namespace HumanErrorProject.Engine.Generators
{
    public interface ISnapshotGenerator
    {
        Task<IList<Snapshot>> Generate(SubmissionData data, Assignment assignment);
    }
}
