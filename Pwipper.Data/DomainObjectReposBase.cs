using System;
using System.Collections.Generic;
using NHibernate;
using Pwipper.Core.DataInterfaces;
using Pwipper.Core.Domain;

namespace Pwipper.Data
{
    public abstract class DomainObjectReposBase<T, TId> : ReposBase, IDomainObjectRepos<T, TId>
        where T : DomainObject<TId>
    {
        protected DomainObjectReposBase(Lazy<ISession> session)
            : base(session)
        {
        }

        public virtual IList<T> GetAll()
        {
            return NHibSession.QueryOver<T>().List<T>();
        }

        public virtual T Save(T obj)
        {
            NHibSession.Save(obj);
            return obj;
        }

        public virtual T Get(TId id)
        {
            return NHibSession.Get<T>(id);
        }

        public virtual void Update(T obj)
        {
            NHibSession.Update(obj);
        }

        public virtual void Delete(T obj)
        {
            NHibSession.Delete(obj);
        }

        public virtual bool Delete(TId id)
        {
            var queryString = string.Format("delete from {0} where id=:id",
                                            NHibSession.SessionFactory.GetClassMetadata(typeof (T)).EntityName);

            var query = NHibSession.CreateQuery(queryString).SetParameter("id", id);

            var result = query.ExecuteUpdate();

            return result != 0;
        }
    }
}