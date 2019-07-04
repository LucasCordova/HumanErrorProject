using HumanErrorProject.Engine.Analysis.AbstractSyntaxTree;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HumanErrorProject.Engine.Test.AnalysisTests.AbstractSnytaxTreeTests
{
    [TestClass]
    public class AbstractSyntaxTreeSearchVisitorTests : BaseAbstractSnytaxTreeVisitorTests
    {
        [TestInitialize]
        public void Init()
        {
            BaseInit();
        }

        [TestMethod]
        public void PreOrder_ShouldFindAllThatIsExpected()
        {
            foreach (var looking in PreOrderExpected)
            {
                var visitor = new MockAbstractSyntaxTreeSearchVisitor(looking);
                Root.PreOrder(visitor);
                Assert.IsTrue(visitor.Found);
            }
        }

        
        [TestMethod]
        public void PostOrder_ShouldFindAllThatIsExpected()
        {
            foreach (var looking in PostOrderExpected)
            {
                var visitor = new MockAbstractSyntaxTreeSearchVisitor(looking);
                Root.PostOrder(visitor);
                Assert.IsTrue(visitor.Found);
            }
        }

        [TestMethod]
        public void BreadthFirst_ShouldFindAllThatIsExpected()
        {
            foreach (var looking in BreadthFirstExpected)
            {
                var visitor = new MockAbstractSyntaxTreeSearchVisitor(looking);
                Root.BreadthFirst(visitor);
                Assert.IsTrue(visitor.Found);
            }
        }

        [TestMethod]
        public void PreOrder_ShouldNotFind()
        {
            var visitor = new MockAbstractSyntaxTreeSearchVisitor("NOT");
            Root.PreOrder(visitor);
            Assert.IsFalse(visitor.Found);
        }
        
        [TestMethod]
        public void PostOrder_ShouldNotFind()
        {
            var visitor = new MockAbstractSyntaxTreeSearchVisitor("NOT");
            Root.PostOrder(visitor);
            Assert.IsFalse(visitor.Found);
        }
        
        [TestMethod]
        public void BreadthFirst_ShouldNotFind()
        {
            var visitor = new MockAbstractSyntaxTreeSearchVisitor("NOT");
            Root.BreadthFirst(visitor);
            Assert.IsFalse(visitor.Found);
        }


    }
}
