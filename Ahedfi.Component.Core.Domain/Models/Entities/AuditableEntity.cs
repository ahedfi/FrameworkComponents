using Ahedfi.Component.Core.Domain.Models.Interfaces;
using System;

namespace Ahedfi.Component.Core.Domain.Models.Entities
{
    public class AuditableEntity<Tkey> : BaseEntity<Tkey>, IAuditable where Tkey : struct
    {
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
