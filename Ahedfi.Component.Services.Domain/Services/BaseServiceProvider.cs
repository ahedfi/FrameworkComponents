using Ahedfi.Component.Core.Domain.DependencyInjection.Interfaces;
using Ahedfi.Component.Services.Domain.Inerfaces;

namespace Ahedfi.Component.Services.Domain.Services
{
    public abstract class BaseServiceProvider : IServicesProvider
    {
        private readonly IServiceLocator _serviceLocator;

        public BaseServiceProvider(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }
        protected TService GetInstance<TService>()
        {
            return _serviceLocator.GetInstance<TService>();
        }
    }
}
