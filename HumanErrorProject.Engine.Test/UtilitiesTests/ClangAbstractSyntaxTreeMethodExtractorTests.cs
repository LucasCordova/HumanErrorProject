using HumanErrorProject.Data.Models;
using HumanErrorProject.Engine.Analysis.AbstractSyntaxTree;
using HumanErrorProject.Engine.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HumanErrorProject.Engine.Test.UtilitiesTests
{
    [TestClass]
    public class ClangAbstractSyntaxTreeMethodExtractorTests
    {
        protected AbstractSyntaxTreeNode Root;
        protected ClangAbstractSyntaxTreeMethodExtractor Extractor;
        protected MethodDeclaration MethodDeclaration;

        [TestInitialize]
        public void Init()
        {
            Root = MockSnapshots.GetCalculatorClassAbstractSyntaxTreeNode();
            Extractor = new ClangAbstractSyntaxTreeMethodExtractor(new ClangLineSplitter());
            MethodDeclaration = MockSnapshots.GetCalculatorAddMethodDeclaration();
        }

        [TestMethod]
        public void Extract_ShouldNotFindClassNodeWithMethod()
        {
            var node = Extractor.ExtractOrDefault(Root, MethodDeclaration);
            var visitor = new MockClangAbstractSyntaxTreeMethodExtractorSearchVisitor(
                MockSnapshots.GetCalculatorAbstractSyntaxTreeClassValue());

            node.PreOrder(visitor);

            Assert.IsFalse(visitor.Found);
        }

        [TestMethod]
        public void Extract_ShouldFindMethod()
        {
            var node = Extractor.ExtractOrDefault(Root, MethodDeclaration);
            var visitor = new MockClangAbstractSyntaxTreeMethodExtractorSearchVisitor(
                MockSnapshots.GetCalculatorAbstractSyntaxTreeAddValue());

            node.PreOrder(visitor);

            Assert.IsTrue(visitor.Found);
        }

        public class MockClangAbstractSyntaxTreeMethodExtractorSearchVisitor : IAbstractSyntaxTreeSearchVisitor
        {
            public MockClangAbstractSyntaxTreeMethodExtractorSearchVisitor(string value)
            {
                Value = value;
            }
            public string Value { get; }
            public bool Found { get; private set; }
            public void Visit(AbstractSyntaxTreeNode node)
            {
                Found = node.Value.Equals(Value);
            }
        }
    }
}
