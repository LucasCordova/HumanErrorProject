using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Engine.Analysis.AbstractSyntaxTree;

namespace HumanErrorProject.Engine.Analysis
{
    public class AbstractSyntaxTreeMetricCreator : IAbstractSyntaxTreeMetricCreator
    {
        public AbstractSyntaxTreeMetric Create(AbstractSyntaxTreeNode expected, AbstractSyntaxTreeNode actual)
        {
            var expectedLevels = CreateLevels(ref expected);
            var actualLevels = CreateLevels(ref actual);

            var metric = InitialLevel(ref expectedLevels, ref actualLevels);

            var index = 1;
            while (index < actualLevels.Count)
            {
                CompareLevels(ref metric, index, ref expectedLevels, ref actualLevels);
                index++;
            }

            metric = RemoveExcessLevels(metric, actualLevels.Count, ref expectedLevels);

            return metric;
        }

        private AbstractSyntaxTreeMetric RemoveExcessLevels(AbstractSyntaxTreeMetric metric, int rightSize, ref IList<AbstractSyntaxTreeAnalysisLevel> left)
        {
            for (; rightSize < left.Count; rightSize++)
            {
                metric.Deletions += left[rightSize].Nodes.Count;
            }
            return metric;
        }

        public AbstractSyntaxTreeMetric InitialLevel(ref IList<AbstractSyntaxTreeAnalysisLevel> expectedLevels,
            ref IList<AbstractSyntaxTreeAnalysisLevel> actualLevels)
        {
            var metric = new AbstractSyntaxTreeMetric();

            if (actualLevels.Count == 0)
                return metric;

            if (expectedLevels.Count == 0)
            {
                expectedLevels.Add(new AbstractSyntaxTreeAnalysisLevel()
                {
                    Height = 1,
                });
            }

            foreach (var analysisNodeWrapper in actualLevels[0].Nodes)
            {
                var indexOfNode = IndexOf(expectedLevels[0], analysisNodeWrapper);

                if (indexOfNode == -1)
                {
                    var node = new AbstractSyntaxTreeAnalysisNodeWrapper()
                    {
                        Used = true,
                        Node = analysisNodeWrapper.Node.Copy()
                    };
                    expectedLevels[0].Nodes.Add(node);
                    metric.Insertations++;
                }
                else
                {
                    expectedLevels[0].Nodes[indexOfNode].Used = true;
                }
            }
            RemoveExcessAtLevel(ref metric, 0, ref expectedLevels);
            return metric;
        }

        private void RemoveExcessAtLevel(ref AbstractSyntaxTreeMetric metric, int index,
            ref IList<AbstractSyntaxTreeAnalysisLevel> expected)
        {
            int levelIndex = 0;
            while (levelIndex < expected[index].Nodes.Count)
            {
                if (expected[index].Nodes[levelIndex].Used)
                {
                    levelIndex++;
                }
                else
                {
                    expected[index].Nodes.RemoveAt(levelIndex);
                    metric.Deletions++;
                }
            }
        }

        public int IndexOf(AbstractSyntaxTreeAnalysisLevel level, AbstractSyntaxTreeAnalysisNodeWrapper node)
        {
            for (var index = 0; index < level.Nodes.Count; index++)
            {
                var astAnalysisNode = level.Nodes[index];
                if (astAnalysisNode.Used == false && astAnalysisNode.Node == node.Node)
                {
                    return index;
                }
            }
            return -1;
        }

        private void CompareLevels(ref AbstractSyntaxTreeMetric metric, int index,
            ref IList<AbstractSyntaxTreeAnalysisLevel> expectedLevels,
            ref IList<AbstractSyntaxTreeAnalysisLevel> actualLevels)
        {
            if (expectedLevels.Count == index)
            {
                expectedLevels.Add(new AbstractSyntaxTreeAnalysisLevel()
                {
                    Height = index
                });
            }

            foreach (var analysisNode in actualLevels[index].Nodes)
            {
                var indexOfNode = IndexOf(expectedLevels[index], analysisNode);

                if (indexOfNode == -1)
                {
                    AdjustLevel(ref metric, index, ref expectedLevels, analysisNode);
                }
                else
                {
                    expectedLevels[index].Nodes[indexOfNode].Used = true;
                }
            }
            RemoveExcessAtLevel(ref metric, index, ref expectedLevels);
        }

