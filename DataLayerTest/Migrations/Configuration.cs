namespace DataLayerTest.Migrations
{
    using Entities;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<DataLayerTest.DbContextTest>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DataLayerTest.DbContextTest context)
        {
            var customers = new List<Customer>
            {
               new Customer { FirstName="Amine", LastName="bouchalkha" },
               new Customer { FirstName="Bahija", LastName="chaoulid" },
               new Customer { FirstName="Arwa", LastName="bouchalkha" },
               new Customer { FirstName="Sarrah", LastName="bouchalkha" },
               new Customer { FirstName="Salmane", LastName="bouchalkha" }
            };

            context.CustomerSet.AddRange(customers);
            context.SaveChanges();
        }
    }
}
