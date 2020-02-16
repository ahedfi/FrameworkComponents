using Ahedfi.Component.Core.Domain.Entities;
using System.Collections.Generic;

namespace Ahedfi.Component.Data.Domain.Interfaces
{
    public interface IMapEngine
    {
       TMapTo Map<TMapTo> (object source) where TMapTo : Entity;
       IEnumerable<TMapTo> MapList<TMapTo>(object source) where TMapTo : Entity;
    }
}
