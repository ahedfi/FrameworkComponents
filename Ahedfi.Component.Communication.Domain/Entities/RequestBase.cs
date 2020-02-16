using Ahedfi.Component.Security.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ahedfi.Component.Communication.Domain.Entities
{
    public class BaseRequest
    {
        public IUserIdentity Owner { get; set; }
        public string IpAddress { get; set; }
        public string Token { get; set; }
        public Guid CorrelationId { get; set; }
    }
}
