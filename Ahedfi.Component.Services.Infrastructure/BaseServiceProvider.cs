using Ahedfi.Component.Services.Domain.Inerfaces;

namespace Ahedfi.Component.Services.Infrastructure
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
