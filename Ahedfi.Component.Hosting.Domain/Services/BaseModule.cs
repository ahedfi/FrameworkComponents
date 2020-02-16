using Ahedfi.Component.Hosting.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ahedfi.Component.Hosting.Domain.Services
{
    public abstract class BaseModule : IBaseModule
    {
        public virtual int Order { get; }
        public virtual void RegisterTypes(IConfiguration configuration, IServiceCollection services)
        {
            
        }
    }
}
