using HumanErrorProject.Data.Models;
using HumanErrorProject.Engine.Analysis;
using HumanErrorProject.Engine.Analysis.AbstractSyntaxTree;
using HumanErrorProject.Engine.Generators;
using HumanErrorProject.Engine.Utilities;
using HumanErrorProject.Engine.Utilities.Filter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HumanErrorProject.Engine.Test.GeneratorsTests
{
    [TestClass]
    public class SnapshotMethodGeneratorTests
    {
        protected SnapshotMethodGenerator Generator;
        protected MockAbstractSyntaxTreeMetricExtractor Extractor;

        [TestInitialize]
        public void Init()
        {
            Extractor = new MockAbstractSyntaxTreeMetricExtractor()
            {
                ReturnNull = false,
            };
            Generator = new SnapshotMethodGenerator(
                Extractor, new MockLineFilter(), 
                new MockAbstractSyntaxxTreeMetricCreator(), 
                new MockBagOfWordsMetricCreator());
        }

        [TestMethod]
        public void Generate_ShouldReturnANonDeclaredMethod()
        {
            Extractor.ReturnNull = true;
            var method = Generator.Generate(new AbstractSyntaxTreeNode(""),
                new AbstractSyntaxTreeNode(""),
                new MethodDeclaration());
            Assert.IsFalse(method.Declared);
        }

        [TestMethod]
        public void Generate_ShouldReturnADeclaredMethod()
        {
            var method = Generator.Generate(new AbstractSyntaxTreeNode(""),
                new AbstractSyntaxTreeNode(""),
                new MethodDeclaration());
            Assert.IsTrue(method.Declared);
        }


        public class MockLineFilter : ILineFilter
        {
            public string Filter(string value) => value;
        }

        public class MockAbstractSyntaxTreeMetricExtractor : 
            IAbstractSyntaxTreeMethodExtractor
        {
            public bool ReturnNull { get; set; }
            public AbstractSyntaxTreeNode ExtractOrDefault(AbstractSyntaxTreeNode root, MethodDeclaration methodDeclaration)
            {
                if (ReturnNull) return null;
                return root;
            }
        }

        public class MockAbstractSyntaxxTreeMetricCreator : IAbstractSyntaxTreeMetricCreator
        {
            public AbstractSyntaxTreeMetric Create(AbstractSyntaxTreeNode expected, AbstractSyntaxTreeNode actual)
            {
                return new AbstractSyntaxTreeMetric();
            }
        }

        public class MockBagOfWordsMetricCreator : IBagOfWordsMetricCreator
        {
            public BagOfWordsMetric Create(AbstractSyntaxTreeNode expected, AbstractSyntaxTreeNode actual)
            {
                return new BagOfWordsMetric();
            }
        }
    }
}
