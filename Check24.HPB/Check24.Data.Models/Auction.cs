using System.Collections.Generic;
using Check24.Contracts.Models;

namespace Check24.Data.Models
{
    public class Auction : IAuction
    {
        public IAsset Packages { get; set; }
        //public List<IBid> Bids { get; set; }
    }
}