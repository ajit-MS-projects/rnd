using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Traffective.Contracts
{
    public interface IContentBase
    {
        Guid Id { get; set; }
        DateTime CreatedTimeStamp { get; set; }
        DateTime UpdatedTimeStamp { get; set; }
    }
}
