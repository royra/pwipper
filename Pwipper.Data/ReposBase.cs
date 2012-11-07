using System;
using NHibernate;
using Pwipper.Core.DataInterfaces;

namespace Pwipper.Data
{
    public abstract class ReposBase : IRepos
    {
        private readonly Lazy<ISession> session;

        public ISession NHibSession
        {
            get { return session.Value; }
        }

        protected ReposBase(Lazy<ISession> session)
        {
            this.session = session;
        }
    }
}