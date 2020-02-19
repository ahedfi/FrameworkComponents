using Ahedfi.Component.Core.Domain.DependencyInjection.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ahedfi.Component.Hosting.WebApi.Domain.Entities
{
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        private readonly IServiceLocator _serviceLocator;

        public BaseApiController(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }
        protected TService GetInstance<TService>()
        {
            return _serviceLocator.GetInstance<TService>();
        }
    }
}
