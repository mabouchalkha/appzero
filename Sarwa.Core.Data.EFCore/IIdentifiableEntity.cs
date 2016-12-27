namespace Sarwa.Core.Data.EFCore
{
    public interface IIdentifiableEntity
    {

    }

    public interface IIdentifiableEntity<TKey> : IIdentifiableEntity
    {
        TKey EntityId { get; set; }
    }
}
