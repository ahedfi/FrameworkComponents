using Ahedfi.Component.Core.Domain.Entities;
using Ahedfi.Component.Core.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ahedfi.Component.Data.Domain.Interfaces
{
    public interface IAsyncRepository<TEntity> where TEntity : Entity, IAggregateRoot
    {
        Task<TEntity> GetByIdAsync(int id);
        Task<IReadOnlyList<TEntity>> ListAllAsync();
        Task<IReadOnlyList<TEntity>> ListAsync(ISpecification<TEntity> spec);
        Task<TEntity> AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task<int> CountAsync(ISpecification<TEntity> spec);
    }
}
