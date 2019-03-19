using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Radio.Core.Domain
{
    public interface IRepository<T>
        where T : EntityBase
    {
        IEnumerable<T> Get();

        T GetById(Guid id);

        Task<T> GetByIdAsync(Guid id);

        T GetByIdOrDefault(Guid id);

        Task<T> GetByIdOrDefaultAsync(Guid id);

        T Create();

        void Add(T entity);

        void Remove(T entity);
    }
}
