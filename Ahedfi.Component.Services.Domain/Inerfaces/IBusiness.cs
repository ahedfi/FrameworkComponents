using Ahedfi.Component.Core.Domain.Models.Interfaces;
using Ahedfi.Component.Core.Domain.Security.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ahedfi.Component.Services.Domain.Inerfaces
{
    public interface IBusiness<TEntity> where TEntity : IEntity, IAggregateRoot
    {
        Task<TEntity> SaveAsync(IUserIdentity user, TEntity Entity);
        Task DeleteAsync(IUserIdentity user, TEntity Entity);
        Task<TEntity> FindFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> FindAllAsync();
    }
}
