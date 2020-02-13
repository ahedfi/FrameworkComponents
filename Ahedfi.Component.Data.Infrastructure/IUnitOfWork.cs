using Ahedfi.Component.Data.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ahedfi.Component.Data.Infrastructure
{
    public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        TContext DbContext { get; }
    }
}
