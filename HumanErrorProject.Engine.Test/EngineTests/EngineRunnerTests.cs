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
using HumanErrorProject.Engine.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HumanErrorProject.Engine.Test.EngineTests
{
    [TestClass]
    public class EngineRunnerTests
    {
        protected EngineRunner Runner;
        protected IRepository<Student, int> Students;
        protected IRepository<Survey, string> Surveys;
        protected const string StudentName = "Student";
        protected const int StudentId = 1;
        protected const string ClassName = "Class";
        protected const int ClassId = 1;
        protected string Root;
        protected StudentSubmissionDto Submission;
        protected MockSnapshotGenerator SnapshotGenerator;

        [TestInitialize]
        public void Init()
        {
            Students = new MockRepository<Student, int>(
                new List<Student>()
                {
                    new Student()
                    {
                        Id = StudentId,
                        Name = StudentName,
                        StudentCourseClasses = new List<StudentCourseClass>()
                        {
                            new StudentCourseClass()
                            {
                                Class = new CourseClass()
                                {
                                    Id = ClassId,
                                    Name = ClassName,
                                }
                            }
                        }
                    }
                });

            Surveys = new MockRepository<Survey, string>();
            SnapshotGenerator = new MockSnapshotGenerator();
            Runner = new EngineRunner(Microsoft.Extensions.Options.Options.Create(
                new EngineRunnerOptions()
                {
                    SurveyUrl = "Blank",
                }), Students, Surveys,
                SnapshotGenerator , new MockAssignmentGenerator(), new MockMarkovModelGenerator());
            Root = Path.Combine(Directory.GetCurrentDirectory(), nameof(EngineRunnerTests));

            Submission = new StudentSubmissionDto()
            {
                SnapshotFolder = MockSnapshots.GetCalculatorSnapshots(),
            };
        }

        [TestMethod]
        public async Task GetStudent_ShouldGetStudentInRepository()
        {
            Submission.ClassName = ClassName;
            Submission.StudentName = StudentName;
            using (var data = new SubmissionData(Submission, Root))
            {
                var student = await Runner.GetStudent(data);
                Assert.AreEqual(StudentId, student.Id);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(EngineExceptionData))]
        public async Task GetStudent_ShouldThrowForStudentNotInRepository()
        {
            Submission.StudentName = "Random Student";
            Submission.ClassName = ClassName;
            using (var data = new SubmissionData(Submission, Root))
            {
                await Runner.GetStudent(data);
            }
        }

        [TestMethod]
        public async Task GetCourseClass_ShouldGetClassInStudentClasses()
        {
            Submission.StudentName = StudentName;
            Submission.ClassName = ClassName;
            using (var data = new SubmissionData(Submission, Root))
            {
                var student = await Students.Get(StudentId);
                var course = await Runner.GetCourseClass(student, data);
                Assert.AreEqual(ClassId, course.Id);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(EngineExceptionData))]
        public async Task GetCourseClass_ShouldThrowForRandomClass()
        {
            Submission.StudentName = StudentName;
            Submission.ClassName = "Random Class";
            using (var data = new SubmissionData(Submission, Root))
            {
                var student = await Students.Get(StudentId);
                await Runner.GetCourseClass(student, data);
            }
        }

        [TestMethod]
        public void HasSnapshotsToReport_ShouldPassForOneSnapshot()
        {
            var input = new List<List<Snapshot>>()
            {
                new List<Snapshot>()
                {
                    new Snapshot()
                }
            };
            Assert.IsTrue(Runner.HasSnapshotsToReport(input));
        }

        [TestMethod]
        public void HasSnapshotsToReport_ShouldFailForNoSnapshot()
        {
            var input = new List<List<Snapshot>>()
            {
                new List<Snapshot>()
            };
            Assert.IsFalse(Runner.HasSnapshotsToReport(input));
        }

        [TestMethod]
        public void HasSnapshotsToReport_ShouldFailForMultiListWithNoSnapshot()
        {
            var input = new List<List<Snapshot>>()
            {
                new List<Snapshot>(),
                new List<Snapshot>(),
                new List<Snapshot>(),
            };
            Assert.IsFalse(Runner.HasSnapshotsToReport(input));
        }

        [TestMethod]
        public void HasSnapshotsToReport_ShouldPassForMultiListWithTwoSnapshots()
        {
            var input = new List<List<Snapshot>>()
            {
                new List<Snapshot>(),
                new List<Snapshot>(),
                new List<Snapshot>(),
                new List<Snapshot>()
                {
                    new Snapshot(),
                    new Snapshot(),
                }
            };
            Assert.IsTrue(Runner.HasSnapshotsToReport(input));
        }

        [TestMethod]
        public async Task GenerateSurvey_ShouldAddToRepository()
        {
            var expected = (await Surveys.GetAll()).Count() + 1;

            Submission.StudentName = StudentName;
            Submission.ClassName = ClassName;
            using (var data = new SubmissionData(Submission, Root))
            {
                var input = new List<List<Snapshot>>()
                {
                    new List<Snapshot>()
                    {
                        new Snapshot()
                        {
                        }
                    }
                };
                await Runner.GenerateSurvey(data, input);
                var actual = (await Surveys.GetAll()).Count();
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public async Task GenerateSurvey_ShouldHaveId()
        {
            Submission.StudentName = StudentName;
            Submission.ClassName = ClassName;
            using (var data = new SubmissionData(Submission, Root))
            {
                var input = new List<List<Snapshot>>()
                {
                    new List<Snapshot>()
                    {
                        new Snapshot()
                        {
                        }
                    }
                };
                var survey = await Runner.GenerateSurvey(data, input);
                Assert.AreNotEqual("", survey.Id);
            }
        }


        [TestCleanup]
        public void Cleanup()
        {
            if (Directory.Exists(Root))
            {
                Directory.Delete(Root);
            }
        }


        public class MockSnapshotGenerator : ISnapshotGenerator
        {
            public Task<IList<Snapshot>> Generate(SubmissionData data, Assignment assignment)
            {
                return Task.FromResult<IList<Snapshot>>(new List<Snapshot>());
            }
        }

        public class MockAssignmentGenerator : IAssignmentGenerator
        {
            public Task Generate(PreAssignment assignment, DirectoryHandler handler)
            {
                return Task.CompletedTask;
            }

        }

        public class MockMarkovModelGenerator : IMarkovModelGenerator
        {
            public Task Generate(IList<Snapshot> snapshots, MarkovModelOptions options, DirectoryHandler handler, Assignment assignment)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
