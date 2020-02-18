using Ahedfi.Component.Core.Domain.Validation.interfaces;
using Ahedfi.Component.Hosting.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ahedfi.Component.Hosting.Domain.Services
{
    public abstract class BaseModule : IBaseModule
    {
        public virtual int Order { get; }
        public abstract void RegisterTypes(IConfiguration configuration, IServiceCollection services);

        public virtual void RegisterValidator(IRequestValidator requestValidator)
        {
        }
    }
}
