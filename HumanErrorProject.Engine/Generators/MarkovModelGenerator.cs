using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HumanErrorProject.Data.DataAccess;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Engine.Analysis;
using HumanErrorProject.Engine.Analysis.AbstractSyntaxTree;
using HumanErrorProject.Engine.Options;
using HumanErrorProject.Engine.Utilities;
using HumanErrorProject.Engine.Utilities.Filter;

namespace HumanErrorProject.Engine.Generators
{
    public class MarkovModelGenerator : IMarkovModelGenerator
    {
        protected ILineFilter LineFilter;
        protected IAbstractSyntaxTreeGenerator AbstractSyntaxTreeGenerator;
        protected IAbstractSyntaxTreeClassExtractor AbstractSyntaxTreeClassExtractor;
        protected IAbstractSyntaxTreeMethodExtractor AbstractSyntaxTreeMethodExtractor;
        protected IAbstractSyntaxTreeMetricCreator AbstractSyntaxTreeMetricCreator;
        protected IBagOfWordsMetricCreator BagOfWordsMetricCreator;
        protected IMarkovModelCreator MarkovModelCreator;
        protected IRepository<MarkovModel, int> MarkovModelRepository;

        public MarkovModelGenerator(ILineFilter lineFilter, IAbstractSyntaxTreeGenerator abstractSyntaxTreeGenerator,
            IAbstractSyntaxTreeClassExtractor abstractSyntaxTreeClassExtractor, IAbstractSyntaxTreeMetricCreator abstractSyntaxTreeMetricCreator,
            IBagOfWordsMetricCreator bagOfWordsMetricCreator, IRepository<MarkovModel, int> markovModelRepository,
            IAbstractSyntaxTreeMethodExtractor abstractSyntaxTreeMethodExtractor,
            IMarkovModelCreator markovModelCreator)
        {
            LineFilter = lineFilter;
            AbstractSyntaxTreeGenerator = abstractSyntaxTreeGenerator;
            AbstractSyntaxTreeClassExtractor = abstractSyntaxTreeClassExtractor;
            AbstractSyntaxTreeMetricCreator = abstractSyntaxTreeMetricCreator;
            AbstractSyntaxTreeMethodExtractor = abstractSyntaxTreeMethodExtractor;
            BagOfWordsMetricCreator = bagOfWordsMetricCreator;
            MarkovModelCreator = markovModelCreator;
            MarkovModelRepository = markovModelRepository;
        }

        public async Task Generate(IList<Snapshot> snapshots, MarkovModelOptions options, DirectoryHandler handler, Assignment assignment)
        {
            var markovModel = new MarkovModel()
            {
                Assignment = assignment,
                Finished = false,
            };
            await MarkovModelRepository.Add(markovModel);

            var distanceMatrix = DistanceMatrix(snapshots, options, handler, assignment);

            markovModel.States = MarkovModelCreator.Create(snapshots, distanceMatrix, options.NumberOfStates);
            markovModel.Finished = true;
            await MarkovModelRepository.Update(markovModel);
        }

        public IList<IList<double>> DistanceMatrix(IList<Snapshot> snapshots, MarkovModelOptions options,
            DirectoryHandler handler, Assignment assignment)
        {
            var distances = CreateSquareMatrix(snapshots.Count);

            for (var row = 0; row < snapshots.Count; ++row)
            {
                var left = snapshots[row];
                var leftRoot = CreateOrDefualtAbstractSyntaxTreeNode(left, handler, assignment);

                for (var col = row + 1; col < snapshots.Count; ++col)
                {
                    var distance = 0.0;
                    var right = snapshots[col];
                    distance += CalculateBuildDistance(left, right, options);
                    distance += CalculateTestDistance(left, right, options, assignment);

                    var rightRoot = CreateOrDefualtAbstractSyntaxTreeNode(right, handler, assignment);

                    distance += CalculateMetricDistance(leftRoot, rightRoot, assignment, options);

                    distances[row][col] = distance;
                    distances[col][row] = distance;
                }
            }
            return distances;
        }

