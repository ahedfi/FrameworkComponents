using Ahedfi.Component.Core.Domain.Models.Interfaces;
using Ahedfi.Component.Data.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ahedfi.Component.Data.Infrastructure
{
    internal static class AuditService
    {
        internal static IEnumerable<AuditTrail> TrackEntities(string username, DbContext context)
        {
            var audits = new List<AuditTrail>();
            var entries = GetEntitiesState(context);
            var batchId = Guid.NewGuid();
            foreach (var entityEntry in entries.ToList())
            {
                AddToAuditTrails(entityEntry, username, batchId, audits);
                SetAudit(username, entityEntry);
            }
            return audits;
        }
        private static void AddToAuditTrails(EntityEntry entry, string username, Guid guid, ICollection<AuditTrail> audits)
        {
            var tableName = entry.Entity.GetType().Name;
            var pk = GetPrimaryKeys(entry);
            if (entry.State == EntityState.Added)
            {
                foreach (var property in entry.Properties)
                {
                    if (property.IsTemporary || property.CurrentValue == null)
                    {
                        continue;
                    }

                    var auditEntry = new AuditTrail
                    {
                        ObjectName = tableName,
                        ObjectId = pk,
                        PropertyName = property.Metadata.Name,
                        OldValue = string.Empty,
                        NewValue = property.CurrentValue != null ? property.CurrentValue.ToString() : string.Empty,
                        Date = DateTime.Now,
                        UserName = username,
                        ChangeType = entry.State.ToString(),
                        BatchId = guid
                    };

                    audits.Add(auditEntry);
                }
            }
            else if (entry.State == EntityState.Modified)
            {
                foreach (var property in entry.Properties)
                {
                    if (!property.IsModified || Equals(property.OriginalValue, property.CurrentValue))
                        continue;

                    var auditEntry = new AuditTrail
                    {
                        ObjectName = tableName,
                        ObjectId = pk,
                        PropertyName = property.Metadata.Name,
                        OldValue = property.OriginalValue != null ? property.OriginalValue.ToString() : string.Empty,
                        NewValue = property.CurrentValue != null ? property.CurrentValue.ToString() : string.Empty,
                        Date = DateTime.Now,
                        UserName = username,
                        ChangeType = entry.State.ToString(),
                        BatchId = guid
                    };

                    audits.Add(auditEntry);
                }
            }
            else if (entry.State == EntityState.Deleted)
            {
                foreach (var property in entry.Properties)
                {
                    var auditEntry = new AuditTrail
                    {
                        ObjectName = tableName,
                        ObjectId = pk,
                        PropertyName = property.Metadata.Name,
                        OldValue = property.OriginalValue != null ? property.OriginalValue.ToString() : string.Empty,
                        NewValue = string.Empty,
                        Date = DateTime.Now,
                        UserName = username,

                        ChangeType = entry.State.ToString(),
                        BatchId = guid
                    };
                    audits.Add(auditEntry);
                }
            }
        }
        private static string GetPrimaryKeys(EntityEntry entry)
        {
            string pk = string.Empty;
            if (!entry.IsKeySet) return "N/A";
            return entry.Metadata.FindPrimaryKey()
                                     .Properties
                                     .Select(p => entry.Property(p.Name).CurrentValue.ToString()
                                     ).FirstOrDefault().ToString();
        }
        private static void SetAudit(string username, EntityEntry entityEntry)
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
        private static IEnumerable<EntityEntry> GetEntitiesState(DbContext context)
        {
            return context.ChangeTracker
                                .Entries()
                                .Where(e => e.Entity is IAuditable &&
                                            (e.State == EntityState.Added ||
                                            e.State == EntityState.Modified ||
                                           e.State == EntityState.Deleted)
                                       );
        }
    }
}
