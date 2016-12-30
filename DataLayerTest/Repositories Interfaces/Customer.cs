using Sarwa.Core.Common.Contracts;
using System.Collections.Generic;

namespace DataLayerTest.Entities
{
    public class Customer : IIdentifiableEntity<int>
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Address> Adresses { get; set; }
        public List<Order> Orders { get; set; }

        public int EntityId
        {
            get { return CustomerId; }

            set { CustomerId = value; }
        }
    }
}
