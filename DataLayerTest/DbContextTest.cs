﻿using DataLayerTest.Entities;
using Sarwa.Core.Common.Contracts;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DataLayerTest
{
    public class DbContextTest : DbContext
    {
        public DbContextTest() : base("name=main")
        {
            Database.SetInitializer<DbContextTest>(null);
        }

        public DbSet<Customer> CustomerSet { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Ignore<IIdentifiableEntity<int>>();

            modelBuilder.Entity<Customer>().HasKey<int>(e => e.CustomerId).Ignore(e => e.EntityId);
            base.OnModelCreating(modelBuilder);
        }

    }
}
