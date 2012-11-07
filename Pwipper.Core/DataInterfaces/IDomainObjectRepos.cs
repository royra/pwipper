using System.Collections.Generic;
using Pwipper.Core.Domain;

namespace Pwipper.Core.DataInterfaces
{
    public interface IDomainObjectRepos<T, in TId> where T : DomainObject<TId>
    {
        IList<T> GetAll();
        T Save(T obj);
        T Get(TId id);
        void Update(T obj);
        void Delete(T obj);
        bool Delete(TId id);
    }

    public interface IUserRepos : IDomainObjectRepos<User, long>
    {
    }

    public interface IPwipRepos : IDomainObjectRepos<Pwip, long>
    {
        IList<Pwip> GetNewFrom(int maxResults, long? lastId = null);
    }
}