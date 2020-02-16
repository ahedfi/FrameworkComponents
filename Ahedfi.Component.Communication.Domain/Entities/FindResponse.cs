using System.Collections.Generic;

namespace Ahedfi.Component.Communication.Domain.Entities
{
    public class FindResponse<T> : BaseResponse
    {
        public IEnumerable<T> Value { get; set; }
    }
}
