using Ahedfi.Component.Data.Domain.Interfaces;
using Ahedfi.Component.Data.Infrastructure;
using Ahedfi.Component.Hosting.Domain.Services;
using Ahedfi.Component.Services.Domain.Inerfaces;
using Ahedfi.Component.Services.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ahedfi.Component.Bootstrapper
{
    public class BootstrapperModule : BaseModule
    {
        public override int Order => -1;
        public override void RegisterTypes(IConfiguration configuration, IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IServiceLocator, ServiceLocator>();
            services.AddScoped<IMapEngine, MapEngine>();
        }
    }
}
