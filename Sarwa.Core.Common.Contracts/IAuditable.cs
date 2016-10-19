using System;

namespace Sarwa.Core.Common.Contracts
{
    public interface IAuditable
    {
        string CreatedBy { get; set; }
        DateTime CreatedDate { get; set; }
        string UpdatedBy { get; set; }
        DateTime UpdateDate { get; set; }
    }
}
