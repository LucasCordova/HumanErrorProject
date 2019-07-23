using System.Collections.Generic;
using System.Threading.Tasks;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Engine.Options;
using HumanErrorProject.Engine.Utilities;

namespace HumanErrorProject.Engine.Generators
{
    public interface IMarkovModelGenerator
    {
        Task Generate(IList<Snapshot> snapshots, MarkovModelOptions options, DirectoryHandler handler, Assignment assignment);
    }
}
