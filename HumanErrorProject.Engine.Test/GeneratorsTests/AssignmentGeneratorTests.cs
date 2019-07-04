using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HumanErrorProject.Data.DataAccess;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Engine.Analysis.AbstractSyntaxTree;
using HumanErrorProject.Engine.Data;
using HumanErrorProject.Engine.Generators;
using HumanErrorProject.Engine.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HumanErrorProject.Engine.Test.GeneratorsTests
{
    [TestClass]
    public class AssignmentGeneratorTests
    {
        protected string Root;
        protected DirectoryHandler Handler;
        protected PreAssignment Assignment;
        protected IRepository<PreAssignment, int> PreAssignmentRepository;
        protected AssignmentGenerator Generator;
        protected MockAbstractSyntaxTreeGenerator AbstractSyntaxTreeGenerator;
        protected MockAbstractSyntaxTreeClassExtractor AbstractSyntaxTreeClassExtractor;
        protected MockAbstractSyntaxTreeMethodExtractor AbstractSyntaxTreeMethodExtractor;
        protected MockUnitTestGenerator UnitTestGenerator;
        [TestInitialize]
        public void Init()
        {
            Root = Path.Combine(Directory.GetCurrentDirectory(), nameof(AssignmentGeneratorTests));
            Handler = new DirectoryHandler(Root);
            PreAssignmentRepository = new MockRepository<PreAssignment, int>();

            Assignment = new PreAssignment()
            {
                PreAssignmentReport = new PreAssignmentPendingReport(),
                Filename = MockSnapshots.GetCalculatorFile(),
                Solution = new AssignmentSolution()
                {
                    Id = 1,
                    Name = MockSnapshots.GetCalculatorClassName(),
                    Files = MockSnapshots.GetCalculatorSolutionFiles(),
                    MethodDeclarations = MockSnapshots.GetCalculatorMethodDeclaration().ToList(),
                },
                AssignmentSolutionId = 1,
                TestProject = new TestProject()
                {
                    Files = MockSnapshots.GetCalculatorTestProjectFiles(),
                    TestFolder = MockSnapshots.GetCalculatorTestProjectFolder(),
                    TestDllFile = MockSnapshots.GetCalculatorTestProjectDll(),
                    TestProjectFile = MockSnapshots.GetCalculatorTestProjectFile()
                }
            };

            PreAssignmentRepository.Add(Assignment);
            AbstractSyntaxTreeGenerator = new MockAbstractSyntaxTreeGenerator();
            AbstractSyntaxTreeClassExtractor = new MockAbstractSyntaxTreeClassExtractor();
            AbstractSyntaxTreeMethodExtractor = new MockAbstractSyntaxTreeMethodExtractor();
            UnitTestGenerator = new MockUnitTestGenerator();

            Generator = new AssignmentGenerator(PreAssignmentRepository,
                AbstractSyntaxTreeGenerator,
                AbstractSyntaxTreeClassExtractor,
                AbstractSyntaxTreeMethodExtractor,
                UnitTestGenerator
                );
        }

        [TestMethod]
        public async Task Generate_ShouldHaveSucessfulGenerate()
        {
            await Generator.Generate(Assignment, Handler);
            Assert.AreEqual(PreAssignmentReport.PreAssignmentReportTypes.Success,
                Assignment.PreAssignmentReport.Type);
        }

        [TestMethod]
        public async Task Generate_ShouldFailWithNoFileFound()
        {
            AbstractSyntaxTreeGenerator.Type = PreAssignmentReport.PreAssignmentReportTypes.NoFileFailure;
            await Generator.Generate(Assignment, Handler);
            Assert.AreEqual(PreAssignmentReport.PreAssignmentReportTypes.NoFileFailure,
                Assignment.PreAssignmentReport.Type);
        }

        [TestMethod]
        public async Task Generate_ShouldFailWithComipleError()
        {
            AbstractSyntaxTreeGenerator.Type = PreAssignmentReport.PreAssignmentReportTypes.CompileFailure;
            await Generator.Generate(Assignment, Handler);
            Assert.AreEqual(PreAssignmentReport.PreAssignmentReportTypes.CompileFailure,
                Assignment.PreAssignmentReport.Type);
        }

        [TestMethod]
        public async Task Generate_ShouldFailWithNoClassFound()
        {
            AbstractSyntaxTreeClassExtractor.Type = PreAssignmentReport.PreAssignmentReportTypes.NoClassFailure;
            await Generator.Generate(Assignment, Handler);
            Assert.AreEqual(PreAssignmentReport.PreAssignmentReportTypes.NoClassFailure,
                Assignment.PreAssignmentReport.Type);
        }

        [TestMethod]
        public async Task Generate_ShouldFailWithMissingMethods()
        {
            AbstractSyntaxTreeMethodExtractor.Null = true;
            await Generator.Generate(Assignment, Handler);
            Assert.AreEqual(PreAssignmentReport.PreAssignmentReportTypes.MissingMethodsFailure,
                Assignment.PreAssignmentReport.Type);
        }

        [TestMethod]
        public async Task Generate_ShouldFailWithBuildFailure()
        {
            UnitTestGenerator.Type = PreAssignmentReport.PreAssignmentReportTypes.BuildFailure;
            await Generator.Generate(Assignment, Handler);
            Assert.AreEqual(PreAssignmentReport.PreAssignmentReportTypes.BuildFailure,
                Assignment.PreAssignmentReport.Type);
        }
        
        [TestMethod]
        public async Task Generate_ShouldFailWithFailTests()
        {
            UnitTestGenerator.Type = PreAssignmentReport.PreAssignmentReportTypes.FailTestsFailure;
            await Generator.Generate(Assignment, Handler);
            Assert.AreEqual(PreAssignmentReport.PreAssignmentReportTypes.FailTestsFailure,
                Assignment.PreAssignmentReport.Type);
        }

        [TestMethod]
        public async Task Generate_ShouldUpdateRepository()
        {
            await Generator.Generate(Assignment, Handler);
            var assignment = await PreAssignmentRepository.Get(Assignment.Id);

            Assert.AreEqual(PreAssignmentReport.PreAssignmentReportTypes.Success,
                assignment.PreAssignmentReport.Type);
        }

        public class MockUnitTestGenerator : IUnitTestGenerator
        {
            public PreAssignmentReport.PreAssignmentReportTypes Type { get; set; } =
                PreAssignmentReport.PreAssignmentReportTypes.Success;

            public Task<ICollection<UnitTestResult>> GenerateResults(SubmissionData data, string snapshot, Assignment assignment, ICollection<SnapshotMethod> snapshotMethods)
            {
                throw new System.NotImplementedException();
            }

            public Task<ICollection<UnitTest>> GenerateResults(PreAssignment assignment, DirectoryHandler hander, string root)
            {
                switch (Type)
                {
                    case PreAssignmentReport.PreAssignmentReportTypes.BuildFailure:
                        throw new EngineAssignmentExceptionData()
                        {
                            Report = new PreAssignmentBuildFailureReport()
                        };
                    case PreAssignmentReport.PreAssignmentReportTypes.FailTestsFailure:
                        throw new EngineAssignmentExceptionData()
                        {
                            Report = new PreAssignmentFailTestsFailureReport()
                        };
                }

                return Task.FromResult<ICollection<UnitTest>>(assignment.Solution.MethodDeclarations.Select(m => new UnitTest()).ToList());
            }
        }

        public class MockAbstractSyntaxTreeGenerator : IAbstractSyntaxTreeGenerator
        {
            public PreAssignmentReport.PreAssignmentReportTypes Type { get; set; } =
                PreAssignmentReport.PreAssignmentReportTypes.Success;

            public AbstractSyntaxTreeNode CreateFromFile(SubmissionData data, string path)
            {
                throw new System.NotImplementedException();
            }

            public AbstractSyntaxTreeNode CreateFromFile(DirectoryHandler handler, string path)
            {
                switch (Type)
                {
                    case PreAssignmentReport.PreAssignmentReportTypes.NoFileFailure:
                        throw new EngineAssignmentExceptionData()
                        {
                            Report = new PreAssignmentNoFileFailureReport()
                        };
                    case PreAssignmentReport.PreAssignmentReportTypes.CompileFailure:
                        throw new EngineAssignmentExceptionData()
                        {
                            Report = new PreAssignmentCompileFailureReport()
                        };
                }
                return new AbstractSyntaxTreeNode("Root");
            }
        }

        public class MockAbstractSyntaxTreeMethodExtractor : IAbstractSyntaxTreeMethodExtractor
        {
            public bool Null { get; set; } = false;
            public AbstractSyntaxTreeNode ExtractOrDefault(AbstractSyntaxTreeNode root, MethodDeclaration methodDeclaration)
            {
                return Null ? null : new AbstractSyntaxTreeNode("Root");
            }
        }

        public class MockAbstractSyntaxTreeClassExtractor : IAbstractSyntaxTreeClassExtractor
        {
            public PreAssignmentReport.PreAssignmentReportTypes Type { get; set; }
                = PreAssignmentReport.PreAssignmentReportTypes.Success;

            public AbstractSyntaxTreeNode Extract(AbstractSyntaxTreeNode root, string name)
            {
                if (Type == PreAssignmentReport.PreAssignmentReportTypes.NoClassFailure)
                    throw new EngineAssignmentExceptionData()
                    {
                        Report = new PreAssignmentNoClassFailureReport()
                    };
                return new AbstractSyntaxTreeNode("Root");
            }

        }

        [TestCleanup]
        public void Cleanup()
        {
            Handler.Dispose();
        }
    }
}
