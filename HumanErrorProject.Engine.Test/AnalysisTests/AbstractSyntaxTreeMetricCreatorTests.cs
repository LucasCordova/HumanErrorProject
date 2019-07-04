using HumanErrorProject.Data.Models;
using HumanErrorProject.Engine.Analysis;
using HumanErrorProject.Engine.Analysis.AbstractSyntaxTree;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HumanErrorProject.Engine.Test.AnalysisTests
{
    [TestClass]
    public class AbstractSyntaxTreeMetricCreatorTests
    {
        protected AbstractSyntaxTreeMetricCreator Creator;

        [TestInitialize]
        public void Init()
        {
            Creator = new AbstractSyntaxTreeMetricCreator();
        }

        [TestMethod]
        public void Create_IdenticalTerminalNodesShouldBeNoDifference()
        {
            var left = new AbstractSyntaxTreeNode("A");
            var right = new AbstractSyntaxTreeNode("A");

            var metric = Creator.Create(left, right);

            Assert.AreEqual(new AbstractSyntaxTreeMetric(), metric);
        }

        [TestMethod]
        public void Create_IdenticalTreeShouldBeNoDifference()
        {
            var left = new AbstractSyntaxTreeNode("A");
            var terminal = new AbstractSyntaxTreeNode("B");
            var branch = new AbstractSyntaxTreeNode("C");
            branch.Append(terminal);
            left.Append(branch);
            left.Append(terminal.CopyDeep());
            var right = left.CopyDeep();

            var metric = Creator.Create(left, right);

            Assert.AreEqual(new AbstractSyntaxTreeMetric(), metric);
        }

        [TestMethod]
        public void Create_OneInsertationOfNode()
        {
            var right = new AbstractSyntaxTreeNode("A");
            right.Append(new AbstractSyntaxTreeNode("B"));
            var left = right.CopyDeep();
            right.Append(new AbstractSyntaxTreeNode("C"));
            var metric = Creator.Create(left, right);

            Assert.AreEqual(new AbstractSyntaxTreeMetric()
            {
                Insertations = 1,
                Rotations = 1,
            }, metric);
        }

        [TestMethod]
        public void Create_OneDeletionsOfNode()
        {
            var left = new AbstractSyntaxTreeNode("A");
            left.Append(new AbstractSyntaxTreeNode("B"));
            var right = left.CopyDeep();
            left.Append(new AbstractSyntaxTreeNode("C"));
            var metric = Creator.Create(left, right);

            Assert.AreEqual(new AbstractSyntaxTreeMetric()
            {
                Deletions = 1,
            }, metric);
        }

        [TestMethod]
        public void Create_BranchHeavyRightSide()
        {
            var left = new AbstractSyntaxTreeNode("A");
            var right = new AbstractSyntaxTreeNode("A");
            right.Append(new AbstractSyntaxTreeNode("B"));

            var metric = Creator.Create(left, right);

            Assert.AreEqual(
                new AbstractSyntaxTreeMetric()
                {
                    Insertations = 2,
                    Deletions = 1,
                    Rotations = 1
                },
                metric
            );
        }

        [TestMethod]
        public void Create_BranchHasTerminalHeavyRightSide()
        {
            var left = new AbstractSyntaxTreeNode("B");
            var right = new AbstractSyntaxTreeNode("A");
            right.Append(new AbstractSyntaxTreeNode("B"));

            var metric = Creator.Create(left, right);

            Assert.AreEqual(
                new AbstractSyntaxTreeMetric()
                {
                    Insertations = 1,
                    Deletions = 0,
                    Rotations = 1
                },
                metric
            );
        }

        [TestMethod]
        public void Create_BranchHeavyLeftSideNode()
        {
            var left = new AbstractSyntaxTreeNode("A");
            left.Append(new AbstractSyntaxTreeNode("B"));
            var right = new AbstractSyntaxTreeNode("A");

            var metric = Creator.Create(left, right);

            Assert.AreEqual(
                new AbstractSyntaxTreeMetric()
                {
                    Insertations = 1,
                    Deletions = 2,
                    Rotations = 0
                },
                metric
            );
        }

        [TestMethod]
        public void Create_BranchHasTerminalHeavyLeftSideNode()
        {
            var left = new AbstractSyntaxTreeNode("B");
            left.Append(new AbstractSyntaxTreeNode("A"));
            var right = new AbstractSyntaxTreeNode("A");

            var metric = Creator.Create(left, right);

            Assert.AreEqual(
                new AbstractSyntaxTreeMetric()
                {
                    Insertations = 0,
                    Deletions = 1,
                    Rotations = 0
                },
                metric
            );
        }

        [TestMethod]
        public void Create_RotateWithinNode()
        {
            var left = new AbstractSyntaxTreeNode("A");
            left.Append(new AbstractSyntaxTreeNode("B"));
            left.Append(new AbstractSyntaxTreeNode("C"));

            var right = new AbstractSyntaxTreeNode("A");
            right.Append(new AbstractSyntaxTreeNode("C"));
            right.Append(new AbstractSyntaxTreeNode("B"));

            var metric = Creator.Create(left, right);

            Assert.AreEqual(
                new AbstractSyntaxTreeMetric()
                {
                    Insertations = 0,
                    Deletions = 0,
                    Rotations = 1
                },
                metric
            );
        }

        [TestMethod]
        public void Create_RotationOfSeperateTree()
        {
            var left = new AbstractSyntaxTreeNode("A");
            var branchLeft = new AbstractSyntaxTreeNode("B");
            var terminalLeft = new AbstractSyntaxTreeNode("C");
            branchLeft.Append(terminalLeft);
            var branchRight = new AbstractSyntaxTreeNode("D");
            var terminalRight = new AbstractSyntaxTreeNode("E");
            branchRight.Append(terminalRight);
            left.Append(branchLeft);
            left.Append(branchRight);

            var right = new AbstractSyntaxTreeNode("A");
            right.Append(branchRight.CopyDeep());
            right.Append(branchLeft.CopyDeep());

            var metric = Creator.Create(left, right);

            Assert.AreEqual(
                new AbstractSyntaxTreeMetric()
                {
                    Insertations = 0,
                    Deletions = 0,
                    Rotations = 1
                },
                metric
            );
        }
    }
}
