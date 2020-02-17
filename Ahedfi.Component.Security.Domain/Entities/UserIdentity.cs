using Ahedfi.Component.Core.Domain.Security.Interfaces;

namespace Ahedfi.Component.Security.Domain.Entities
{
    public class UserIdentity : IUserIdentity
    {
        public string UserName { get; set; }
    }
}