        private void AdjustLevel(ref AbstractSyntaxTreeMetric metric, int index,
            ref IList<AbstractSyntaxTreeAnalysisLevel> expectedLevel,
            AbstractSyntaxTreeAnalysisNodeWrapper node)
        {
            var bestNodeIndex = FindBestNode(index, ref expectedLevel, node);

            if (bestNodeIndex == -1)
            {
                expectedLevel[index].Nodes.Add(
                    new AbstractSyntaxTreeAnalysisNodeWrapper()
                    {
                        Used = false,
                        Node = node.Node.Copy()
                    });
                metric.Insertations++;
                bestNodeIndex = expectedLevel[index].Nodes.Count - 1;
            }

            metric = RotateNodes(metric, index, bestNodeIndex, ref expectedLevel, node.Node);
            expectedLevel[index].Nodes[bestNodeIndex].Used = true;
        }

        private AbstractSyntaxTreeMetric RotateNodes(AbstractSyntaxTreeMetric metric, int index,
            int nodeIndex, ref IList<AbstractSyntaxTreeAnalysisLevel> level,
            AbstractSyntaxTreeNode node)
        {
            var compareNode = level[index].Nodes[nodeIndex].Node;
            for (var i = 0; i < node.Children.Count; i++)
            {
                var rotatingNode = -1;
                for (var j = i; j < compareNode.Children.Count && rotatingNode == -1; j++)
                {
                    if (compareNode.Children[j] == node.Children[i])
                    {
                        rotatingNode = j;
                    }
                }

                if (i == rotatingNode) continue;

                if (rotatingNode != -1)
                {
                    compareNode.RemoveAt(rotatingNode);
                }

                compareNode.InsertAt(i, node.Children[i]);
                metric.Rotations++;
            }
            return metric;
        }

        private int FindBestNode(int index, ref IList<AbstractSyntaxTreeAnalysisLevel> expected,
            AbstractSyntaxTreeAnalysisNodeWrapper compareAnalysisNode)
        {
            var bestScore = -1;
            var bestMatchIndex = -1;

            for (var i = 0; i < expected[index].Nodes.Count; i++)
            {
                var analysisNode = expected[index].Nodes[i];

                if (!analysisNode.Used && analysisNode.Node.Value == compareAnalysisNode.Node.Value
                    && analysisNode.Node.Height == compareAnalysisNode.Node.Height)
                {
                    var score = CompareChildren(analysisNode.Node, compareAnalysisNode.Node);

                    if (score > bestScore)
                    {
                        bestMatchIndex = i;
                        bestScore = score;
                    }
                }
            }
            return bestMatchIndex;
        }

        public int CompareChildren(AbstractSyntaxTreeNode expected, AbstractSyntaxTreeNode actual)
        {
            int score = 0;

            var leftHashtags = expected.Children.Select(node => node.HashTag).ToList();
            leftHashtags.Sort();

            var rightHashtags = actual.Children.Select(node => node.HashTag).ToList();
            rightHashtags.Sort();

            var leftIndex = 0;
            var rightIndex = 0;
            for (; leftIndex < leftHashtags.Count && rightIndex < rightHashtags.Count; rightIndex++)
            {
                while (leftIndex < leftHashtags.Count && rightHashtags[rightIndex] > leftHashtags[leftIndex])
                {
                    leftIndex++;
                }
                if (leftIndex < leftHashtags.Count && rightHashtags[rightIndex] == leftHashtags[leftIndex])
                {
                    score++;
                    leftIndex++;
                }
            }

            return score;
        }

        public IList<AbstractSyntaxTreeAnalysisLevel> CreateLevels(ref AbstractSyntaxTreeNode expected)
        {
            var visitor = new AbstractSyntaxTreeAnalysisVisitor();
            expected.PreOrder(visitor);
            return visitor.Levels;
        }


        public class AbstractSyntaxTreeAnalysisNodeWrapper
        {
            public AbstractSyntaxTreeNode Node { get; set; }
            public bool Used { get; set; } = false;
        }

        public class AbstractSyntaxTreeAnalysisLevel
        {
            public int Height { get; set; }
            public IList<AbstractSyntaxTreeAnalysisNodeWrapper> Nodes { get; set; } = new List<AbstractSyntaxTreeAnalysisNodeWrapper>();
        }

        public class AbstractSyntaxTreeAnalysisVisitor : IAbstractSyntaxTreeVisitor
        {
            public IList<AbstractSyntaxTreeAnalysisLevel> Levels { get; set; } = new List<AbstractSyntaxTreeAnalysisLevel>();

            public void Visit(AbstractSyntaxTreeNode node)
            {
                while (Levels.Count < node.Height)
                {
                    Levels.Add(new AbstractSyntaxTreeAnalysisLevel
                    {
                        Height = Levels.Count
                    });
                }
                Levels[node.Height - 1].Nodes.Add(
                    new AbstractSyntaxTreeAnalysisNodeWrapper()
                    {
                        Node = node,
                    });
            }
        }
    }
}
