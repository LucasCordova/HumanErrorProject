using System.Collections.Generic;
using HumanErrorProject.Engine.Analysis.AbstractSyntaxTree;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HumanErrorProject.Engine.Test.AnalysisTests.AbstractSnytaxTreeTests
{
    public class BaseAbstractSnytaxTreeVisitorTests
    {
        protected AbstractSyntaxTreeNode Root;
        protected string[] PreOrderExpected;
        protected string[] PostOrderExpected;
        protected string[] BreadthFirstExpected;

        protected void BaseInit()
        {
            Root = new AbstractSyntaxTreeNode("1");
            var leftRootChild = new AbstractSyntaxTreeNode("2");
            var rightRootChild = new AbstractSyntaxTreeNode("3");
            leftRootChild.Append(new AbstractSyntaxTreeNode("4"));
            leftRootChild.Append(new AbstractSyntaxTreeNode("5"));
            rightRootChild.Append(new AbstractSyntaxTreeNode("6"));
            Root.Append(leftRootChild);
            Root.Append(rightRootChild);

            PreOrderExpected = new[]
            {
                "1", "2", "4", "5", "3", "6"
            };
            PostOrderExpected = new[]
            {
                "4", "5", "2", "6", "3", "1"
            };
            BreadthFirstExpected = new[]
            {
                "1", "2", "3", "4", "5", "6"
            };
        }

        protected class MockAbstractSyntaxTreeVisitor : IAbstractSyntaxTreeVisitor
        {
            public IList<string> Actual { get; set; } = new List<string>();
            public void Visit(AbstractSyntaxTreeNode node)
            {
                Actual.Add(node.Value);
            }
        }

        public class MockAbstractSyntaxTreeSearchVisitor : IAbstractSyntaxTreeSearchVisitor
        {
            public MockAbstractSyntaxTreeSearchVisitor(string looking)
            {
                Found = false;
                Looking = looking;
            }

            public bool Found { get; private set; }
            public string Looking { get; }
            public void Visit(AbstractSyntaxTreeNode node)
            {
                if (node.Value.Equals(Looking))
                {
                    Found = true;
                }
            }
        }
    }
}
