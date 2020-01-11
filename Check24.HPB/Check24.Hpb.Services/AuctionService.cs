using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Check24.Contracts.Models;
using Check24.Contracts.Services;
using Check24.Data.Models;

namespace Check24.Hpb.Services
{
    public class AuctionService : IAuctionService
    {
        public IBid PlaceBid(IBid bid)
        {
            throw new NotImplementedException();
            //todo assign bid to asset
            return bid;
        }

        public IAuction PlaceAuction(IAuction auction)
        {
            auction.Packages = DataFillMethods.GetAssetPackagesWithBids();
            auction.Packages.AssignMaxPriceAfterBids();
            auction.Packages.CalculatePrice();
            return auction;
        }

        public IAuction GetAuction(int auctionId)
        {
            IAuction auction = new Auction();
            auction.Packages = DataFillMethods.GetAssetPackages();
            return auction;
        }
    }
}
