using System.Collections.Generic;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Engine.Options;
using HumanErrorProject.Engine.Utilities;

namespace HumanErrorProject.Engine.Analysis
{
    public interface IMarkovModelCreator
    {
        IList<MarkovModelState> Create(IList<Snapshot> snapshots, IList<IList<double>> distanceMatrix, int numberOfStates);
    }
}
