using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Engine.Options;
using HumanErrorProject.Engine.Utilities;
using HumanErrorProject.Engine.Utilities.Filter;

namespace HumanErrorProject.Engine.Analysis
{
    public class MarkovModelCreator : IMarkovModelCreator
    {
        public IList<MarkovModelState> Create(IList<Snapshot> snapshots, IList<IList<double>> distanceMatrix, int numberOfStates)
        {
            var states = CreateMarkovModelStates(snapshots, distanceMatrix, numberOfStates);

            CalculateTransitions(ref states, snapshots);

            return states;
        }

        private void CalculateTransitions(ref IList<MarkovModelState> states, IList<Snapshot> snapshots)
        {
            foreach (var state in states)
            {
                var numbers = new List<int>();
                foreach (var snapshot in state.Snapshots)
                {
                    numbers.Add(FindNextSnapshot(states, snapshot, snapshots));
                }

                numbers = numbers.Where(n => !n.Equals(-1)).ToList();

                var numberGroup = numbers.GroupBy(
                    n => n,
                    (key, g) => new { Number = key, Amount = g.ToList().Count }
                );

                foreach (var group in numberGroup)
                {
                    state.Transitions.Add(new MarkovModelTransition()
                    {
                        To = group.Number,
                        Probability = (double)group.Amount / numbers.Count
                    });
                }
            }
        }

        private int FindNextSnapshot(IList<MarkovModelState> states, MarkovModelSnapshot current, IList<Snapshot> snapshots)
        {
            var currentSnapshot = snapshots.First(x => x.Id.Equals(current.SnapshotId));
            var choices = states.SelectMany(x =>
            {
                return x.Snapshots.Where(y =>
                {
                    var snapshot = snapshots.First(z => z.Id.Equals(y.SnapshotId));
                    return snapshot.StudentId.Equals(currentSnapshot.StudentId) && 
                           snapshot.SnapshotSubmission.CreatedDateTime > currentSnapshot.SnapshotSubmission.CreatedDateTime;
                }).Select(y =>
                {
                    var snapshot = snapshots.First(z => z.Id.Equals(y.SnapshotId));
                    return new
                    {
                        x.Number,
                        Time = snapshot.SnapshotSubmission.CreatedDateTime
                    };
                });
            }).ToList();
            
            if (choices.Count == 0) return -1;
            return choices.OrderBy(x => x.Time).First().Number;
        }

        public IList<MarkovModelState> CreateMarkovModelStates(IList<Snapshot> snapshots, IList<IList<double>> distanceMatrix, int numberOfStates)
        {
            var states = new List<MarkovModelState>();

            var number = 1;
            states.AddRange(snapshots.Select(x => new MarkovModelState()
            {
                Number = number,
                Snapshots = new List<MarkovModelSnapshot>()
                {
                    new MarkovModelSnapshot()
                    {
                        SnapshotId = x.Id
                    }
                }
            }));

            while (states.Count > numberOfStates)
            {
                var minRow = 0;
                var minCol = 1;
                var minValue = distanceMatrix[minRow][minCol];

                for (var row = 0; row < states.Count - 1; ++row)
                {
                    for (var col = row + 1; col < states.Count; col++)
                    {
                        if (distanceMatrix[row][col] < minValue)
                        {
                            minValue = distanceMatrix[row][col];
                            minRow = row;
                            minCol = col;
                        }
                    }
                }

                var tempStates = new List<MarkovModelState>();
                var tempdistanceMatrix = new List<IList<double>>();

                for (var row = 0; row < states.Count; row++)
                {
                    if (row == minRow)
                    {
                        var temp = new List<MarkovModelSnapshot>();
                        temp.AddRange(states[minRow].Snapshots);
                        temp.AddRange(states[minCol].Snapshots);
                        tempStates.Add(new MarkovModelState()
                        {
                            Number = row + 1,
                            Snapshots = temp
                        });
                    }
                    else if (row != minCol)
                    {
                        tempStates.Add(
                            new MarkovModelState()
                            {
                                Number = row + 1,
                                Snapshots = states[row].Snapshots
                            }
                        );
                    }
                }

                for (var row = 0; row < minRow; row++)
                {
                    var tempDistance = new List<double>();
                    for (var col = 0; col < minRow; col++)
                        tempDistance.Add(distanceMatrix[row][col]);

                    tempDistance.Add(distanceMatrix[row][minRow] < distanceMatrix[row][minCol]
                        ? distanceMatrix[row][minRow]
                        : distanceMatrix[row][minCol]);

                    for (var col = minRow + 1; col < minCol; col++)
                        tempDistance.Add(distanceMatrix[row][col]);

                    for (var col = minCol + 1; col < states.Count; col++)
                        tempDistance.Add(distanceMatrix[row][col]);

                    tempdistanceMatrix.Add(tempDistance);
                }

                var mintempDistance = new List<double>();
                for (var col = 0; col < minCol; col++)
                    mintempDistance.Add(distanceMatrix[minRow][col] < distanceMatrix[minCol][col]
                        ? distanceMatrix[minRow][col]
                        : distanceMatrix[minCol][col]);
                for (var col = minCol + 1; col < states.Count; col++)
                    mintempDistance.Add(distanceMatrix[minRow][col] < distanceMatrix[minCol][col]
                        ? distanceMatrix[minRow][col]
                        : distanceMatrix[minCol][col]);
                tempdistanceMatrix.Add(mintempDistance);

                for (var row = minRow + 1; row < minCol; row++)
                {
                    var tempDistance = new List<double>();
                    for (var col = 0; col < minRow; col++)
                        tempDistance.Add(distanceMatrix[row][col]);

                    tempDistance.Add(distanceMatrix[row][minRow] < distanceMatrix[row][minCol]
                        ? distanceMatrix[row][minRow]
                        : distanceMatrix[row][minCol]);

                    for (var col = minRow + 1; col < minCol; col++)
                        tempDistance.Add(distanceMatrix[row][col]);

                    for (var col = minCol + 1; col < states.Count; col++)
                        tempDistance.Add(distanceMatrix[row][col]);

                    tempdistanceMatrix.Add(tempDistance);
                }

                for (var row = minCol + 1; row < states.Count; row++)
                {
                    var tempDistance = new List<double>();
                    for (var col = 0; col < minRow; col++)
                        tempDistance.Add(distanceMatrix[row][col]);

                    tempDistance.Add(distanceMatrix[row][minRow] < distanceMatrix[row][minCol]
                        ? distanceMatrix[row][minRow]
                        : distanceMatrix[row][minCol]);

                    for (var col = minRow + 1; col < minCol; col++)
                        tempDistance.Add(distanceMatrix[row][col]);

                    for (var col = minCol + 1; col < states.Count; col++)
                        tempDistance.Add(distanceMatrix[row][col]);

                    tempdistanceMatrix.Add(tempDistance);
                }

                states = tempStates;
                distanceMatrix = tempdistanceMatrix;
            }
            return states;
        }
    }
}
