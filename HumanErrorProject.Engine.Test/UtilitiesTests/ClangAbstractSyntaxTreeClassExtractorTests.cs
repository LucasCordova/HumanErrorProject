using HumanErrorProject.Engine.Analysis.AbstractSyntaxTree;
using HumanErrorProject.Engine.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HumanErrorProject.Engine.Test.UtilitiesTests
{
    [TestClass]
    public class ClangAbstractSyntaxTreeClassExtractorTests
    {
        protected AbstractSyntaxTreeNode Root;
        protected ClangAbstractSyntaxTreeClassExtractor Extractor;

        [TestInitialize]
        public void Init()
        {
            Root = MockSnapshots.GetCalculatorFullAbstractSyntaxTreeNode();
            Extractor = new ClangAbstractSyntaxTreeClassExtractor(
                new ClangLineSplitter());
        }

        [TestMethod]
        public void Extract_ShouldFindNodeWithClassLine()
        {
            var visitor = new MockClangAbstractSyntaxTreeClassExtractorSearchVisitor(
                MockSnapshots.GetCalculatorAbstractSyntaxTreeClassValue());

            var node = Extractor.Extract(Root, MockSnapshots.GetCalculatorClassName());

            node.PreOrder(visitor);

            Assert.IsTrue(visitor.Found);
        }

        [TestMethod]
        public void Extract_ShouldFindNodeWithAddLine()
        {
            var visitor = new MockClangAbstractSyntaxTreeClassExtractorSearchVisitor(
                MockSnapshots.GetCalculatorAbstractSyntaxTreeAddValue());

            var node = Extractor.Extract(Root, MockSnapshots.GetCalculatorClassName());

            node.PreOrder(visitor);

            Assert.IsTrue(visitor.Found);
        }
        
        [TestMethod]
        public void Extract_ShouldFindNodeWithSubtLine()
        {
            var visitor = new MockClangAbstractSyntaxTreeClassExtractorSearchVisitor(
                MockSnapshots.GetCalculatorAbstractSyntaxTreeSubtValue());

            var node = Extractor.Extract(Root, MockSnapshots.GetCalculatorClassName());

            node.PreOrder(visitor);

            Assert.IsTrue(visitor.Found);
        }
        
        [TestMethod]
        public void Extract_ShouldFindNodeWithMultLine()
        {
            var visitor = new MockClangAbstractSyntaxTreeClassExtractorSearchVisitor(
                MockSnapshots.GetCalculatorAbstractSyntaxTreeMultValue());

            var node = Extractor.Extract(Root, MockSnapshots.GetCalculatorClassName());

            node.PreOrder(visitor);

            Assert.IsTrue(visitor.Found);
        }
        
        [TestMethod]
        public void Extract_ShouldFindNodeWithDivLine()
        {
            var visitor = new MockClangAbstractSyntaxTreeClassExtractorSearchVisitor(
                MockSnapshots.GetCalculatorAbstractSyntaxTreeDivValue());

            var node = Extractor.Extract(Root, MockSnapshots.GetCalculatorClassName());

            node.PreOrder(visitor);

            Assert.IsTrue(visitor.Found);
        }

        public class MockClangAbstractSyntaxTreeClassExtractorSearchVisitor : IAbstractSyntaxTreeSearchVisitor
        {
            public MockClangAbstractSyntaxTreeClassExtractorSearchVisitor(string value)
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
