using Affilinet.Business.ProductImport.DAO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;

namespace TestProductImport
{
    
    
    /// <summary>
    ///This is a test class for ProductDAOTest and is intended
    ///to contain all ProductDAOTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ProductDAOTest
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
        ///A test for ProductDAO Constructor
        ///</summary>
        [TestMethod()]
        public void ProductDAOConstructorTest()
        {
            ProductDAO target = new ProductDAO();
            Assert.IsNotNull(target, "ProductDAOConstructorTest failed");
        }

        /// <summary>
        ///A test for GetScheduledPrograms
        ///</summary>
        [TestMethod()]
        public void GetScheduledProgramsTest()
        {
            ProductDAO target = new ProductDAO(); // TODO: Initialize to an appropriate value
            DataTable expected = null; // TODO: Initialize to an appropriate value
            DataTable actual;
            actual = target.GetScheduledPrograms(true);
            Assert.IsNotNull(actual, "GetScheduledProgramsTest failed");
        }
    }
}
