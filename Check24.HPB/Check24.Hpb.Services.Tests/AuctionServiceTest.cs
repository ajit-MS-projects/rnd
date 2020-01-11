using System.Collections.Generic;
using System.Linq;
using Check24.Data.Models;
using Check24.Hpb.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Check24.Contracts.Models;

namespace Check24.Hpb.Services.Tests
{
    
    
    /// <summary>
    ///This is a test class for ActionServiceTest and is intended
    ///to contain all ActionServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AuctionServiceTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for PlaceAuction
        ///</summary>
        [TestMethod()]
        public void PlaceAuctionTest()
        {
            AuctionService target = new AuctionService(); 
            IAuction auction = new Auction();
            auction.Packages = DataFillMethods.GetAssetPackagesWithBids();
            target.PlaceAuction(auction);
            Assert.IsTrue(auction.Packages.Price==700);
        }

      
    }
}
