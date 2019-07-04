using System;
using HumanErrorProject.Engine.Analysis.AbstractSyntaxTree;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HumanErrorProject.Engine.Test.AnalysisTests.AbstractSnytaxTreeTests
{
    [TestClass]
    public class AbstractSyntaxTreeNodeTests
    {
        protected AbstractSyntaxTreeNode Root;
        protected AbstractSyntaxTreeNode Child;
        protected const string RootValue = "Root";
        protected const string ChildValue = "Child";

        [TestInitialize]
        public void Init()
        {
            Root = new AbstractSyntaxTreeNode(RootValue);
            Child = new AbstractSyntaxTreeNode(ChildValue);
        }

        [TestMethod]
        public void Constructor_ValueShouldEqualRoot()
        {
            Assert.AreEqual(RootValue, Root.Value);
        }

        [TestMethod]
        public void Constructor_HashTagShouldNotEqualZero()
        {
            Assert.AreNotEqual(0, Root.HashTag);
        }

        [TestMethod]
        public void Constructor_HeightShouldEqualOne()
        {
            Assert.AreEqual(1, Root.Height);
        }

        [TestMethod]
        public void Constructor_DegreeShouldEqualZero()
        {
            Assert.AreEqual(0, Root.Degrees);
        }

        [TestMethod]
        public void Constructor_IsTerminalShouldBeTrue()
        {
            Assert.IsTrue(Root.Terminal);
        }

        [TestMethod]
        public void Append_IsTerminalShouldBeFalse()
        {
            Root.Append(Child);
            Assert.IsFalse(Root.Terminal);
        }

        [TestMethod]
        public void Append_HashTagShouldChange()
        {
            var prior = Root.HashTag;

            Root.Append(Child);

            Assert.AreNotEqual(prior, Root.HashTag);
        }

        [TestMethod]
        public void Append_DegreesShouldEqualOne()
        {
            Root.Append(Child);

            Assert.AreEqual(1, Root.Degrees);
        }

        [TestMethod]
        public void Append_HeightShouldBeTwo()
        {
            Root.Append(Child);
            Assert.AreEqual(2, Root.Height);
        }

        [TestMethod]
        public void Prepend_IsTerminalShouldBeFalse()
        {
            Root.Prepend(Child);
            Assert.IsFalse(Root.Terminal);
        }

        [TestMethod]
        public void Prepend_HashTagShouldChange()
        {
            var prior = Root.HashTag;
            Root.Prepend(Child);

            Assert.AreNotEqual(prior, Root.HashTag);
        }

        [TestMethod]
        public void Prepend_DegressShouldEqualOne()
        {
            Root.Prepend(Child);
            Assert.AreEqual(1, Root.Degrees);
        }

        [TestMethod]
        public void Prepend_HeightShouldBeTwo()
        {
            Root.Prepend(Child);
            Assert.AreEqual(2, Root.Height);
        }

        [TestMethod]
        public void EqualOperator_ShouldBeTrueForSameValue()
        {
            var lhs = new AbstractSyntaxTreeNode(RootValue);
            var rhs = new AbstractSyntaxTreeNode(RootValue);

            Assert.IsTrue(lhs == rhs);
        }

        [TestMethod]
        public void NotEqualOperator_ShouldBeFalseForDifferentValue()
        {
            var lhs = new AbstractSyntaxTreeNode(RootValue);
            var rhs = new AbstractSyntaxTreeNode(ChildValue);

            Assert.IsTrue(lhs != rhs);
        }

        [TestMethod]
        public void Remove_ShouldSetRootTerminalToTrue()
        {
            Root.Append(Child);
            Root.Remove(Child);

            Assert.IsTrue(Root.Terminal);
        }

        [TestMethod]
        public void Remove_ShouldSetHeightToZero()
        {
            Root.Append(Child);
            Root.Remove(Child);

            Assert.AreEqual(1, Root.Height);
        }

        [TestMethod]
        public void Remove_ShouldUpdateHashTag()
        {
            Root.Append(Child);
            var prior = Root.HashTag;

            Root.Remove(Child);

            Assert.AreNotEqual(prior, Root.HashTag);
        }

        [TestMethod]
        public void InsertAt_ShouldSetTerminalFalse()
        {
            Root.InsertAt(0, Child);
            Assert.IsFalse(Root.Terminal);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void InsertAt_ShouldThrowExpectionForInsertAt100()
        {
            Root.InsertAt(100, Child);
        }

        [TestMethod]
        public void RemoveAt_ShouldSetTerminalToTrue()
        {
            Root.Append(Child);

            Root.RemoveAt(0);

            Assert.IsTrue(Root.Terminal);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void RemoveAt_ShouldThrowExceptionForRemoveAtNegative()
        {
            Root.RemoveAt(-1);
        }

        
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void RemoveAt_ShouldThrowExceptionForRemoveAt100()
        {
            Root.RemoveAt(100);
        }
    }
}
