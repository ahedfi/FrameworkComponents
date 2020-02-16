using System;

namespace Ahedfi.Component.Communication.Domain.Entities
{
    public class BaseResponse
    {
        public Guid CorrelationId { get; set; }
        public Exception Exception { get; set; }
        public string Token { get; set; }
    }
}
