using System;
using HumanErrorProject.Engine.Analysis;
using HumanErrorProject.Engine.Analysis.AbstractSyntaxTree;
using HumanErrorProject.Engine.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HumanErrorProject.Engine.Test.AnalysisTests
{
    [TestClass]
    public class BagOfWordsMetricCreatorTests
    {
        public BagOfWordsMetricCreator Creator;
        public MockBagOfWordsMetricCreatorSplitter Splitter;

        [TestInitialize]
        public void Init()
        {
            Splitter = new MockBagOfWordsMetricCreatorSplitter();
            Creator = new BagOfWordsMetricCreator(Splitter);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CalculateDifference_ShouldThrowExceptionForDifferentLength()
        {
            Creator.CalculateDifference(
                new[] { 1, 2, },
                new[] { 1, 2, 3 });
        }

        [TestMethod]
        public void CalculateDifference_ZeroVectorsShouldBeZeroDifference()
        {
            var value = Creator.CalculateDifference(
                new[] { 0 },
                new[] { 0 });

            Assert.AreEqual(0.0, value);
        }

        [TestMethod]
        public void CalculateDifference_SameVectorShouldBeZeroDifference()
        {
            var value = Creator.CalculateDifference(
                new[] { 0, 1, 2 },
                new[] { 0, 1, 2 }
            );

            Assert.AreEqual(0.0, value);
        }

        [TestMethod]
        public void CalculateDifference_PythagoreanTriple()
        {
            var value = Creator.CalculateDifference(
                new[] { 0, 4 },
                new[] { 3, 0 }
            );

            Assert.AreEqual(5.0, value);
        }

        [TestMethod]
        public void Create_CheckDistanceOfDifferentAbstractSyntaxTreeNodes()
        {
            var left = new AbstractSyntaxTreeNode("one");
            left.Append(new AbstractSyntaxTreeNode("two"));
            left.Append(new AbstractSyntaxTreeNode("three"));
            left.Append(new AbstractSyntaxTreeNode("one two three"));

            var right = new AbstractSyntaxTreeNode("four");
            right.Append(new AbstractSyntaxTreeNode("one"));
            right.Append(new AbstractSyntaxTreeNode("ones"));
            right.Append(new AbstractSyntaxTreeNode("four ones"));

            var metric = Creator.Create(left, right);

            Assert.AreEqual(Math.Pow(17, 0.5), metric.Difference);
        }


        [TestMethod]
        public void Create_CheckDistanceOfDifferentAbstractSyntaxTreeNodesWithOperator()
        {
            var left = new AbstractSyntaxTreeNode("one");
            left.Append(new AbstractSyntaxTreeNode("two"));
            left.Append(new AbstractSyntaxTreeNode("three"));
            left.Append(new AbstractSyntaxTreeNode("one (two) + + + + + + + three"));

            var right = new AbstractSyntaxTreeNode("four");
            right.Append(new AbstractSyntaxTreeNode("one"));
            right.Append(new AbstractSyntaxTreeNode("ones"));
            right.Append(new AbstractSyntaxTreeNode("four ones"));

            var metric = Creator.Create(left, right);

            Assert.AreEqual(Math.Pow(64, 0.5), metric.Difference);
        }

        public class MockBagOfWordsMetricCreatorSplitter : ILineSplitter
        {
            public string[] Split(string line)
            {
                return line.Split(new[] { " " },
                    StringSplitOptions.RemoveEmptyEntries);
            }
        }
    }
}
