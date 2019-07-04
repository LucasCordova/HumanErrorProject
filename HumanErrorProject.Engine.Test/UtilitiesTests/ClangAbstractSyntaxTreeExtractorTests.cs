using HumanErrorProject.Engine.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HumanErrorProject.Engine.Test.UtilitiesTests
{
    [TestClass]
    public class ClangAbstractSyntaxTreeExtractorTests
    {
        protected ClangAbstractSyntaxTreeExtractor Extractor;

        [TestInitialize]
        public void Init()
        {
            Extractor = new ClangAbstractSyntaxTreeExtractor();
        }

        [TestMethod]
        public void Extract_GetClassWithDefaultParameterShouldHaveHeight()
        {
            using (var reader = MockSnapshots.GetClassWithDefaultParameterAbstractSyntaxTreeReader())
            {
                var node = Extractor.Extract(reader);
                Assert.AreEqual(MockSnapshots.GetClassWithDefaultParameterHeight(), node.Height);
            }
        }
    }
}
