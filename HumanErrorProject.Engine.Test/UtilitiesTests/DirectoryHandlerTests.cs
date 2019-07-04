using System.IO;
using System.Linq;
using HumanErrorProject.Engine.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HumanErrorProject.Engine.Test.UtilitiesTests
{
    [TestClass]
    public class DirectoryHandlerTests
    {
        protected string Directory;

        [TestInitialize]
        public void Init()
        {
            Directory = Path.Combine(System.IO.Directory.GetCurrentDirectory(), nameof(DirectoryHandlerTests));
        }

        [TestMethod]
        public void ShouldCreateDirectory()
        {
            using (var directory = new DirectoryHandler(Directory))
            {
                Assert.IsTrue(System.IO.Directory.Exists(Directory));
            }
        }

        [TestMethod]
        public void ShouldDestoryDirectory()
        {
            using (var directory = new DirectoryHandler(Directory))
            {
            }
            Assert.IsFalse(System.IO.Directory.Exists(Directory));
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (System.IO.Directory.Exists(Directory))
            {
                System.IO.Directory.Delete(Directory, true);
            }
        }
    }
}
