using Ahedfi.Component.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Ahedfi.Component.Data.Domain.Interfaces
{
    public interface ISpecification<TEntity> where TEntity : Entity
    {
        Expression<Func<TEntity, bool>> Criteria { get; }
        List<Expression<Func<TEntity, object>>> Includes { get; }
        List<string> IncludeStrings { get; }
        Expression<Func<TEntity, object>> OrderBy { get; }
        Expression<Func<TEntity, object>> OrderByDescending { get; }
        Expression<Func<TEntity, object>> GroupBy { get; }

        int Take { get; }
        int Skip { get; }
        bool IsPagingEnabled { get; }
    }
}
