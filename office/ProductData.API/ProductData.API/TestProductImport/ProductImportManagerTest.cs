using Affilinet.Business.ProductImport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace TestProductImport
{
    
    
    /// <summary>
    ///This is a test class for ProductImportManagerTest and is intended
    ///to contain all ProductImportManagerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ProductImportManagerTest
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
        ///A test for StartImport
        ///</summary>
        [TestMethod()]
        public void StartImportTest()
        {
            ProductImportManager_Accessor target = new ProductImportManager_Accessor(); // TODO: Initialize to an appropriate value
           // target.StartImport();
           // Assert.IsNotNull(target.dtScheduledPrograms, "StartImport failed.");
        }


        ///// <summary>
        /////A test for DownloadCompleteEventHandler
        /////</summary>
        //[TestMethod()]
        //[DeploymentItem("Affilinet.Business.ProductImport.dll")]
        //public void DownloadCompleteEventHandlerTest()
        //{
        //    ProductImportManager_Accessor target = new ProductImportManager_Accessor(); // TODO: Initialize to an appropriate value
        //    BaseDownloadManager objDwndMgr = null; // TODO: Initialize to an appropriate value
        //    target.DownloadCompleteEventHandler(objDwndMgr);
        //    Assert.Inconclusive("DownloadCompleteEventHandler run - success.");
        //}

        /// <summary>
        ///A test for ProductImportManager Constructor
        ///</summary>
        [TestMethod()]
        public void ProductImportManagerConstructorTest()
        {
            ProductImportManager target = new ProductImportManager();
            Assert.IsNotNull(target, "StartImport failed.");
        }
    }
}
