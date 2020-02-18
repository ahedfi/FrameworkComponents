using System.Reflection;
using System.Threading.Tasks;

namespace Ahedfi.Component.Core.Domain.Validation.interfaces
{
    public interface IModelValidator
    {
        bool HasRules(object model);
        Task ValidateAsync(object model);
        void RegisterValidator(Assembly assembly);
        void RegisterValidator(string assemblyName);
    }
}
