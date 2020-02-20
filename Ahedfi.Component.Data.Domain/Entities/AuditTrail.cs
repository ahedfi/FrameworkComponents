using Ahedfi.Component.Core.Domain.Models.Entities;
using Ahedfi.Component.Core.Domain.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ahedfi.Component.Data.Domain.Entities
{
    public class AuditTrail : BaseEntity<int>, IAggregateRoot
    {
        public string UserName { get; set; }
        public string Table { get; set; }
        public string Column { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime Date { get; set; }
    }
}
