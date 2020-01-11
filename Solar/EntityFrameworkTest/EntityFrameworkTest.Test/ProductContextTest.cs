using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Text;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EntityFrameworkTest;

namespace EntityFrameworkTest.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ProductContextTest
    {
        public ProductContextTest()
        {
            //EncryptConfig();
            // neu erstellen, wenn model sich ändert
            Database.SetInitializer<ProductContext>(new DropCreateDatabaseIfModelChanges<ProductContext>());
        }

        private void EncryptConfig()
        {
            Configuration config;
            ConfigurationSection configSection;
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configSection = config.ConnectionStrings;
            if (configSection != null)
            {
                if (!(configSection.SectionInformation.IsLocked))
                {
                    configSection.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                    config.Save();

                }
            }
        }

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
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestProductAdd()
        {
            using (var db = new ProductContext())
            {
                String category = "L";
                CheckAndAddCategory(db, category, "Lego");
                CheckAndAddAddress(db, new Address() { AddressId = 2, Country = "DE", Street = "johann clanze str 55" });

                var product = new Product() { Name = "Raumschiff", Test = "nicht wichtig", CategoryId = "L", AddressId = 2, Price = 100.36m};
                db.Products.Add(product);

                int recordsAffected = db.SaveChanges();

                Assert.IsTrue(recordsAffected > 0);
            }
        }

        private static void CheckAndAddCategory(ProductContext db, string categoryId, string categoryName)
        {
            // check, if category exists
            var categoryList = db.Categories.Find(categoryId);

            if (categoryList == null) // add, if no
            {
                Category objCategory = new Category() { CategoryId = categoryId, Name = categoryName };
                db.Categories.Add(objCategory);
            }
        }

        private static void CheckAndAddAddress(ProductContext db, Address address)
        {
            // check, if category exists
            var addressList = db.Addresses.Find(address.AddressId);

            if (addressList == null) // add, if no
            {
                Address objAddress = new Address() { AddressId = address.AddressId, Street = address.Street, Country = address.Country };
                db.Addresses.Add(objAddress);
            }
        }

        [TestMethod]
        public void TestProductEdit()
        {
            using (var db = new ProductContext())
            {
                int recordsAffected = 0;
                // does product exist?
                int productID = (new Random().Next(1, 5000));

                var product = db.Products.Where(p => p.ProductId == productID).FirstOrDefault();

                if (product == null) // add, if no
                {
                    product = new Product() { Name = "Raumschiff" , Test = "nicht wichtig", CategoryId = "X" , AddressId = 1};
                    db.Products.Add(product);
                    recordsAffected = db.SaveChanges();
                    productID = product.ProductId;
                    Assert.IsTrue(recordsAffected > 0);
                }
                else
                {
                    // edit
                    product.Name = "Changed Raumschiff neu" + DateTime.Now.ToLongDateString() + DateTime.Now.ToLongTimeString();
                    productID = product.ProductId;
                    recordsAffected = db.SaveChanges();
                }

                Assert.IsTrue(recordsAffected > 0, "Records affected must be > 0");

                product = db.Products.Find(productID);

                Assert.IsNotNull(product, "Could not find product with id:" + productID);
                Assert.IsTrue(product.Name.StartsWith("Changed") || product.Name.StartsWith("Raumschiff"), "product name was not updated");
            }
        }


        [TestMethod]
        public void TestProductEditAutoMapping()
        {
            using (var db = new ProductContext())
            {
                Product product = new Product()
                {
                    ProductId = new Random().Next(1,5000),
                    Name = "Changed Raumschiff " + DateTime.Now.ToLongDateString() + DateTime.Now.ToLongTimeString(),
                    Test = "new nicht wichtig",
                    CategoryId = "X",
                    Price = 11.23m,
                    AddressId = 1
                };
                CheckAndAddCategory(db, product.CategoryId, "Electronics");
                CheckAndAddAddress(db, new Address() { AddressId = product.AddressId, Country = "DE", Street = "johann clanze str" });
                TestProductEditAutoMapping(product, db);
            }
        }
        private void TestProductEditAutoMapping(Product newProduct, ProductContext db)
        {

            int recordsAffected = 0;
            // does product exist?
            var product = db.Products.Where(p => p.ProductId == newProduct.ProductId).FirstOrDefault();

            if (product == null) // add, if no
            {
                db.Products.Add(newProduct);//note: product id will be ignored for new product, will be auto generated
                recordsAffected = db.SaveChanges();
                Assert.IsTrue(recordsAffected > 0);
            }
            else
            {
                product.Name = newProduct.Name;
                product.AddressId = newProduct.AddressId;
                product.CategoryId = newProduct.CategoryId;
                product.Price = newProduct.Price;

                //newProduct.CopyObject(product);//map product todo
                recordsAffected = db.SaveChanges();//save changes
            }

            Assert.IsTrue(recordsAffected > 0, "Records affected must be > 0");

            product = db.Products.Find(newProduct.ProductId);
            Assert.IsNotNull(product, "Could not find product with id:" + newProduct.ProductId);
            Assert.IsTrue(product.Name.StartsWith("Changed"), "product name was not updated");
        }

        [TestMethod]
        public void TestProductDeleteAll()
        {
            using (var db = new ProductContext())
            {
                var productList = db.Products.ToList();
                Assert.IsNotNull(productList);
                foreach (var product in productList)
                {
                    db.Products.Remove(product);
                }

                db.SaveChanges();

                productList = db.Products.ToList();
                Assert.IsTrue(productList.Count == 0);
            }
        }

        [TestMethod]
        public void TestProductGetList()
        {
            using (var db = new ProductContext())
            {
                var productList = db.Products.ToList();
                Assert.IsNotNull(productList);
            }
        }


    }
}
