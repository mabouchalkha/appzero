using DataLayerTest.Entities;
using Sarwa.Core.Common.Contracts;

namespace DataLayerTest.Repositories_Interfaces
{
    public interface ICustomerRepository : IDataRepositoryBase<Customer, DbContextTest, int>
    {
    }
}
