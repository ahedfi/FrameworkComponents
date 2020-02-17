using System.Reflection;

namespace Ahedfi.Component.Hosting.Domain.Interfaces
{
    public interface IModelValidator
    {
        bool HasRules(object model);
        bool IsValid(object model);
        void Validate(object model);
        void RegisterValidator(string assemblyName);
        void RegisterValidator(Assembly assembly);
    }
}
