using Traffective.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Traffective.Data.Helpers;

namespace Traffective.Data.Tests
{
    
    
    /// <summary>
    ///This is a test class for TrackingTest and is intended
    ///to contain all TrackingTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TrackingTest
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
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            PersistenceHelper.Initialize();
            PersistenceHelper.CreateDbSchema();
        }
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
        ///A test for Id
        ///</summary>
        [TestMethod()]
        public void ClickTest()
        {
            Tracking target = new Tracking()
                                  {
                                      AdvertisementId = 1,AdvertiserId = 11,CreatedTimeStamp = DateTime.Now,FingerPrintId = "ssskjjkjk5",IP="22.44.55.66",Referer = "4444"
                                      ,UpdatedTimeStamp = DateTime.Now
                                  }; 
           target.Save();
        }

    }
}
