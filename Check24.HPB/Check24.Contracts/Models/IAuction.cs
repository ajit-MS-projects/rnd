using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Check24.Contracts.Models
{
    public interface IAuction
    {
        IAsset Packages { get; set; }
        //List<IBid> Bids { get; set; }
    }
}
