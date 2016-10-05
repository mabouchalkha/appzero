using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
