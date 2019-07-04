using System.IO;
using HumanErrorProject.Engine.Data;
using HumanErrorProject.Engine.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HumanErrorProject.Engine.Test.UtilitiesTests
{
    [TestClass]
    public class EngineProcessTests
    {

        [TestMethod]
        public void Run_GoodClangHelpShouldReturnExitCodeOfOne()
        {
            var process = new EngineProcess(
                new EngineProcessData()
                {
                    WaitForExit = 10000,
                    Command = "clang++",
                    Arguments = "--help",
                    WorkingDirectory = Directory.GetCurrentDirectory()
                });
            var code = process.Run();
            Assert.AreEqual(0, code);
        }

        [TestMethod]
        public void Run_IncorrectClangHelpShouldNotReutrnExitCodeOfOne()
        {
            var process = new EngineProcess(
                new EngineProcessData()
                {
                    WaitForExit = 10000,
                    Command = "clang++",
                    Arguments = "help",
                    WorkingDirectory = Directory.GetCurrentDirectory()
                });
            var code = process.Run();
            Assert.AreNotEqual(0, code);
        }

        [TestMethod]
        public void Run_IncorrectClangHelpShouldHaveErrorOutput()
        {
            var process = new EngineProcess(
                new EngineProcessData()
                {
                    WaitForExit = 10000,
                    Command = "clang++",
                    Arguments = "help",
                    WorkingDirectory = Directory.GetCurrentDirectory()
                });
            process.Run();
            var error = process.StandardError;
            Assert.IsFalse(error.EndOfStream);
        }

    }
}
