using System;
using HumanErrorProject.Engine.Analysis.AbstractSyntaxTree;
using HumanErrorProject.Engine.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HumanErrorProject.Engine.Test.AnalysisTests.AbstractSnytaxTreeTests
{
    [TestClass]
    public class AbstractSyntaxTreeWordSearchVisitorTests
    {
        protected ILineSplitter Splitter;

        [TestInitialize]
        public void Init()
        {
            Splitter = new MockLineSplitter();
        }


        [TestMethod]
        public void Search_ShouldFindAndNotBeNull()
        {
            var root = new AbstractSyntaxTreeNode("root");
            root.Append(new AbstractSyntaxTreeNode("1 one"));
            var visitor = new AbstractSyntaxTreeWordSearchVisitor(Splitter, "one");
            root.PreOrder(visitor);
            Assert.IsNotNull(visitor.Node);
        }

        [TestMethod]
        public void Search_ShouldFindTheNodeWithOne()
        {
            var root = new AbstractSyntaxTreeNode("root");
            root.Append(new AbstractSyntaxTreeNode("1 one"));
            var visitor = new AbstractSyntaxTreeWordSearchVisitor(Splitter, "one");
            root.PreOrder(visitor);

            Assert.IsTrue(visitor.Node.Value.Contains("1"));
        }

        [TestMethod]
        public void Search_ShouldNotFindTwo()
        {
            var root = new AbstractSyntaxTreeNode("root");
            root.Append(new AbstractSyntaxTreeNode("1 one"));
            var visitor = new AbstractSyntaxTreeWordSearchVisitor(Splitter, "two");
            root.PreOrder(visitor);
            Assert.IsNull(visitor.Node);
        }


        public class MockLineSplitter : ILineSplitter
        {
            public string[] Split(string line)
            {
                return line.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            }
        }
    }
}
