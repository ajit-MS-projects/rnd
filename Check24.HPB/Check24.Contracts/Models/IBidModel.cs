using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Check24.Contracts.Models
{
    public interface IBid
    {
        int BidId { get; set; }
        double BidPrice { get; set; }
    }
}
