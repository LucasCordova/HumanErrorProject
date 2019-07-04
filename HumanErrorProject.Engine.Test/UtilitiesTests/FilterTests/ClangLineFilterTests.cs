using System;
using HumanErrorProject.Engine.Test.UtilitiesTests.FilterTests.ClangCriteriaTests;
using HumanErrorProject.Engine.Utilities;
using HumanErrorProject.Engine.Utilities.Filter;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HumanErrorProject.Engine.Test.UtilitiesTests.FilterTests
{
    [TestClass]
    public class ClangLineFilterTests : BaseClangCriteriaNodeTests
    {
        protected ClangLineFilter Filter;

        [TestInitialize]
        public void Init()
        {
            Filter = new ClangLineFilter(new MockClangCriteriaNode("ab"),
                new MockClangLineSplitter());
        }

        [TestMethod]
        public void Filter_ShouldWorkForOneValue()
        {
            Assert.AreEqual("ab", Filter.Filter("ab"));
        }

        [TestMethod]
        public void Filter_ShouldWorkForOneValueWithExtraCharacter()
        {
            Assert.AreEqual("abc", Filter.Filter("abc"));
        }

        [TestMethod]
        public void Filter_ShouldWorkForTwoValues()
        {
            Assert.AreEqual(new []
            {
                "abd", "abc",
            }.Join(" "), Filter.Filter("abd abc"));
        }

        [TestMethod]
        public void Filter_ShouldWorkForTwoValuesAndIgnoreOne()
        {
            Assert.AreEqual(new[]
            {
                "abd", "abc",
            }.Join(" "), Filter.Filter("abd ff abc"));
        }
        
        [TestMethod]
        public void Filter_ShouldWorkForTwoValuesAndIgnoreThree()
        {
            Assert.AreEqual(new[]
            {
                "abd", "abc",
            }.Join(" "), Filter.Filter("ff abd ff abc ff"));
        }


        protected class MockClangLineSplitter : ILineSplitter
        {
            public string[] Split(string line)
            {
                return line.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);
            }
        }
    }
}
