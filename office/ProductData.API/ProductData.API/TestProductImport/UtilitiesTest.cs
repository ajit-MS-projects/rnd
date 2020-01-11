using Affilinet.Business.ProductImport;
using Affilinet.Business.ProductImport.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Affilinet.Utility.Logging;
using System;

namespace TestProductImport
{
    
    
    /// <summary>
    ///This is a test class for UtilitiesTest and is intended
    ///to contain all UtilitiesTest Unit Tests
    ///</summary>
    [TestClass()]
    public class UtilitiesTest
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
        ///A test for GetAppSettingValue
        ///</summary>
        [TestMethod()]
        public void GetAppSettingValueTest()
        {
            string settingName = "ProdProgProcessOrder"; // TODO: Initialize to an appropriate value
            string expected = "asc"; // TODO: Initialize to an appropriate value
            string actual;
            actual = Utilities.GetAppSettingValue(settingName);
            Assert.AreEqual(expected, actual, "GetAppSettingValueTest - failed");
        }

        /// <summary>
        ///A test for CreateWarningLog
        ///</summary>
        [TestMethod()]
        public void CreateWarningLogTest()
        {
            string message = "This is warning log...";
            ApplicationEventsEnum eventId = new ApplicationEventsEnum();
            Utilities.CreateWarningLog(message, eventId);
            Assert.Inconclusive("No exception but - A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for CreateStartLog
        ///</summary>
        [TestMethod()]
        public void CreateStartLogTest()
        {
            string message = "This is start log...";
            ApplicationEventsEnum eventId = new ApplicationEventsEnum(); // TODO: Initialize to an appropriate value
            Utilities.CreateStartLog(message, eventId);
            Assert.Inconclusive("No exception but - A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for CreateLog
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Affilinet.Business.ProductImport.dll")]
        public void CreateLogTest()
        {
            string message = "This is log...";
            ApplicationEventsEnum eventId = new ApplicationEventsEnum(); // TODO: Initialize to an appropriate value
            LoggingCategoriesEnum logCat = new LoggingCategoriesEnum(); // TODO: Initialize to an appropriate value
            Utilities_Accessor.CreateLog(message, eventId, logCat);
            Assert.Inconclusive("No exception but - A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for CreateInfoLog
        ///</summary>
        [TestMethod()]
        public void CreateInfoLogTest()
        {
            string message = "This is info log...";
            ApplicationEventsEnum eventId = new ApplicationEventsEnum(); // TODO: Initialize to an appropriate value
            Utilities.CreateInfoLog(message, eventId);
            Assert.Inconclusive("No exception but - A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for CreateEndLog
        ///</summary>
        [TestMethod()]
        public void CreateEndLogTest()
        {
            string message = "This is end log...";
            ApplicationEventsEnum eventId = new ApplicationEventsEnum(); // TODO: Initialize to an appropriate value
            Utilities.CreateEndLog(message, eventId);
            Assert.Inconclusive("No exception but - A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for CreateDebugLog
        ///</summary>
        [TestMethod()]
        public void CreateDebugLogTest()
        {
            string message = "This is debug log...";
            ApplicationEventsEnum eventId = new ApplicationEventsEnum(); // TODO: Initialize to an appropriate value
            Utilities.CreateDebugLog(message, eventId);
            Assert.Inconclusive("No exception but - A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Utilities Constructor
        ///</summary>
        [TestMethod()]
        public void UtilitiesConstructorTest()
        {
            Utilities target = new Utilities();
            Assert.IsNotNull(target, "UtilitiesConstructorTest - failed");
        }

        /// <summary>
        ///A test for Utilities IncrementDate
        ///</summary>
        [TestMethod()]
        public void IncrementDateTest()
        {
            //DateTime dt = DateTime.Now;
            //string intervalType = "H";
            //int incrValue = 1;

            //string resultCompare = dt.AddHours(incrValue).ToString("yyyy-MM-dd HH:mm");
                
            //string result = Utilities.IncrementDate(dt, intervalType, incrValue);

            //Assert.IsNotNull(result, "IncrementDateTest - failed");
            //Assert.AreEqual(resultCompare, result, "IncrementDateTest - Result are not equal!");
        }
    }
}
