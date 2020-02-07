using Ahedfi.Component.Hosting.Domain.Interface;
using Autofac;

namespace Ahedfi.Component.Hosting.Domain.Service
{
    public abstract class AutofacModule : Module, IModule
    {
        public int Order { get; }
        public abstract void RegisterTypes(ContainerBuilder builder);
    }
}
