using Ahedfi.Component.Core.Domain.Models.Entities;
using Ahedfi.Component.Core.Domain.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ahedfi.Component.Data.Domain.Entities
{
    public class AuditTrail : Entity, IAggregateRoot
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string ObjectName { get; set; }
        public string ObjectId { get; set; }
        public string PropertyName { get; set; }
        public string PropertyType { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string ChangeType { get; set; }
        public DateTime Date { get; set; }
        public Guid BatchId { get; set; }
    }
}
