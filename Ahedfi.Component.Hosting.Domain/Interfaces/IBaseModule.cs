using Ahedfi.Component.Core.Domain.Validation.interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ahedfi.Component.Hosting.Domain.Interfaces
{
    public interface IBaseModule
    {
        int Order { get; }
        void RegisterTypes(IConfiguration configuration, IServiceCollection container);
        void RegisterValidator(IRequestValidator requestValidator); 
    }
}
