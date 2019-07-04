using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HumanErrorProject.Data.DataAccess;
using HumanErrorProject.Data.Dtos;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Engine.Data;
using HumanErrorProject.Engine.Generators;
using HumanErrorProject.Engine.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HumanErrorProject.Engine.Test.GeneratorsTests
{
    [TestClass]
    public class PowershellUnitTestGeneratorTests
    {
        protected string Root;
        protected SubmissionData Data;
        protected PowershellUnitTestGenerator Generator;
        protected Assignment Assignment;

        [TestInitialize]
        public void Init()
        {
            Root = Path.Combine(MockSnapshots.GetShortRoot(), "Powershell");
            Data = new SubmissionData(new StudentSubmissionDto()
                {
                    StudentName = "StudentName",
                    ClassName = "ClassName",
                    SnapshotFolder = MockSnapshots.GetCalculatorSnapshots(),
                }, Root);

            Assignment = new Assignment()
            {
                TestProject = new TestProject()
                {
                    Id = 1,
                    Files = MockSnapshots.GetCalculatorTestProjectFiles(),
                    TestFolder = MockSnapshots.GetCalculatorTestProjectFolder(),
                    TestDllFile = MockSnapshots.GetCalculatorTestProjectDll(),
                    TestProjectFile = MockSnapshots.GetCalculatorTestProjectFile(),
                    UnitTests = MockSnapshots.GetCalculatorUnitTests(),
                },
                TestProjectId = 1,
            };
            Generator = new PowershellUnitTestGenerator(
                Microsoft.Extensions.Options.Options.Create(
                    new PowershellOptions()
                    {
                        CommandPath = MockSnapshots.PowershellScript(),
                        PassedValue = MockSnapshots.PowershellPassedValue(),
                        ResultsFile = "test_results.txt",
                    }));
        }

        [TestMethod]
        public async Task Generate_ShouldReturnSameNumberOfTestsAsTestProject()
        {
            var methods = new CalculatorSnapshotMethods()
            {
                Addition = true,
                Division = true,
                Multiplication = true,
                Subtraction = true,
            }.GetMethods();
            var results = await Generator.GenerateResults(Data, MockSnapshots.GetLastCalculatorSnpahostName(),
                Assignment, methods);

            Assert.AreEqual(Assignment.TestProject.UnitTests.Count, results.Count);
        }

        [TestMethod]
        public async Task Generate_ShouldReturnPassedForAllTests()
        {
            var methods = new CalculatorSnapshotMethods()
            {
                Addition = true,
                Division = true,
                Multiplication = true,
                Subtraction = true,
            }.GetMethods();
            var results = await Generator.GenerateResults(Data, MockSnapshots.GetLastCalculatorSnpahostName(),
                Assignment, methods);
            foreach (var result in results) Assert.IsTrue(result.Passed);
        }

        [TestMethod]
        public async Task Generate_ShouldHaveOneFailTestForDivisionNotDeclared()
        {
            var methods = new CalculatorSnapshotMethods()
            {
                Addition = true,
                Division = false,
                Multiplication = true,
                Subtraction = true,
            }.GetMethods();
            var results = await Generator.GenerateResults(Data, MockSnapshots.GetLastCalculatorSnpahostName(),
                Assignment, methods);

            Assert.AreEqual(1, results.Count(r => !r.Passed));
        }


        public class CalculatorSnapshotMethods
        {
            public bool Addition { get; set; }
            public bool Subtraction { get; set; }
            public bool Division { get; set; }
            public bool Multiplication { get; set; }

            public IList<SnapshotMethod> GetMethods()
            {
                return new List<SnapshotMethod>()
                {
                    new SnapshotMethod()
                    {
                        Declared = Addition,
                        MethodDeclaration = new MethodDeclaration()
                        {
                            PreprocessorDirective = "ADDITION",
                        }
                    },
                    new SnapshotMethod()
                    {
                        Declared = Subtraction,
                        MethodDeclaration = new MethodDeclaration()
                        {
                            PreprocessorDirective = "SUBTRACTION",
                        }
                    },
                    new SnapshotMethod()
                    {
                        Declared = Division,
                        MethodDeclaration = new MethodDeclaration()
                        {
                            PreprocessorDirective = "DIVISION",
                        }
                    },
                    new SnapshotMethod()
                    {
                        Declared = Multiplication,
                        MethodDeclaration = new MethodDeclaration()
                        {
                            PreprocessorDirective = "MULTIPLICATION",
                        }
                    },

                };
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
    }
}
