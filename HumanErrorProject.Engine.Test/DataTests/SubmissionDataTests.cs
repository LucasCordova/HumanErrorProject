using System.IO;
using System.Linq;
using System.Threading;
using HumanErrorProject.Data.Dtos;
using HumanErrorProject.Engine.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HumanErrorProject.Engine.Test.DataTests
{
    [TestClass]
    public class SubmissionDataTests
    {
        protected StudentSubmissionDto Submission;
        protected const string StudentName = "StudentName";
        protected const string ClassName = "ClassName";
        protected string Root;

        [TestInitialize]
        public void Init()
        {
            Submission = new StudentSubmissionDto()
            {
                StudentName = StudentName,
                ClassName = ClassName,
                SnapshotFolder = MockSnapshots.GetCalculatorSnapshots(),
            };

            Root = Path.Combine(Directory.GetCurrentDirectory(), nameof(SubmissionDataTests));
        }

        [TestMethod]
        public void Dispose_CheckRootFolderIsCreated()
        {
            using (var data = new SubmissionData(Submission, Root))
            {
                Assert.IsTrue(Directory.Exists(Root));
            }
        }

        [TestMethod]
        public void Dispose_CheckRootFolderIsRemove()
        {
            using (var data = new SubmissionData(Submission, Root))
            {
            }
            Assert.IsFalse(Directory.Exists(Root));
        }

        [TestMethod]
        public void SnapshotFolderNames_CheckThatSnapshotExists()
        {
            using (var data = new SubmissionData(Submission, Root))
            {
                var folder = data.SnapshotFolderNames();
                Assert.IsTrue(folder.Any());
            }
        }
        

        [TestMethod]
        public void HasSourceFile_ShouldPassForEachSnapshot()
        {
            using (var data = new SubmissionData(Submission, Root))
            {
                foreach (var folder in data.SnapshotFolderNames())
                {
                    Assert.IsTrue(data.HasSourceFile(folder,
                        MockSnapshots.GetCalculatorFile()));
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void HasSourceFile_ShouldThrowExceptionForBadSnapshotName()
        {
            using (var data = new SubmissionData(Submission, Root))
            {
                data.HasSourceFile("Random", MockSnapshots.GetCalculatorFile());
            }
        }

        [TestMethod]
        public void HasSourceFile_ShouldFailForNonFileName()
        {
            using (var data = new SubmissionData(Submission, Root))
            {
                var first = data.SnapshotFolderNames().First();
                Assert.IsFalse(data.HasSourceFile(first, "Random.hpp"));
            }
        }

        [TestMethod]
        public void SnapshotSourceFiles_ShouldReturnFilesWithFullUrl()
        {
            using (var data = new SubmissionData(Submission, Root))
            {
                foreach (var snapshot in data.SnapshotFolderNames())
                {
                    foreach (var file in data.SnapshotSourceFiles(snapshot))
                    {
                        Assert.IsTrue(file.Length > Root.Length);
                    }
                }
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

    }
}
