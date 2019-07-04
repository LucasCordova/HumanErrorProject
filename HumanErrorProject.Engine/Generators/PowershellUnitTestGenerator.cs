using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HumanErrorProject.Data.Models;
using HumanErrorProject.Engine.Data;
using HumanErrorProject.Engine.Options;
using HumanErrorProject.Engine.Utilities;
using Microsoft.Extensions.Options;

namespace HumanErrorProject.Engine.Generators
{
    public class PowershellUnitTestGenerator : IUnitTestGenerator
    {
        public PowershellOptions Options { get; }

        public PowershellUnitTestGenerator(IOptions<PowershellOptions> options)
        {
            Options = options.Value;
        }

        public async Task<ICollection<UnitTestResult>> GenerateResults(SubmissionData data, string snapshot, Assignment assignment, ICollection<SnapshotMethod> snapshotMethods)
        {
            try
            {
                return await GenerateResultsImpl(data, snapshot, assignment, snapshotMethods);
            }
            catch (EngineReportExceptionData exception)
            {
                exception.Type = "Build";
                throw;
            }
        }

        public async Task<ICollection<UnitTest>> GenerateResults(PreAssignment assignment, DirectoryHandler handler, string root)
        {
            using (var testHandler = new DirectoryHandler(GetTestDirectory(handler.Directory)))
            {
                var testProject = new TestProjectObj(testHandler.Directory, assignment.TestProject);

                if (!Directory.Exists(testProject.TestFolder))
                    throw new EngineAssignmentExceptionData()
                    {
                        Report = new PreAssignmentBadTestFolderReport()
                    };

                testProject.MoveFilesToProject(root);

                var preprocessorArguments = GetPreprocessorArguments(assignment.Solution.MethodDeclarations);

                var process = new EngineProcess(GetEngineProcessData(testHandler.Directory,
                    testProject, preprocessorArguments));

                var exitCode = process.Run();

                if (exitCode == 0)
                {
                    process.Stop();
                    return GetUnitTestResults(ResultsFile(testHandler.Directory));
                }

                EngineAssignmentExceptionData exception;

                using (var reader = process.StandardError)
                {
                    exception = new EngineAssignmentExceptionData()
                    {
                        Report = new PreAssignmentBuildFailureReport()
                        {
                            Report = reader.ReadToEnd(),
                        }
                    };
                }
                process.Stop();
                throw exception;
            }
        }

        public ICollection<UnitTest> GetUnitTestResults(string results)
        {
            var passedUnitTests = new List<UnitTest>();
            var failedUnitTests = new List<UnitTest>();
            var lines = File.ReadAllLines(results);

            foreach (var line in lines)
            {
                var split = line.Split(' ');
                if (split.Length != 2) continue;

                if (split[0].Equals(Options.PassedValue))
                {
                    passedUnitTests.Add(new UnitTest()
                    {
                        Name = split[1]
                    });
                }
                else
                {
                    failedUnitTests.Add(new UnitTest()
                    {
                        Name = split[1],
                    });
                }
            }

            if (failedUnitTests.Any())
                throw new EngineAssignmentExceptionData()
                {
                    Report = new PreAssignmentFailTestsFailureReport()
                    {
                        FailUnitTests = failedUnitTests
                    }
                };
            return passedUnitTests;
        }

        public string GetPreprocessorArguments(ICollection<MethodDeclaration> methodDeclarations)
        {
            return methodDeclarations.Aggregate("", (current, method) => current + $"/p:{method.PreprocessorDirective}=1 ");
        }

        public async Task<ICollection<UnitTestResult>> GenerateResultsImpl(SubmissionData data, string snapshot, Assignment assignment,
            ICollection<SnapshotMethod> snapshotMethods)
        {
            using (var handler =
                new DirectoryHandler(GetTestDirectory(data)))
            {
                var testProject = new TestProjectObj(handler.Directory, assignment.TestProject);
                testProject.MoveFilesToProject(data.SnapshotSourceFiles(snapshot));

                var preprocessorArguments = GetPreprocessorArguments(snapshotMethods);

                var process = new EngineProcess(GetEngineProcessData(handler.Directory,
                    testProject, preprocessorArguments));

                var exitCode = process.Run();
                if (exitCode == 0)
                {
                    process.Stop();
                    return GetUnitTestResults(assignment.TestProject,
                        ResultsFile(handler.Directory));
                }

                EngineReportExceptionData exception;

                using (var reader = process.StandardError)
                {
                    exception = new EngineReportExceptionData(reader.ReadToEnd())
                    {
                        Type = "Build",
                    };
                }
                process.Stop();
                throw exception;
            }
        }

        public ICollection<UnitTestResult> GetUnitTestResults(TestProject testProject, string resultsFile)
        {
            var lines = File.ReadAllLines(resultsFile)
                .Where(line =>
                {
                    var split = line.Split(' ');
                    if (split.Length == 2)
                    {
                        return testProject.UnitTests.Any(unitTest =>
                            unitTest.Name.Equals(split[1]));
                    }
                    return false;
                });

            return testProject.UnitTests.Select(unitTest =>
            {
                var test = lines.FirstOrDefault(line => line.Split(' ')[1].Equals(unitTest.Name));
                if (test == null)
                {
                    return new UnitTestResult()
                    {
                        Passed = false,
                        UnitTest = unitTest
                    };
                }
                return new UnitTestResult()
                {
                    UnitTest = unitTest,
                    Passed = test.Split(' ')[0].Equals(Options.PassedValue)
                };
            }).ToList();
        }

        public string GetPreprocessorArguments(ICollection<SnapshotMethod> snapshotMethods)
        {
            var argument = "";
            foreach (var method in snapshotMethods)
            {
                argument += $"/p:{method.MethodDeclaration.PreprocessorDirective}=";
                argument += method.Declared ? "1 " : "0 ";
            }

            return argument;
        }

        public string ResultsFile(string workingDirectory)
            => Path.Combine(workingDirectory, Options.ResultsFile);

        public EngineProcessData GetEngineProcessData(string workingDirectory,
            TestProjectObj testProject, string preprocessorArgument)
        {
            return new EngineProcessData()
            {
                Command = Options.CommandPath,
                Arguments = $"-TestProject \'{testProject.TestProjectFile}\' " +
                            $"-TestFile \'{testProject.TestDllFile}\' " +
                            $"-ResultFile \'{ResultsFile(workingDirectory)}\' " +
                            $"-Arguments \'{preprocessorArgument}\'",
                WorkingDirectory = workingDirectory
            };
        }

        public string GetTestDirectory(SubmissionData data)
            => GetTestDirectory(data.Root);

        public string GetTestDirectory(string root) => Path.Combine(root, "Test");

        public class TestProjectObj
        {
            public TestProject TestProject { get; }
            public string Root { get; }
            public TestProjectObj(string root, TestProject testProject)
            {
                TestProject = testProject;
                Root =
                    EngineFileUtilities.ExtractZip(root,
                    "TestProject", testProject.Files);
            }

            public string TestProjectFile
                => Path.Combine(Root, TestProject.TestProjectFile);
            public string TestDllFile
                => Path.Combine(Root, TestProject.TestDllFile);
            public string TestFolder
                => Path.Combine(Root, TestProject.TestFolder);

            public void MoveFilesToProject(IEnumerable<string> files)
            {
                foreach (var file in files)
                {
                    File.Copy(file, Path.Combine(TestFolder,
                            Path.GetFileName(file)));
                }
            }

            public void MoveFilesToProject(string root)
            {
                MoveFilesToProject(Directory.GetFiles(root));
            }
        }
    }
}
