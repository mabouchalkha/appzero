using Sarwa.Core.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
