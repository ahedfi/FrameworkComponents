using Ahedfi.Component.Security.Domain.Entities;
using Ahedfi.Component.Security.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Ahedfi.Component.Communication.Domain.Entities
{
    public class BaseRequest
    {
        [JsonIgnore]
        public IUserIdentity Owner { get; set; }
        [JsonIgnore]
        public string IpAddress { get; set; }
        [JsonPropertyName("t")]
        public string Token { get; set; }
        [JsonIgnore]
        public Guid CorrelationId { get; set; }
    }
}
