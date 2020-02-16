namespace Ahedfi.Component.Communication.Domain.Entities
{
    public class SaveRequest<T> : BaseRequest
    {
        public T Value { get; set; }
    }
}
