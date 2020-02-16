namespace Ahedfi.Component.Communication.Domain.Entities
{
    public class SaveResponse<T> : BaseResponse
    {
        public T Value { get; set; }
    }
}
