using Check24.Contracts.Models;
using Check24.Data.Models;

namespace Check24.Hpb.Services
{
    public class DataFillMethods
    {
        #region private methods
        public static IAsset GetAssetPackages()
        {
            IAsset rootAsset = new Asset()
                                   {
                                       Id = 1,
                                       Name = "abc"
                                   };

            #region package 1

            var packAB = new Asset()
                             {
                                 Id = 2,
                                 Name = "ab"
                             };

            var packA = new Asset()
                            {
                                Id = 3,
                                Name = "a",
                                Price = 100
                            };
            var packB = new Asset()
                            {
                                Id = 4,
                                Name = "b",
                                Price = 200
                            };
            packAB.Package.Add(packA);
            packAB.Package.Add(packB);
            rootAsset.Package.Add(packAB);

            #endregion

            #region package B

            var packC = new Asset()
                            {
                                Id = 5,
                                Name = "c",
                                Price = 300
                            };
            rootAsset.Package.Add(packC);

            #endregion

            return rootAsset;
        }

        public static IAsset GetAssetPackagesWithBids()
        {
            IAsset rootAsset = new Asset()
            {
                Id = 1,
                Name = "abc",
                
            };
            
            #region package 1

            var packAB = new Asset()
            {
                Id = 2,
                Name = "ab"
            };

            var packA = new Asset()
            {
                Id = 3,
                Name = "a",
                Price = 100
            };
            var packB = new Asset()
            {
                Id = 4,
                Name = "b",
                Price = 200
            };

            
            packAB.Package.Add(packA);
            packAB.Package.Add(packB);
            rootAsset.Package.Add(packAB);

            #endregion

            #region package B

            var packC = new Asset()
            {
                Id = 5,
                Name = "c",
                Price = 300
            };
            rootAsset.Package.Add(packC);
            packC.Bids.Add(new Bid() { BidPrice = 400, BidId = 1 });
            #endregion

            return rootAsset;
        }
        #endregion

    }
}