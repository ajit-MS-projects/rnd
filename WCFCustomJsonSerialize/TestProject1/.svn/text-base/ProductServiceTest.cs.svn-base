using System.ServiceModel;
using WCFCustomJsonSerialize;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;

namespace TestProject1
{
    
    
    /// <summary>
    ///This is a test class for ProductServiceTest and is intended
    ///to contain all ProductServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ProductServiceTest
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

        private ProductResponse SearchProducts_PositiveTestBasic(ProductRequest request)
        {
            EndpointAddress productAddress = new EndpointAddress("http://localhost:12877/ProductService.svc");
            IProductService metaDataServiceChannel = ChannelFactory<IProductService>.CreateChannel(new BasicHttpBinding(), productAddress);
            //ProductService productService = metaDataServiceChannel.WhatAreYouDoing();

            ProductResponse resp = metaDataServiceChannel.SearchProducts(request);
            Assert.IsTrue(resp.Products.Count > 0, "Product count is 0");

            return resp;
        }

        /// <summary>
        ///A test for SearchProducts
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod()]
        public void S001_SearchProductsTest()
        {
            ProductRequest req = new ProductRequest();
            req.Query = "ipod";
            ProductResponse resp = SearchProducts_PositiveTestBasic(req);
            //Assert.IsTrue(resp.Products[0].ProductType == ProductTypeEnum.ConsumerGood, "Expected product type is ConsumerGood");
        }
    }
}
