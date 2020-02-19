using Ahedfi.Component.Core.Domain.Models.Interfaces;
using Ahedfi.Component.Core.Domain.Security.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Ahedfi.Component.Services.Domain.Inerfaces
{
    public interface IBusiness<T> where T : IEntity
    {
        T Save(IUserIdentity user, T Entity);
        bool Delete(IUserIdentity user, T Entity);
        T FindFirstOrDefault(Expression<Func<T, bool>> predicate);
        IEnumerable<T> Filter(Expression<Func<T, bool>> predicate);
        IEnumerable<T> FindAll();
    }
}
