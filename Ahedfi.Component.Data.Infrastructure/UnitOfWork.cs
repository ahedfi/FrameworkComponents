using Ahedfi.Component.Core.Domain.Models.Entities;
using Ahedfi.Component.Core.Domain.Models.Interfaces;
using Ahedfi.Component.Data.Domain.Entities;
using Ahedfi.Component.Data.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ahedfi.Component.Data.Infrastructure
{
    public abstract class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
    {
        protected readonly TContext _context;
        private bool disposed = false;
        private Dictionary<Type, object> _repositories;

        public UnitOfWork(TContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public TContext DbContext => _context;

        public IRepository<TEntity> Repository<TEntity>() where TEntity : Entity, IAggregateRoot
        {
            if (_repositories == null)
            {
                _repositories = new Dictionary<Type, object>();
            }

            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new EfRepository<TEntity>(_context);
            }

            return (IRepository<TEntity>)_repositories[type];
        }

        public async Task CommitAsync(string username)
        {
            TrackEntities(username);
            await _context.SaveChangesAsync();
        }

        private void TrackEntities(string username)
        {
            var entries = GetEntitiesState();
            var batchId = Guid.NewGuid();
            foreach (var entityEntry in entries.ToList())
            {
                AddToAuditTrails(entityEntry, username, batchId);
                SetAudit(username, entityEntry);
            }
        }
        private IEnumerable<EntityEntry> GetEntitiesState()
        {
            return _context.ChangeTracker
                                .Entries()
                                .Where(e => e.Entity is IAuditable &&
                                            (e.State == EntityState.Added ||
                                            e.State == EntityState.Modified ||
                                            e.State == EntityState.Deleted)
                                       );
        }

        private void AddToAuditTrails(EntityEntry entry, string username, Guid guid)
        {
            foreach (var property in entry.Properties)
            {
                if (property.OriginalValue == property.CurrentValue)
                    continue;

                var auditEntry = new AuditTrail
                {
                    ObjectName = entry.Entity.GetType().Name,
                    ObjectId = ((dynamic)entry.Entity).Id.ToString(),
                    PropertyName = property.Metadata.Name,
                    OldValue = property.OriginalValue != null ? property.OriginalValue.ToString() : string.Empty,
                    NewValue = property.CurrentValue != null ? property.CurrentValue.ToString() : string.Empty,
                    Date = DateTime.Now,
                    UserName = username,
                    ChangeType = entry.State.ToString(),
                    BatchId = guid
                };
                Repository<AuditTrail>().AddAsync(auditEntry);
            }
        }
        private void SetAudit(string username, EntityEntry entityEntry)
        {
            if (entityEntry.Entity is IAuditable)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    ((IAuditable)entityEntry.Entity).CreatedOn = DateTime.Now;
                    ((IAuditable)entityEntry.Entity).CreatedBy = username;
                }
                else
                {
                    ((IAuditable)entityEntry.Entity).UpdatedOn = DateTime.Now;
                    ((IAuditable)entityEntry.Entity).UpdatedBy = username;
                }
            }
        }

       
      
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _repositories.Clear();
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
