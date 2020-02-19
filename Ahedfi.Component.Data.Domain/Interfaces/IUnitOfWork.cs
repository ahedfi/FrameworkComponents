using Ahedfi.Component.Core.Domain.Models.Entities;
using Ahedfi.Component.Core.Domain.Models.Interfaces;
using System;
using System.Threading.Tasks;

namespace Ahedfi.Component.Data.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : Entity, IAggregateRoot;
        Task CommitAsync(string username);
    }
}
