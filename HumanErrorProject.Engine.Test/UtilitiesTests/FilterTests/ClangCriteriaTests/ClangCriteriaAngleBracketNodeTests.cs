using HumanErrorProject.Engine.Utilities.Filter.ClangCriteria;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HumanErrorProject.Engine.Test.UtilitiesTests.FilterTests.ClangCriteriaTests
{
    [TestClass]
    public class ClangCriteriaAngleBracketNodeTests
    {
        protected ClangCriteriaAngleBracketNode Node;

        [TestInitialize]
        public void Init()
        {
            Node = new ClangCriteriaAngleBracketNode();
        }

        [TestMethod]
        public void Pass_ShouldWorkForTwoAngles()
        {
            Assert.IsTrue(Node.Pass("<>"));
        }
        
        [TestMethod]
        public void Pass_ShouldWorkForTwoAnglesWithWord()
        {
            Assert.IsTrue(Node.Pass("<abc>"));
        }

        [TestMethod]
        public void Pass_ShouldFailForLeftOnlyBracket()
        {
            Assert.IsFalse(Node.Pass("<abc"));
        }

        [TestMethod]
        public void Pass_ShouldFailForRightOnlyBracket()
        {
            Assert.IsFalse(Node.Pass("abc>"));
        }

        [TestMethod]
        public void Pass_ShouldFailForNoBrackets()
        {
            Assert.IsFalse(Node.Pass("abc"));
        }
    }
}
