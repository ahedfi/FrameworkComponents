using Ahedfi.Component.Security.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ahedfi.Component.Security.Domain.Entities
{
    public class UserIdentity : IUserIdentity
    {
        public string UserName { get; set; }
    }
}
