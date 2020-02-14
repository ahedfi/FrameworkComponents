using Ahedfi.Component.Services.Domain.Inerfaces;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection.Abstractions;
using System;

namespace Ahedfi.Component.Services.Infrastructure
{
    public class ServiceLocator : IServiceLocator
    {
        private readonly IServiceProvider _provider;

        public ServiceLocator(IServiceProvider provider)
        {
            _provider = provider;
        }
        public IEnumerable<TService> GetAllInstances<TService>()
        {
            return GetInstance<IEnumerable<TService>>();
        }
        public TService GetInstance<TService>()
        {
            return (TService)_provider.GetService(typeof(TService));
        }
        public object GetService(Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }
            return _provider.GetService(serviceType);
        }
    }
}
