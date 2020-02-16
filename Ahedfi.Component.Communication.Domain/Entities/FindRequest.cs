namespace Ahedfi.Component.Communication.Domain.Entities
{
    public class FindRequest : BaseRequest
    {
        public int Index { get; set; }
        public int PageSize { get; set; }
    }
}
