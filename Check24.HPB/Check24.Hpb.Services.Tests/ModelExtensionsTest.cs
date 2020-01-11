using Check24.Data.Models;
using Check24.Hpb.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Check24.Contracts.Models;

namespace Check24.Hpb.Services.Tests
{
    
    
    /// <summary>
    ///This is a test class for ModelExtensionsTest and is intended
    ///to contain all ModelExtensionsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ModelExtensionsTest
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
        ///A test for CalculatePrice
        ///</summary>
        [TestMethod()]
        public void CalculatePrice_Of_Package_Hierarchy_Test()
        {
            var rootAsset = DataFillMethods.GetAssetPackages();

            rootAsset.CalculatePrice();
            Assert.IsTrue(rootAsset.Price == 600.00, "Expected price for root asset node is invalid.");
        }



        /// <summary>
        ///A test for CalculatePriceAfterBids
        ///</summary>
        [TestMethod()]
        public void CalculatePriceAfterBidsTest()
        {
            var rootAsset = DataFillMethods.GetAssetPackagesWithBids();

            rootAsset.AssignMaxPriceAfterBids();
            rootAsset.CalculatePrice();
            Assert.IsTrue(rootAsset.Price == 700.00, "Expected price for root asset node is invalid.");
        }
    }
}
