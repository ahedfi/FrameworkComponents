namespace Ahedfi.Component.Communication.Domain.Entities
{
    public class DeleteRequest<T> : BaseRequest
    {
        public T Id { get; set; }
    }
}
