using DataLayerTest.Entities;
using Sarwa.Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayerTest.Repositories_Interfaces
{
    public interface ICustomerRepository : IBaseRepository<Customer, DbContextTest, int>
    {
    }
}
