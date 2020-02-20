using Ahedfi.Component.Data.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ahedfi.Component.Data.Infrastructure
{
    public abstract class BaseContext : DbContext
    {
        public BaseContext(DbContextOptions options) : base(options)
        {

        }
        protected internal DbSet<AuditTrail> AuditTrails { get; set; }
    }
}
