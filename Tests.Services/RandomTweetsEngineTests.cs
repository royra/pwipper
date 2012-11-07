using System.Linq;
using NUnit.Framework;
using Pwipper.Core.Config;
using Pwipper.Services;

namespace Tests.Services
{
    [TestFixture]
    public class RandomTweetsEngineTests
    {
        [Test]
        public void AllTweetsAreDistinct()
        {
            var e = new RandomTweetsEngine(new AppConfig.RandomTweetsConfig {BatchSize = 3, TwitterAccount = "ladygaga"});
            Assert.That(e.GetRandomTweets().Take(10).Distinct().Count(), Is.EqualTo(10));
        }
    }
}
