using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Check24.Contracts.Models;

namespace Check24.Hpb.Models
{
    public class BidModel : IBid
    {
        public int BidId{ get; set; }
        public IAsset Asset{ get; set; }
        public int AssetId { get; set; }
        public double BidPrice { get; set; }
        public string Message { get; set; }
        private const string DefaultMessage="Your Bid has been registered";
        public BidModel()
        {
            
            Message = DefaultMessage;
        }
    }
}