using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HumanErrorProject.Data.DataAccess;
using HumanErrorProject.Data.Dtos;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Engine.Analysis.AbstractSyntaxTree;
using HumanErrorProject.Engine.Data;
using HumanErrorProject.Engine.Generators;
using HumanErrorProject.Engine.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HumanErrorProject.Engine.Test.GeneratorsTests
{
    [TestClass]
    public class SnapshotReportGeneratorTests
    {
        protected string Root;
        protected SnapshotReportGenerator Generator;
        protected IRepository<SnapshotReport, int> Reports;
        protected MockAbstractSyntaxTreeGenerator AbstractSyntaxTreeGenerator;
        protected SubmissionData Data;
        protected Assignment Assignment;

        [TestInitialize]
        public void Init()
        {
            Root = Path.Combine(Directory.GetCurrentDirectory(), nameof(SnapshotReportGeneratorTests));
            AbstractSyntaxTreeGenerator = new MockAbstractSyntaxTreeGenerator {Throw = false};
            Reports = new MockRepository<SnapshotReport, int>();
            Generator = new SnapshotReportGenerator(
                AbstractSyntaxTreeGenerator, 
                new MockAbstractSyntaxTreeClassExtractor(), Reports,
                new MockSnapshotMethodGenerator(),
                new MockUnitTestGenerator()
                );
            Data = new SubmissionData(new StudentSubmissionDto()
            {
                ClassName = "ClassName",
                StudentName = "StudentName",
                SnapshotFolder = MockSnapshots.GetCalculatorSnapshots(),
            }, Root);

            Assignment = new Assignment()
            {
                Filename = MockSnapshots.GetCalculatorFile(),
                Solution = new AssignmentSolution()
                {
                    Name = MockSnapshots.GetCalculatorClassName(),
                },
            };
        }


        [TestMethod]
        public async Task Generate_ShouldReturnAFailureReportForTreeGeneratorThrow()
        {
            AbstractSyntaxTreeGenerator.Throw = true;
            var report = await Generator.Generate(Data,
                MockSnapshots.GetFirstCalculatorSnapshotName(),
                Assignment, new AbstractSyntaxTreeNode("Root"));

            Assert.AreEqual(SnapshotReport.SnapshotReportTypes.Failure, report.Type);
        }

        [TestMethod]
        public async Task Generate_ShouldReturnASucessfulReport()
        {
            var report = await Generator.Generate(Data, MockSnapshots.GetFirstCalculatorSnapshotName(),
                Assignment, new AbstractSyntaxTreeNode("Root"));
            Assert.AreEqual(SnapshotReport.SnapshotReportTypes.Success, report.Type);
        }

        [TestMethod]
        public async Task Generate_ShouldAddToReports()
        {
            var expected = (await Reports.GetAll()).Count() + 1;
            await Generator.Generate(Data, MockSnapshots.GetFirstCalculatorSnapshotName(),
                Assignment, new AbstractSyntaxTreeNode("Root"));

            var actual = (await Reports.GetAll()).Count();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task Generate_ShouldReturnWithIdNotEqualZero()
        {
            var report = await Generator.Generate(Data, MockSnapshots.GetFirstCalculatorSnapshotName(),
                Assignment, new AbstractSyntaxTreeNode("Root"));
            Assert.AreNotEqual(0, report.Id);
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

        public class MockUnitTestGenerator : IUnitTestGenerator
        {
            public Task<ICollection<UnitTestResult>> GenerateResults(SubmissionData data, string snapshot, Assignment assignment, ICollection<SnapshotMethod> snapshotMethods)
            {
                return Task.FromResult<ICollection<UnitTestResult>>(new List<UnitTestResult>());
            }

            public Task<ICollection<UnitTest>> GenerateResults(PreAssignment assignment, DirectoryHandler handler, string root)
            {
                throw new System.NotImplementedException();
            }
        }

        public class MockAbstractSyntaxTreeGenerator : IAbstractSyntaxTreeGenerator
        {
            public bool Throw { get; set; } = false;
            public AbstractSyntaxTreeNode CreateFromFile(SubmissionData data, string path)
            {
                if (Throw) throw new EngineReportExceptionData("Throwing")
                {
                    Type = "Mock",
                };
                return new AbstractSyntaxTreeNode("Root");
            }

            public AbstractSyntaxTreeNode CreateFromFile(DirectoryHandler handler, string path)
            {
                throw new System.NotImplementedException();
            }

            public AbstractSyntaxTreeNode CreateOrDefaultFromFile(DirectoryHandler handler, string path)
            {
                throw new System.NotImplementedException();
            }
        }

        public class MockAbstractSyntaxTreeClassExtractor : IAbstractSyntaxTreeClassExtractor
        {
            public AbstractSyntaxTreeNode Extract(AbstractSyntaxTreeNode root, string name)
            {
                return new AbstractSyntaxTreeNode("Root");
            }

            public AbstractSyntaxTreeNode ExtractOrDefault(AbstractSyntaxTreeNode root, string name)
            {
                throw new System.NotImplementedException();
            }
        }

        public class MockSnapshotMethodGenerator : ISnapshotMethodGenerator
        {
            public SnapshotMethod Generate(AbstractSyntaxTreeNode student, AbstractSyntaxTreeNode solution, MethodDeclaration method)
            {
                return new SnapshotMethod()
                {
                    Declared = true,
                };
            }
        }
    }
}
