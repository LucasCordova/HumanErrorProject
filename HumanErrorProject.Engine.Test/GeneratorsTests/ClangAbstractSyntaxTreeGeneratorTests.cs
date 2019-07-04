using System.IO;
using HumanErrorProject.Data.Dtos;
using HumanErrorProject.Engine.Analysis.AbstractSyntaxTree;
using HumanErrorProject.Engine.Data;
using HumanErrorProject.Engine.Generators;
using HumanErrorProject.Engine.Options;
using HumanErrorProject.Engine.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HumanErrorProject.Engine.Test.GeneratorsTests
{
    [TestClass]
    public class ClangAbstractSyntaxTreeGeneratorTests
    {
        protected string Root;
        protected ClangAbstractSyntaxTreeGenerator Generator;
        protected SubmissionData Data;  
        protected const string ValidNode = "Valid";
        protected const string InvalidNode = "Invalid";
        protected DirectoryHandler Handler;

        [TestInitialize]
        public void Init()
        {
            Root = Path.Combine(Directory.GetCurrentDirectory(), nameof(ClangAbstractSyntaxTreeGeneratorTests));
            Data = new SubmissionData(new StudentSubmissionDto()
            {
                SnapshotFolder = MockSnapshots.GetCalculatorSnapshots(),
                StudentName = "Student",
                ClassName = "Class",
            }, Root);
            Generator = new ClangAbstractSyntaxTreeGenerator(
                Microsoft.Extensions.Options.Options.Create(new ClangOptions()
                {
                    Command = MockSnapshots.ClangCommand,
                    Arguments = MockSnapshots.ClangArguments,
                    OutputFile = MockSnapshots.ClangOutputFile,
                }), new MockClangAbstractSyntaxTreeExtractor());
        }

        [TestMethod]
        public void Create_ShouldReturnValidNodeForCalculatorSnapshot()
        {
            var node = Generator.CreateFromFile(Data,
                Data.SnapshotSourceFileFullPath(MockSnapshots.GetFirstCalculatorSnapshotName(),
                    MockSnapshots.GetCalculatorFile()));
            Assert.AreEqual(ValidNode, node.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(EngineReportExceptionData))]
        public void Create_ShouldThrowExceptionForInvalidFile()
        {
            Generator.CreateFromFile(Data,
                Path.Combine(Data.SnapshotFolder,
                    MockSnapshots.GetFirstCalculatorSnapshotName(),
                    "Random.hpp"));
        }

        [TestMethod]
        public void Create_ShouldThrowAndCleanupDirectoryForInvalidFile()
        {
            try
            {
                Generator.CreateFromFile(Data,
                    Path.Combine(Data.SnapshotFolder,
                        MockSnapshots.GetFirstCalculatorSnapshotName(),
                        "Random.hpp"));
                Assert.Fail();
            }
            catch (EngineReportExceptionData)
            {
                Assert.IsFalse(Directory.Exists(Generator.GetCompilationDirectory(Data)));
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            Data.Dispose();
            if (Directory.Exists(Root))
            {
                Directory.Delete(Root, true);
            }
        }

        public class MockClangAbstractSyntaxTreeExtractor : IAbstractSyntaxTreeExtractor
        {
            public AbstractSyntaxTreeNode Extract(StreamReader reader)
            {
                return reader.EndOfStream ? new AbstractSyntaxTreeNode(InvalidNode) : new AbstractSyntaxTreeNode(ValidNode);
            }
        }
    }
}
