using HumanErrorProject.Engine.Utilities;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HumanErrorProject.Engine.Test.UtilitiesTests
{
    [TestClass]
    public class ClangLineSplitterTests
    {
        protected ClangLineSplitter Splitter;

        [TestInitialize]
        public void Init()
        {
            Splitter = new ClangLineSplitter();
        }

        [TestMethod]
        public void Split_TwoCharactersBySpace()
        {
            var expected = new[]
            {
                "a", "b"
            };
            var actual = Splitter.Split("a b");

            Assert.AreEqual(expected.Join(), actual.Join());
        }

        [TestMethod]
        public void Split_WithSequenceOfCharacters()
        {
            var expected = new[]
            {
                "abc",
                "a",
                "abc"
            };
            var actual = Splitter.Split("abc a abc");

            Assert.AreEqual(expected.Join(), actual.Join());
        }

        [TestMethod]
        public void Split_WithBrackets()
        {
            var expected = new[]
            {
                "<abc>",
                "abc",
                "<abc>"
            };
            var actual = Splitter.Split("<abc> abc <abc>");

            Assert.AreEqual(expected.Join(), actual.Join());
        }

        [TestMethod]
        public void Split_WithEmbeddedBrackets()
        {
            var expected = new[]
            {
                "abc",
                "< abc >",
                "<<abc>>",
                "<abc < abc < abc > ds>as >"
            };
            var actual = Splitter.Split("abc < abc > <<abc>> <abc < abc < abc > ds>as >");

            Assert.AreEqual(expected.Join(), actual.Join());
        }

        [TestMethod]
        public void Split_SingleQuotes()
        {
            var expected = new[]
            {
                "\'a\'",
                "\'abcde\'",
            };
            var actual = Splitter.Split("\'a\' \'abcde\'");

            Assert.AreEqual(expected.Join(), actual.Join());
        }
        [TestMethod]
        public void Split_DoubleQuotes()
        {
            var expected = new[]
            {
                "\"a\"",
                "\"abcde\"",
            };
            var actual = Splitter.Split("\"a\" \"abcde\"");

            Assert.AreEqual(expected.Join(), actual.Join());
        }

        [TestMethod]
        public void Split_WithColon()
        {
            var expected = new[]
            {
                "\'list_node<T>\'",
                ":",
                "\'node<T>\'",
            };
            var actual = Splitter.Split("\'list_node<T>\':\'node<T>\'");

            Assert.AreEqual(expected.Join(), actual.Join());
        }

        [TestMethod]
        public void Split_WithAllCases()
        {
            var expected = new[]
            {
                "CXXDependentScopeMemberExpr",
                "\'list_node<T>\'",
                ":",
                "\"node<T>\"",
                "0x23141046a00",
                "<<col:11, col:15>>",
                "\'<dependent type>\'",
                "lvalue",
                ".next_",
                "->tail_"
            };
            var actual = 
                Splitter.Split("CXXDependentScopeMemberExpr   \'list_node<T>\':\"node<T>\" 0x23141046a00   <<col:11, col:15>> \'<dependent type>\' lvalue .next_ ->tail_");

            Assert.AreEqual(expected.Join(), actual.Join());
        }
    }
}
