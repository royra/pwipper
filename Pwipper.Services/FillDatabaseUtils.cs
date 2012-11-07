using System;
using System.IO;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using Pwipper.Core.Domain;

namespace Pwipper.Services
{
    public class FillDatabaseUtils
    {
        private readonly ISession session;
        private readonly Configuration nhConfig;
        private readonly IRandomTweetsEngine randomTweetsEngine;

        public FillDatabaseUtils(ISession session, Configuration nhConfig, IRandomTweetsEngine randomTweetsEngine)
        {
            this.session = session;
            this.nhConfig = nhConfig;
            this.randomTweetsEngine = randomTweetsEngine;
        }

        public void EnsureUser()
        {
            if (session.QueryOver<User>().RowCount() == 0)
            {
                var user = new User {Email = "bjames@mi6.gov.uk", Name = "James Bond"};
                session.Save(user);
            }

            session.Flush();
        }

        public void RecreateSchema()
        {
            var export = new SchemaExport(nhConfig);
            var sb = new StringBuilder();
            using (var output = new StringWriter(sb))
                export.Execute(false, true, false, session.Connection, output);
        }

        private static readonly DateTime MIN_DATE = new DateTime(1950, 1, 1);

        public void EnsurePosts(int num)
        {
            var existingPosts = session.QueryOver<Pwip>().RowCount();
            if (existingPosts >= num) return;

            var user = session.QueryOver<User>().Take(1).SingleOrDefault();
            if (user == null)
                throw new Exception("no user in database. call EnsureUser first");

            var pwips = randomTweetsEngine.GetRandomTweets().Take(num - existingPosts)
                .Select(t => new Pwip {Author = user, Time = t.Time ?? MIN_DATE, Text = t.Text})
                .OrderBy(t => t.Time);

            foreach (var pwip in pwips)
                session.Save(pwip);

            session.Flush();
        }
    }
}
