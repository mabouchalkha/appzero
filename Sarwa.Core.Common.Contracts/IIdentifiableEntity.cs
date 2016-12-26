namespace Sarwa.Core.Common.Contracts
{
    public interface IIdentifiableEntity
    {

    }

    public interface IIdentifiableEntity<TKey> : IIdentifiableEntity
    {
        TKey EntityId { get; set; }
    }
}
