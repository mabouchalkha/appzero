using System;
using Sarwa.Core.Common.Contracts;

namespace DataLayerTest.Entities
{
    public class Order : IIdentifiableEntity<int>
    {
        public int EntityId
        {
            get
            {
                return OrderId;
            }

            set
            {
                OrderId = value;
            }
        }

        public int OrderId { get; set; }
        public string Product { get; set; }
    }
}