using Ahedfi.Component.Data.Domain.Interfaces;
using Ahedfi.Component.Data.Infrastructure.Behaviors;
using Ahedfi.Component.Services.Domain.Inerfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Ahedfi.Component.Hosting.Domain.Extensions
{
    public static class ModuleExtensions
    {
        public static IServiceCollection AddServiceProvider<TService, TServiceImpl, TUnitOfWork>(this IServiceCollection services)
           where TService : class, IServicesProvider
           where TServiceImpl : class, IServicesProvider, TService
           where TUnitOfWork : IUnitOfWork
        {
            services.AddScoped<TServiceImpl>();
            services.AddScoped<TService>(service => TransactionBehavior<TService>.Create(
                service.GetService<TServiceImpl>(),
                service.GetService<TUnitOfWork>()));
            return services;
        }
    }
}
