using System;
using System.Collections.Generic;
using System.Text;

namespace Ahedfi.Component.Core.Domain.Interfaces
{
    public interface IEntity
    { }
    public interface IEntity<Tkey> : IEntity
    {
        Tkey Id { get; set; }
    }
}
