using HumanErrorProject.Engine.Utilities.Filter.ClangCriteria;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HumanErrorProject.Engine.Test.UtilitiesTests.FilterTests.ClangCriteriaTests
{
    [TestClass]
    public class ClangCriteriaNotNodeTests : BaseClangCriteriaNodeTests
    {
        protected ClangCriteriaNotNode Node;

        [TestInitialize]
        public void Init()
        {
            Node = new ClangCriteriaNotNode(new MockClangCriteriaNode("A"));
        }

        [TestMethod]
        public void Pass_ShouldFailForA()
        {
            Assert.IsFalse(Node.Pass("A"));
        }

        [TestMethod]
        public void Pass_ShouldPassForB()
        {
            Assert.IsTrue(Node.Pass("B"));
        }
    }
}
