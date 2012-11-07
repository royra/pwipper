using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using NUnit.Framework;
using Pwipper.Core.DataInterfaces;
using Pwipper.Core.Domain;

namespace Pwipper.Tests.Data
{
    [TestFixture]
    public class PwipReposTests : DbTransactionTest
    {
        private List<long> savedIds;
        private const int NUM_PWIPS = 10;
        private IPwipRepos repos;

        public override void SetUp()
        {
            base.SetUp();

            repos = TestScope.Resolve<IPwipRepos>();
            var user = new User {Email = "test@example.org", Name = "Test Icle"};
            TestSession.Save(user);
            
            savedIds = new List<long>();
            var startTime = DateTime.Now.AddMinutes(-(NUM_PWIPS+1));
            for(var i=0;i<NUM_PWIPS;++i)
            {
                var pwip = new Pwip {Author = user, Text = string.Format("pwip number {0}", i), Time = startTime.AddMinutes(i)};
                repos.Save(pwip);
                savedIds.Add(pwip.Id);
            }

            TestSession.Flush();
        }

        [Test]
        public void GetFrom_ReturnsAll()
        {
            var resultObjects = repos.GetNewFrom(savedIds.Count);
            Assert.That(resultObjects, Is.Ordered.By("Time").Descending);

            var result1 = resultObjects.Select(p => p.Id);
            Assert.That(result1, Is.EquivalentTo(savedIds));

            var result2 = repos.GetNewFrom(savedIds.Count + 1).Select(p => p.Id);
            Assert.That(result2, Is.EquivalentTo(result1));
        }

        [Test]
        public void GetFrom_Max()
        {
            savedIds = savedIds.OrderByDescending(i => i).ToList();

            Assert.That(repos.GetNewFrom(1).Select(p => p.Id), Is.EquivalentTo(savedIds.Take(1)));
            Assert.That(repos.GetNewFrom(2).Select(p => p.Id), Is.EquivalentTo(savedIds.Take(2)));
        }

        [Test]
        public void GetFrom_StartId()
        {
            savedIds = savedIds.OrderByDescending(i => i).ToList();

            var firstTwo = repos.GetNewFrom(2).Select(p => p.Id);
            Assert.That(firstTwo, Is.EquivalentTo(savedIds.Take(2)));
            var nextTwo = repos.GetNewFrom(2, firstTwo.Min()).Select(p => p.Id);
            Assert.That(nextTwo, Is.EquivalentTo(savedIds.Skip(2).Take(2)));
        }
    }
}
