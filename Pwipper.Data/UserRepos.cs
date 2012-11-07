using System;
using NHibernate;
using Pwipper.Core.DataInterfaces;
using Pwipper.Core.Domain;

namespace Pwipper.Data
{
    public class UserRepos : DomainObjectReposBase<User, long>, IUserRepos
    {
        public UserRepos(Lazy<ISession> session)
            : base(session)
        {
        }
    }
}