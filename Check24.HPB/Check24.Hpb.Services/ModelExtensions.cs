using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Check24.Contracts.Models;

namespace Check24.Hpb.Services
{
    public static class ModelExtensions
    {
        public static void CalculatePrice(this IAsset rootAsset)
        {
            if (rootAsset.Package.Count > 0)
            {
                double price = 0;
                foreach (var asset in rootAsset.Package)
                {
                    asset.CalculatePrice();
                    price += asset.Price;
                }
                rootAsset.Price = price;
            }

            return;
        }

        public static void AssignMaxPriceAfterBids(this IAsset rootAsset)
        {
            foreach (var asset in rootAsset.Package)
            {
                foreach (var bid in asset.Bids)
                {
                    if (bid.BidPrice > asset.Price)
                    {
                        asset.Price = bid.BidPrice;
                    }
                    asset.AssignMaxPriceAfterBids();
                }
            }
        }
    }
}
