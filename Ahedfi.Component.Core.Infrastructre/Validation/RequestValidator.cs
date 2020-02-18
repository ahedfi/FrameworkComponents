using Ahedfi.Component.Core.Domain.Validation.interfaces;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Ahedfi.Component.Core.Infrastructre.Validation
{
    public class RequestValidator : IRequestValidator
    {
        private readonly IServiceCollection _services;
        private readonly IServiceProvider _provider;

        public RequestValidator(IServiceCollection services, IServiceProvider provider)
        {
            _services = services;
            _provider = provider;
        }

        public bool HasRules(object model)
        {
            Type t = model.GetType();
            if (t.IsClass)
            {
                IValidator validator = (IValidator)_provider.GetService(typeof(IValidator<>).MakeGenericType(t));
                return validator != null;
            }
            return false;
        }

        public void RegisterValidator(string assemblyName)
        {
            var assembly = Assembly.Load(assemblyName);
            RegisterValidator(assembly);
        }

        public void RegisterValidator(Assembly assembly)
        {
            AssemblyScanner.FindValidatorsInAssembly(assembly).ForEach(pair =>
            {
                _services.Add(ServiceDescriptor.Singleton(pair.InterfaceType, pair.ValidatorType));
            });
        }

        public async Task ValidateAsync(object model)
        {
            IValidator validator = (IValidator)_provider.GetService(typeof(IValidator<>).MakeGenericType(model.GetType()));
            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
        }
    }
}
