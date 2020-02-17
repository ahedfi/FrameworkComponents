using System;
using System.Collections.Generic;

namespace Ahedfi.Component.Core.Domain.DependencyInjection.Interfaces
{
    public interface IServiceLocator : IServiceProvider
    {
        TService GetInstance<TService>();
        IEnumerable<TService> GetAllInstances<TService>();
    }
}
