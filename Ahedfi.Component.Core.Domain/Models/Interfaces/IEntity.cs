namespace Ahedfi.Component.Core.Domain.Models.Interfaces
{
    public interface IEntity
    { }
    public interface IEntity<Tkey> : IEntity
    {
        Tkey Id { get; set; }
    }
}
