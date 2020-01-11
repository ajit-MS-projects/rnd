using Check24.Contracts.Models;

namespace Check24.Contracts.Services
{
    public interface IAuctionService
    {
        IBid PlaceBid(IBid bid);
        IAuction PlaceAuction(IAuction auction);
        IAuction GetAuction(int auctionId);
    }
}