using Autofac;

namespace Ahedfi.Component.Hosting.Domain.Interface
{
    public interface IModule
    {
        int Order { get; }

        void RegisterTypes(ContainerBuilder builder);
    }
}