        private double CalculateMetricDistance(AbstractSyntaxTreeNode leftRoot, AbstractSyntaxTreeNode rightRoot, Assignment assignment, MarkovModelOptions options)
        {
            if (leftRoot == null && rightRoot == null) return 0.0d;

            var accumlator = 0.0d;
            foreach (var methodDeclarration in assignment.Solution.MethodDeclarations)
            {
                var leftNode = leftRoot == null ? null : GetMethodOrDefault(leftRoot, methodDeclarration);
                var rightNode = rightRoot == null ? null : GetMethodOrDefault(leftRoot, methodDeclarration);
                
                if (leftNode != null && rightNode != null)
                {
                    var abstractSyntaxTreeMetric = AbstractSyntaxTreeMetricCreator.Create(leftNode, rightNode);
                    var bagOfWordsMetric = BagOfWordsMetricCreator.Create(leftNode, rightNode);
                    accumlator += Math.Pow(abstractSyntaxTreeMetric.Rotations * options.AbstractSyntaxTreeWeight, 2);
                    accumlator += Math.Pow(abstractSyntaxTreeMetric.Insertations * options.AbstractSyntaxTreeWeight, 2);
                    accumlator += Math.Pow(abstractSyntaxTreeMetric.Deletions * options.AbstractSyntaxTreeWeight, 2);
                    accumlator += Math.Pow(bagOfWordsMetric.Difference * options.BagOfWordsWeight, 2);
                }
                else if (leftNode != null)
                {
                    var amount = leftNode.NumberOfNodes();
                    accumlator += Math.Pow(amount * options.AbstractSyntaxTreeWeight, 2);
                    accumlator += Math.Pow(amount * options.BagOfWordsWeight, 2);
                }
                else if (rightNode != null)
                {
                    var amount = rightNode.NumberOfNodes();
                    accumlator += Math.Pow(amount * options.AbstractSyntaxTreeWeight, 2);
                    accumlator += Math.Pow(amount * options.BagOfWordsWeight, 2);
                }
            }
            return accumlator;
        }

        private double CalculateTestDistance(Snapshot left, Snapshot right, MarkovModelOptions options, Assignment assignment)
        {
            if (left.Report.Type == SnapshotReport.SnapshotReportTypes.Failure
                && right.Report.Type == SnapshotReport.SnapshotReportTypes.Failure)
            {
                return 0.0d;
            }

            if (left.Report.Type == SnapshotReport.SnapshotReportTypes.Failure)
            {
                var report = (SnapshotSuccessReport) right.Report;

                var sum = report
                    .UnitTestResults
                    .Select(x => x.Passed ? Math.Pow(options.TestWeight, 2.0f) : 0.0d)
                    .Sum();
                return sum;
            }

            if (right.Report.Type == SnapshotReport.SnapshotReportTypes.Failure)
            {
                var report = (SnapshotSuccessReport)left.Report;

                var sum = report
                    .UnitTestResults
                    .Select(x => x.Passed ? Math.Pow(options.TestWeight, 2.0f) : 0.0d)
                    .Sum();
                return sum;
            }

            var leftReport = (SnapshotSuccessReport) left.Report;
            var rightReport = (SnapshotSuccessReport) right.Report;
            var accumulator = 0.0d;
            foreach (var unitTest in assignment.TestProject.UnitTests)
            {
                var leftTest = leftReport.UnitTestResults.Single(x => x.UnitTestId.Equals(unitTest.Id));
                var rightTest = rightReport.UnitTestResults.Single(x => x.UnitTestId.Equals(unitTest.Id));
                var leftValue = leftTest.Passed ? options.TestWeight : 0.0d;
                var rightValue = rightTest.Passed ? options.TestWeight : 0.0d;
                accumulator += Math.Pow(leftValue - rightValue, 2);
            }
            return accumulator;
        }

        public double CalculateBuildDistance(Snapshot left, Snapshot right, MarkovModelOptions options)
        {
            var leftValue = left.Report.Type == SnapshotReport.SnapshotReportTypes.Success ? options.BuildWeight : 0.0d;
            var rightValue = right.Report.Type == SnapshotReport.SnapshotReportTypes.Success ? options.BuildWeight : 0.0d;
            return Math.Pow(leftValue - rightValue, 2.0d);
        }

        public AbstractSyntaxTreeNode CreateOrDefualtAbstractSyntaxTreeNode(Snapshot snapshot, DirectoryHandler handler, Assignment assignment)
        {
            using (var directory = new DirectoryHandler(Path.Combine(handler.Directory, nameof(MarkovModelGenerator))))
            {
                var snapshotPath = EngineFileUtilities.ExtractZip(directory.Directory,
                    "Submission", snapshot.SnapshotSubmission.Files);
                var file = Path.Combine(snapshotPath, assignment.Filename);
                var node = AbstractSyntaxTreeGenerator.CreateOrDefaultFromFile(directory, file);
                if (node != null)
                    return AbstractSyntaxTreeClassExtractor.ExtractOrDefault(node, assignment.Solution.Name);
            }
            return null;
        }

        public IList<IList<double>> CreateSquareMatrix(int length)
        {
            var distances = new List<IList<double>>();
            for (var i = 0; i < length; ++i)
            {
                distances.Add(new List<double>());
                for (var j = 0; j < length; ++j)
                {
                    distances[i].Add(0.0);
                }
            }
            return distances;
        }

        public AbstractSyntaxTreeNode GetMethodOrDefault(AbstractSyntaxTreeNode node, MethodDeclaration method)
        {
            var methodNode = AbstractSyntaxTreeMethodExtractor.ExtractOrDefault(node, method);
            if (methodNode == null) return null;
            var filter = new AbstractSyntaxTreeFilterVisitor(LineFilter);
            methodNode.PostOrder(filter);
            return methodNode;
        }
    }
}
