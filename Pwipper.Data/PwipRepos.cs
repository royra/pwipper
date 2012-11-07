using System;
using System.Collections.Generic;
using NHibernate;
using Pwipper.Core.DataInterfaces;
using Pwipper.Core.Domain;

namespace Pwipper.Data
{
    public class PwipRepos : DomainObjectReposBase<Pwip, long>, IPwipRepos
    {
        public PwipRepos(Lazy<ISession> session) : base(session)
        {
        }

        public IList<Pwip> GetNewFrom(int maxResults, long? lastId = null)
        {
            var queryOver = NHibSession.QueryOver<Pwip>().OrderBy(p => p.Time).Desc;
            if (lastId != null)
                queryOver = queryOver.Where(p => p.Id < lastId.Value);

            return queryOver.Take(maxResults).List();
        }
    }
}
