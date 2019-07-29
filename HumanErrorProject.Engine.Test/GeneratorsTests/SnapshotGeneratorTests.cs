using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
    public class SnapshotGeneratorTests
    {
        protected SnapshotGenerator Generator;
        protected SnapshotGenerator.SnapshotGeneratorObj GeneratorObj;
        protected Student Student;
        protected IRepository<Snapshot, int> Snapshots;
        protected IRepository<Student, string> Students;
        protected IRepository<SnapshotSubmission, int> SnapshotSubmission;
        protected Assignment Assignment;
        protected StudentSubmissionDto Submission;
        protected SubmissionData Data;
        protected const string StudentName = "Student";
        protected const string StudentId = "1";
        protected const int LastSnapshotId = 2;
        protected string Root;
        protected const int AssignmentId = 1;
        protected const int FirstCalculatorSubmissionId = 1;

        [TestInitialize]
        public void Init()
        {
            Root = Path.Combine(Directory.GetCurrentDirectory(), nameof(SnapshotGeneratorTests));

            Submission = new StudentSubmissionDto()
            {
                SnapshotFolder = MockSnapshots.GetCalculatorSnapshots(),
                StudentName = StudentName,
                ClassName = "Class",
            };
            Assignment = new Assignment()
            {
                Id = AssignmentId,
                Filename = MockSnapshots.GetCalculatorFile(),
                Solution = new AssignmentSolution()
                {
                    Files = MockSnapshots.GetCalculatorSolutionFiles(),
                    Name = MockSnapshots.GetCalculatorClassName(),
                }
            };

            Student =
                new Student()
                {
                    Id = StudentId,
                    Name = StudentName,
                    Submissions = new List<SnapshotSubmission>()
                    {
                        new SnapshotSubmission()
                        {
                            Id = FirstCalculatorSubmissionId,
                            CreatedDateTime = MockSnapshots.GetFirstCalculatorSnapshotTime(),
                        }
                    },
                    Snapshots = new List<Snapshot>()
                    {
                        new Snapshot()
                        {
                            Id = FirstCalculatorSubmissionId,
                            SnapshotSubmission = new SnapshotSubmission()
                            {
                                Id = FirstCalculatorSubmissionId,
                                CreatedDateTime = MockSnapshots.GetFirstCalculatorSnapshotTime(),
                                Files = MockSnapshots.GetFirstCalculatorSnapshotFile(),
                            },
                            AssignmentId = AssignmentId
                        },
                        new Snapshot()
                        {
                            Id = LastSnapshotId,
                            SnapshotSubmission = new SnapshotSubmission()
                            {
                                Id = 2,
                                CreatedDateTime = MockSnapshots.GetSecondCalculatorSnapshotTime(),
                                Files = MockSnapshots.GetSecondCalculatorSnapshotFile(),
                            },
                            AssignmentId = AssignmentId
                        }
                    }
                };
            Students = new MockRepository<Student, string>();
            Students.Add(Student);
            Snapshots = new MockRepository<Snapshot, int>();
            SnapshotSubmission = new MockRepository<SnapshotSubmission, int>(
                new List<SnapshotSubmission>()
                {
                    new SnapshotSubmission()
                    {
                        Id = FirstCalculatorSubmissionId,
                        CreatedDateTime = MockSnapshots.GetFirstCalculatorSnapshotTime()
                    }
                });
            Generator = new SnapshotGenerator(new MockSnapshotDateConverter(), SnapshotSubmission,
                Snapshots, new MockSnapshotReportGenerate(), Students,
                new MockAbstractSyntaxClassTreeExtractor(), 
                new MockAbstractSyntaxTreeGenerator());
            Data = new SubmissionData(Submission, Root)
            {
                Student = Student
            };
            GeneratorObj = new SnapshotGenerator.SnapshotGeneratorObj(Data, Assignment, Generator);
        }

        [TestMethod]
        public async Task Generate_ShouldGenerateSnapshotsForTheRemaining()
        {
            var expected = MockSnapshots.GetNumberOfNewSnapshotsAfterSecond();

            var actual = (await Generator.Generate(Data, Assignment)).Count;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetLastSnapshotOrDefault_ShouldGetSnapshot()
        {
            var snapshot = GeneratorObj.LastSnapshot;
            Assert.AreEqual(LastSnapshotId, snapshot.Id);
        }

        [TestMethod]
        public void GetLastSnapshotOrDefault_ShouldReturnNull()
        {
            var snapshot = new SnapshotGenerator.SnapshotGeneratorObj(Data,
                new Assignment()
                {
                    Filename = "RandomFile.hpp",
                    Id = 100
                }, Generator);
            Assert.IsNull(snapshot.LastSnapshot);
        }

        [TestMethod]
        public void NewSnapshot_ShouldReturnFalseForFirstSnapshot()
        {
            Assert.IsFalse(GeneratorObj.NewValidSnapshot(MockSnapshots.GetFirstCalculatorSnapshotName()));
        }

        [TestMethod]
        public void NewSnapshot_ShouldReturnFalseForSnapshotItCantConvert()
        {
            Assert.IsFalse(GeneratorObj.NewValidSnapshot("Can't Convert"));
        }

        [TestMethod]
        public void AreSnapshotsSame_ShouldReturnTrueForSecondSnapshot()
        {
            Assert.IsTrue(GeneratorObj.AreSnapshotsTheSame(
                Data.SnapshotSourceFileFullPath(MockSnapshots.GetSecondCalculatorSnapshotName(),
                    MockSnapshots.GetCalculatorFile()), MockSnapshots.GetCalculatorFile()));
        }

        [TestMethod]
        public async Task AddSubmissionToStudentSubmission_ShouldAddToStudentCollection()
        {
            var expected = (await SnapshotSubmission.GetAll()).Count() + 1;
            await GeneratorObj.AddSubmissionToStudentSubmission(
                MockSnapshots.GetThirdCalculatorSnapshotName());
            var actual = (await SnapshotSubmission.GetAll()).Count();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task AddSubmissionToStudentSubmission_ShouldPassForAlreadyInCollection()
        {
            var snapshotSubmission = await GeneratorObj.AddSubmissionToStudentSubmission(
                MockSnapshots.GetFirstCalculatorSnapshotName());

            Assert.AreEqual(FirstCalculatorSubmissionId, snapshotSubmission.Id);
        }

        [TestMethod]
        public async Task AddSubmissionToStudentSubmission_UnzipFilesShouldGetFile()
        {
            var snapshotSubmission = await GeneratorObj.AddSubmissionToStudentSubmission(
                MockSnapshots.GetThirdCalculatorSnapshotName());

            var files = (await SnapshotSubmission.Get(snapshotSubmission.Id)).Files;
            var snapshot = "Snapshot";

            EngineFileUtilities.ExtractZip(Data.Root, snapshot, files);

            var path = Path.Combine(Data.Root, snapshot, MockSnapshots.GetCalculatorFile());

            Assert.IsTrue(File.Exists(path));
        }

        [TestMethod]
        public async Task AddSubmissionToStudentSubmission_ShouldReturnIdNotNull()
        {
            await GeneratorObj.AddSubmissionToStudentSubmission(
                MockSnapshots.GetThirdCalculatorSnapshotName());
            var actual = (await SnapshotSubmission.GetAll()).Count();
            Assert.AreNotEqual(0, actual);
        }

        [TestMethod]
        public void AreSnapshotsSame_ShouldReturnFalseForDifferentSnapshot()
        {
            Assert.IsFalse(GeneratorObj.AreSnapshotsTheSame(
                Data.SnapshotSourceFileFullPath(MockSnapshots.GetThirdCalculatorSnapshotName(),
                    MockSnapshots.GetCalculatorFile()), MockSnapshots.GetCalculatorFile()));
        }

        [TestCleanup]
        public void Cleanup()
        {
            Data.Dispose();
            if (Directory.Exists(Root))
            {
                Directory.Delete(Root);
            }
        }

        private class MockSnapshotReportGenerate : ISnapshotReportGenerator
        {
            public Task<SnapshotReport> Generate(SubmissionData data, string snapshot, Assignment assignment, AbstractSyntaxTreeNode solution)
            {
                return Task.FromResult<SnapshotReport>(new SnapshotFailureReport());
            }
        }

        private class MockAbstractSyntaxTreeGenerator : IAbstractSyntaxTreeGenerator
        {
            public AbstractSyntaxTreeNode CreateFromFile(SubmissionData data, string path)
            {
                return new AbstractSyntaxTreeNode("Mock");
            }

            public AbstractSyntaxTreeNode CreateFromFile(DirectoryHandler handler, string path)
            {
                return new AbstractSyntaxTreeNode("Mock");
            }

            public AbstractSyntaxTreeNode CreateOrDefaultFromFile(DirectoryHandler handler, string path)
            {
                throw new NotImplementedException();
            }
        }

        private class MockAbstractSyntaxClassTreeExtractor : IAbstractSyntaxTreeClassExtractor
        {
            public AbstractSyntaxTreeNode Extract(AbstractSyntaxTreeNode root, string name)
            {
                return new AbstractSyntaxTreeNode("Mock");
            }

            public AbstractSyntaxTreeNode ExtractOrDefault(AbstractSyntaxTreeNode root, string name)
            {
                throw new NotImplementedException();
            }
        }


        private class MockSnapshotDateConverter : ISnapshotDateConverter
        {
            private readonly Regex _regex = new Regex(@"(Snapshot)(\d\d?)(-)(\d\d?)(-)(\d\d\d\d)(_)(\d\d?)(.)(\d\d?)(.)(\d\d?)(.)(\d\d?)");
            public bool CanConvert(string name)
            {
                return _regex.IsMatch(name);
            }

            public DateTime Convert(string name)
            {
                var match = _regex.Match(name);
                return new DateTime(
                    int.Parse(match.Groups[6].Value),
                    int.Parse(match.Groups[2].Value),
                    int.Parse(match.Groups[4].Value),
                    int.Parse(match.Groups[8].Value),
                    int.Parse(match.Groups[10].Value),
                    int.Parse(match.Groups[12].Value)
                );
            }
        }
    }
}
