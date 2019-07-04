using System;
using HumanErrorProject.Engine.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HumanErrorProject.Engine.Test.UtilitiesTests
{
    [TestClass]
    public class SnapshotDateConverterTests
    {
        protected ISnapshotDateConverter Converter;

        [TestInitialize]
        public void Init()
        {
            Converter = new SnapshotDateConverter();
        }

        [TestMethod]
        public void CanConvert_ShouldPassWithValidSnapshot()
        {
            Assert.IsTrue(Converter.CanConvert("Snapshot12-20-2018_12.00.47.15"));
        }

        [TestMethod]
        public void CanConvert_ShouldFailWithInvalidSnapshot()
        {
            Assert.IsFalse(Converter.CanConvert("Fail12-20-2018_12.00.47.15"));
        }

        [TestMethod]
        public void Convert_ShouldPassWithValidSnapshot()
        {
            var time = Converter.Convert("Snapshot12-20-2018_12.00.47.15");
            Assert.IsNotNull(time);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Convert_ShouldThrowWithInValidSnapshot()
        {
            Converter.Convert("Fail12-20-2018_12.00.47.15");
        }
    }
}
