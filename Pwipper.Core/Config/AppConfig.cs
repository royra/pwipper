using System.Xml.Serialization;

namespace Pwipper.Core.Config
{
    [XmlRoot("config")]
    public class AppConfig
    {
        [XmlAttribute("connectionString")]
        public string ConnectionString { get; set; }

        [XmlAttribute("recreateSchema")]
        public bool RecreateSchema { get; set; }

        [XmlAttribute("maxPwipsPerQuery")]
        public int MaxPwipsPerQuery { get; set; }

        [XmlElement("RandomTweets")]
        public RandomTweetsConfig RandomTweets { get; set; }

        public class RandomTweetsConfig
        {
            [XmlAttribute("batchSize")]
            public int BatchSize { get; set; }

            [XmlAttribute("twitterAccount")]
            public string TwitterAccount { get; set; }
        }

    }
}
