using Microsoft.Extensions.DependencyInjection;

namespace Ahedfi.Component.Hosting.Domain.Interfaces
{
    public interface IBaseModule
    {
        int Order { get; }

        void RegisterTypes(IServiceCollection container);
    }
}
