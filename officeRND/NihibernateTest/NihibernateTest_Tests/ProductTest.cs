using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NihibernateTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;

namespace NihibernateTest_Tests
{


	/// <summary>
	///This is a test class for ProductTest and is intended
	///to contain all ProductTest Unit Tests
	///</summary>
	[TestClass()]
	public class ProductTest
	{


		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get { return testContextInstance; }
			set { testContextInstance = value; }
		}

		#region Additional test attributes

		// 
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		[ClassInitialize()]
		public static void MyClassInitialize(TestContext testContext)
		{
			//ProductTest testObj = new ProductTest();
			//testObj.Generate_schema();
			//testObj.CreateInitialData();
		}
		//
		//Use ClassCleanup to run code after all tests in a class have run
		//[ClassCleanup()]
		//public static void MyClassCleanup()
		//{
		//}
		//
		//Use TestInitialize to run code before running each test
		[TestInitialize()]
		public void MyTestInitialize()
		{
			
		Generate_schema();
		CreateInitialData();
		}
		//
		//Use TestCleanup to run code after each test has run
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//

		#endregion

		private Configuration _configuration;

		//[TestMethod]
		public void Generate_schema()
		{
			_configuration = new Configuration();
			_configuration.Configure();
			_configuration.AddAssembly(typeof (Product).Assembly);

			new SchemaExport(_configuration).Execute(false, true, false); //create new db schema (code first thing)

		}

		[TestMethod]
		public void Can_add_new_product()
		{
			var product = new Product {Name = "Apple", Category = "Fruits"};
			IProductRepository repository = new ProductRepository();
			repository.Add(product);

			var fromDb = repository.GetById(product.Id);
			// Test that the product was successfully inserted
			Assert.IsNotNull(fromDb);
			Assert.AreNotSame(product, fromDb);
			Assert.AreEqual(product.Name, fromDb.Name);
			Assert.AreEqual(product.Category, fromDb.Category);
		}

		[TestMethod]
		public void Can_update_existing_product()
		{
			var product = _products[0];
			product.Name = "Yellow Pear";
			IProductRepository repository = new ProductRepository();
			repository.Update(product);

			// use session to try to load the product
			using (ISession session = NHibernateHelper.OpenSession())
			{
				var fromDb = session.Get<Product>(product.Id);
				Assert.AreEqual(product.Name, fromDb.Name);
			}
		}

		[TestMethod]
		public void Can_remove_existing_product()
		{
			var product = _products[0];
			IProductRepository repository = new ProductRepository();
			repository.Remove(product);

			using (ISession session = NHibernateHelper.OpenSession())
			{
				var fromDb = session.Get<Product>(product.Id);
				Assert.IsNull(fromDb);
			}
		}

		[TestMethod]
		public void Can_get_existing_product_by_id()
		{
			IProductRepository repository = new ProductRepository();
			var fromDb = repository.GetById(_products[1].Id);
			Assert.IsNotNull(fromDb);
			Assert.AreNotSame(_products[1], fromDb);
			Assert.AreEqual(_products[1].Name, fromDb.Name);
		}

		[TestMethod]
		public void Can_get_existing_product_by_name()
		{
			IProductRepository repository = new ProductRepository();
			var fromDb = repository.GetByName(_products[1].Name);

			Assert.IsNotNull(fromDb);
			Assert.AreNotSame(_products[1], fromDb);
			Assert.AreEqual(_products[1].Id, fromDb.Id);
		}

		private  Product[] _products = null;

		private void CreateInitialData()
		{
			_products = new[]
                 {
                     new Product {Name = "Melon", Category = "Fruits"},
                     new Product {Name = "Pear", Category = "Fruits"},
                     new Product {Name = "Milk", Category = "Beverages"},
                     new Product {Name = "Coca Cola", Category = "Beverages"},
                     new Product {Name = "Pepsi Cola", Category = "Beverages"},
                 };

			using (ISession session = NHibernateHelper.OpenSession())
			using (ITransaction transaction = session.BeginTransaction())
			{
				foreach (var product in _products)
					session.Save(product);
				transaction.Commit();
			}
		}
	}
}
