using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HumanErrorProject.Engine.Analysis.AbstractSyntaxTree;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HumanErrorProject.Engine.Test.AnalysisTests.AbstractSnytaxTreeTests
{
    [TestClass]
    public class AbstractSyntaxTreeVisitorTests : BaseAbstractSnytaxTreeVisitorTests
    {
        protected MockAbstractSyntaxTreeVisitor Visitor;

        [TestInitialize]
        public void Init()
        {
            BaseInit();
            Visitor = new MockAbstractSyntaxTreeVisitor();
        }

        [TestMethod]
        public void PreOrder_ShouldMatchExpected()
        {
            Root.PreOrder(Visitor);

            Assert.AreEqual(PreOrderExpected.Join(), Visitor.Actual.Join());
        }

        [TestMethod]
        public void PostOrder_ShouldMatchExpected()
        {
            Root.PostOrder(Visitor);

            Assert.AreEqual(PostOrderExpected.Join(), Visitor.Actual.Join());
        }

        [TestMethod]
        public void BreadthFirst_ShouldMatchExpected()
        {
            Root.BreadthFirst(Visitor);

            Assert.AreEqual(BreadthFirstExpected.Join(), Visitor.Actual.Join());
        }
    }
}
