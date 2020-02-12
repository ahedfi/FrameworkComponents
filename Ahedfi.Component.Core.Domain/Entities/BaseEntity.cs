using Ahedfi.Component.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ahedfi.Component.Core.Domain.Entities
{
    public class BaseEntity<Tkey> : Entity, IEntity<Tkey>, IDeleted where Tkey : struct
    {
        public Tkey Id { get; set; }
        public bool IsDeleted { get; set; }
    }
}
