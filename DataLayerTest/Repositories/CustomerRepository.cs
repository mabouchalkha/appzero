using DataLayerTest.Entities;
using DataLayerTest.Repositories_Interfaces;
using Sarwa.Core.Common.Data;
using System;
using System.Data.Entity;
using System.Linq.Expressions;

namespace DataLayerTest.Repositories
{
    public class CustomerRepository : DataRepositoryBase<Customer, DbContextTest, int>, ICustomerRepository
    {
        protected override DbSet<Customer> DbSet(DbContextTest entityContext)
        {
            return entityContext.CustomerSet;
        }

        protected override Expression<Func<Customer, bool>> IdentifierPredicate(DbContextTest entityContext, int id)
        {
            return (e => e.CustomerId == id);
        }
    }
}
