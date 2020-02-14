using System;
using System.Collections.Generic;
using System.Text;

namespace Ahedfi.Component.Services.Domain.Inerfaces
{
    public interface IServiceLocator : IServiceProvider
    {
        TService GetInstance<TService>();
        IEnumerable<TService> GetAllInstances<TService>();
    }
}
