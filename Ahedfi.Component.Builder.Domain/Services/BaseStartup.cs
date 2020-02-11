using Ahedfi.Component.Builder.Domain.Exceptions;
using Ahedfi.Component.Hosting.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ahedfi.Component.Builder.Domain.Services
{
    public class BaseStartup
    {
        public IConfiguration Configuration { get; }

        public BaseStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            var baseModule = typeof(IBaseModule);
            LoadModuleAssemblies();
            // Register core module first
            var coreModule = GetCoreModuleType(baseModule);
            RegisterModule(services, coreModule);
            var modules = GetModulesExceptCoreModule(baseModule, coreModule);
            foreach (var module in modules)
                RegisterModule(services, module);
        }
        private Type GetCoreModuleType(Type type)
        {
            try
            {
                return AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(a => a.GetTypes())
                        .Where(a => type.IsAssignableFrom(a) && a.FullName.ToLower().Contains("server")
                                    && ((int)a.GetProperty(nameof(IBaseModule.Order)).GetValue(Activator.CreateInstance(a), null)) == 0)
                        .Single();
            }
            catch (ArgumentNullException)
            {
                throw new BuilderExcetpion("module assemblies should not be null");
            }
            catch (InvalidOperationException)
            {
                throw new BuilderExcetpion("module assemblies contains more than one core module");
            }
            catch (Exception e)
            {
                throw new BuilderExcetpion(e.Message);
            }
        }
        private IEnumerable<Type> GetModulesExceptCoreModule(Type type, Type coreModule)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                            .SelectMany(a => a.GetTypes())
                            .Where(a => type.IsAssignableFrom(a) && a.FullName.ToLower().Contains("server") && a.FullName != coreModule.FullName);
        }
        private void LoadModuleAssemblies()
        {
            var moduleAssemblies = Configuration.GetSection("Modules").Get<IEnumerable<string>>();
            foreach (var item in moduleAssemblies)
            {
                Assembly.Load(item);
            }
        }
        private void RegisterModule(IServiceCollection services, Type item)
        {
            IBaseModule instance = (IBaseModule)Activator.CreateInstance(item);
            instance.RegisterTypes(services);
        }
    }
}
