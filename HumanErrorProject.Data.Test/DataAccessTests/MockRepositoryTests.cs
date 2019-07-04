using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanErrorProject.Data.DataAccess;
using HumanErrorProject.Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HumanErrorProject.Data.Test.DataAccessTests
{
    [TestClass]
    public class MockRepositoryTests
    {
        protected IRepository<AbstractSyntaxTreeMetric, int> Repository;
        protected IList<AbstractSyntaxTreeMetric> OriginalAbstractSyntaxTreeMetrics;

        [TestInitialize]
        public void Init()
        {
            OriginalAbstractSyntaxTreeMetrics = new List<AbstractSyntaxTreeMetric>()
            {
                new AbstractSyntaxTreeMetric()
                {
                    Id = 1,
                    Rotations = 12,
                    Deletions = 8,
                    Insertations = 13
                },
                new AbstractSyntaxTreeMetric()
                {
                    Id = 2,
                    Rotations = 12,
                    Deletions = 8,
                    Insertations = 13,
                },
                new AbstractSyntaxTreeMetric()
                {
                    Id = 3,
                    Rotations = 2,
                    Deletions = 9,
                    Insertations = 45,
                }
            };

            Repository = new MockRepository<AbstractSyntaxTreeMetric, int>(OriginalAbstractSyntaxTreeMetrics);
        }

        [TestMethod]
        public async Task GetById_ShouldReturnTree()
        {
            var expected = OriginalAbstractSyntaxTreeMetrics[0];

            var actual = await Repository.Get(expected.Id);

            Assert.IsNotNull(actual);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task GetById_ShouldThrow()
        {
            await Repository.Get(100);
        }

        [TestMethod]
        public async Task GetAll_ShouldMatchOriginalList()
        {
            var expected = OriginalAbstractSyntaxTreeMetrics.Count;

            var actual = (await Repository.GetAll()).Count();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task Find_ShouldGetAllWithInsertationsThatMatchFirst()
        {
            var input = OriginalAbstractSyntaxTreeMetrics[0].Insertations;

            var expected = OriginalAbstractSyntaxTreeMetrics.Count(e => e.Insertations.Equals(input));
            var actual = (await Repository.Find(e => e.Insertations.Equals(input))).Count();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task Find_ShouldGetNoneForNegativeInsertations()
        {
            var actual = await Repository.Find(e => e.Insertations.Equals(-1));

            Assert.AreEqual(0, actual.Count());
        }

        [TestMethod]
        public async Task SingleOrDefault_ShouldGetTheFirstById()
        {
            var expected = OriginalAbstractSyntaxTreeMetrics[0].Id;

            var actual = await Repository.SingleOrDefault(e => e.Id.Equals(expected));

            Assert.IsNotNull(actual);
        }

        [TestMethod]
        public async Task SingleOrDefault_ShouldReturnNullForNegativeOne()
        {
            var actual = await Repository.SingleOrDefault(e => e.Id.Equals(-1));

            Assert.IsNull(actual);
        }

        [TestMethod]
        public async Task Add_ShouldIncreaseItemsByOne()
        {
            var expected = OriginalAbstractSyntaxTreeMetrics.Count + 1;

            await Repository.Add(new AbstractSyntaxTreeMetric()
            {
                Insertations = 1,
                Deletions = 4,
                Rotations = 45,
            });

            var actual = (await Repository.GetAll()).Count();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task AddRange_ShouldIncreaseItemsByTwo()
        {
            var expected = OriginalAbstractSyntaxTreeMetrics.Count + 2;

            await Repository.AddRange(new List<AbstractSyntaxTreeMetric>()
            {
                new AbstractSyntaxTreeMetric()
                {
                    Insertations = 4,
                    Deletions = 4,
                    Rotations = 12
                },
                new AbstractSyntaxTreeMetric()
                {
                    Insertations = 32,
                    Deletions = 4,
                    Rotations = 3
                }
            });

            var actual = (await Repository.GetAll()).Count();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task Remove_ShouldDecreaseItemsByOne()
        {
            var expected = OriginalAbstractSyntaxTreeMetrics.Count - 1;

            var input = OriginalAbstractSyntaxTreeMetrics[0];

            await Repository.Remove(input);

            var actual = (await Repository.GetAll()).Count();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task RemoveRange_ShouldDecreaseItemsByTwo()
        {
            var expected = OriginalAbstractSyntaxTreeMetrics.Count - 2;

            await Repository.RemoveRange(new List<AbstractSyntaxTreeMetric>()
            {
                OriginalAbstractSyntaxTreeMetrics[0],
                OriginalAbstractSyntaxTreeMetrics[1]
            });

            var actual = (await Repository.GetAll()).Count();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task Update_ShouldChangeInsertationsByTwo()
        {
            var expected = OriginalAbstractSyntaxTreeMetrics[0];
            expected.Insertations += 4;

            await Repository.Update(expected);

            var actual = (await Repository.Get(expected.Id));

            Assert.AreEqual(expected.Insertations, actual.Insertations);
        }
    }
}
