using Ahedfi.Component.Core.Domain.Models.Interfaces;

namespace Ahedfi.Component.Core.Domain.Models.Entities
{
    public class BaseEntity<Tkey> : Entity, IEntity<Tkey>, IDeleted where Tkey : struct
    {
        public Tkey Id { get; set; }
        public bool IsDeleted { get; set; }
    }
}
