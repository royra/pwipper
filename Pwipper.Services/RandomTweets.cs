using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using Newtonsoft.Json.Linq;
using Pwipper.Core.Config;

namespace Pwipper.Services
{
    public interface IRandomTweetsEngine
    {
        IEnumerable<Tweet> GetRandomTweets();
    }

    public class Tweet
    {
        public string Text { get; set; }
        public DateTime? Time { get; set; }
    }

    public class RandomTweetsEngine : IRandomTweetsEngine
    {
        public int BatchCount { get; set; }
        public string TwitterAccount { get; set; }
        
        public RandomTweetsEngine(AppConfig appConfig)
            : this(appConfig.RandomTweets)
        {
        }

        public RandomTweetsEngine(AppConfig.RandomTweetsConfig config)
        {
            BatchCount = config.BatchSize;
            TwitterAccount = config.TwitterAccount;
        }

        public IEnumerable<Tweet> GetRandomTweets()
        {
            long currentId = 0;

            while (true)
            {
                using (var wc = new WebClient())
                {
                    var url = string.Format(
                        "https://api.twitter.com/1/statuses/user_timeline/{0}.json?count={1}&include_rts=0{2}",
                        TwitterAccount,
                        BatchCount,
                        currentId != 0 ? string.Format("&max_id={0}", currentId-1) : "");

                    var response = wc.DownloadString(url);
                    var ar = JArray.Parse(response);
                    //
                    var texts = ar.Select(o =>
                                              {
                                                  var text = (string) o["text"];
                                                  var timeStr = (string) o["created_at"];
                                                  var time = string.IsNullOrEmpty(timeStr)
                                                      ? null
                                                      : (DateTime?)DateTime.ParseExact(timeStr, "ddd MMM dd HH:mm:ss zzz yyyy", CultureInfo.InvariantCulture);

                                                  return new Tweet {Text = text, Time = time};
                                              }
                        );
                    
                    foreach (var t in texts)
                        yield return t;

                    currentId = (long)ar[ar.Count - 1]["id"];
                }
            }
        }
    }
}
