using Ahedfi.Component.Core.Domain.Entities;
using Ahedfi.Component.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ahedfi.Component.Data.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : Entity, IAggregateRoot;
        Task CommitAsync();
    }
}
