using Ahedfi.Component.Core.Domain.DependencyInjection.Interfaces;
using Ahedfi.Component.Core.Domain.Validation.interfaces;
using Ahedfi.Component.Core.Infrastructre.DependencyInjection;
using Ahedfi.Component.Core.Infrastructre.Validation;
using Ahedfi.Component.Data.Domain.Interfaces;
using Ahedfi.Component.Data.Infrastructure;
using Ahedfi.Component.Hosting.Domain.Services;
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
            services.AddSingleton<IRequestValidator>(provider => new RequestValidator(services, provider));
        }
    }
}
