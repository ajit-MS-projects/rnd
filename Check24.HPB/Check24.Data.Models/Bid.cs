using Check24.Contracts.Models;

namespace Check24.Data.Models
{
    public class Bid : IBid
    {
        public int BidId { get; set; }
        public double BidPrice { get; set; }
    }
}
