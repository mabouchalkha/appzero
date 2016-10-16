namespace Sarwa.Core.Common.Contracts
{
    public interface IBusinessEngineFactory
    {
        T GetBusinessEngine<T>() where T : IBusinessEngine;
    }
}
