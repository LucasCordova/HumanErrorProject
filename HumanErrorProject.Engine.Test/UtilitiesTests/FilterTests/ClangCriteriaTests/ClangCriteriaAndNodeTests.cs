using System.Collections.Generic;
using HumanErrorProject.Engine.Utilities.Filter.ClangCriteria;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HumanErrorProject.Engine.Test.UtilitiesTests.FilterTests.ClangCriteriaTests
{
    [TestClass]
    public class ClangCriteriaAndNodeTests : BaseClangCriteriaNodeTests
    {
        protected ClangCriteriaAndNode Node;

        [TestInitialize]
        public void Init()
        {
            Node = new ClangCriteriaAndNode(new List<ClangCriteriaNode>()
            {
                new MockClangCriteriaNode("A"),
                new MockClangCriteriaNode("B"),
                new MockClangCriteriaNode("C")
            });
        }

        [TestMethod]
        public void Pass_ShouldPassWhenAllCharactersInvolve()
        {
            Assert.IsTrue(Node.Pass("ABC"));
        }

        
        [TestMethod]
        public void Pass_ShouldPassWhenAllCharactersInvolveRandomOrder()
        {
            Assert.IsTrue(Node.Pass("CAB"));
        }

        [TestMethod]
        public void Pass_ShouldFailWhenOnlyOneA()
        {
            Assert.IsFalse(Node.Pass("A"));
        }
        
        [TestMethod]
        public void Pass_ShouldFailWhenOnlyOneAB()
        {
            Assert.IsFalse(Node.Pass("AB"));
        }
        
        [TestMethod]
        public void Pass_ShouldFailWhenOnlyOneAC()
        {
            Assert.IsFalse(Node.Pass("AC"));
        }

    }
}
