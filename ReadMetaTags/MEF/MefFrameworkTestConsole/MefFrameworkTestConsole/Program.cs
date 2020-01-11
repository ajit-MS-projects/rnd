using System ;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Text;
using ProductContracts.Models;
using ProductContracts.Services;

namespace MefFrameworkTestConsole
{
	class Program
	{
		//[Import]
		protected IGreeter ProgrammGreeter { get; set; }
		[Import]
		protected IProductService ProductService { get; set; }
		
		static void Main(string[] args)
		{
			var program = new Program();
			program.Compose();
			//program.Run();
			program.GetProdct();
			program.GetProdcts();
		}

		private void GetProdct()
		{
			Console.WriteLine();
			IProduct product = ProductService.GetProduct(1);
			Console.WriteLine(product.Title);

			Console.ReadKey();
		}

		private void GetProdcts()
		{
			Console.WriteLine();
			var products = ProductService.GetProducts();
			foreach (var product in products)
			{
				Console.WriteLine(product.Title);
			}
			Console.ReadKey();
		}
		private void Run()
		{
			ProgrammGreeter.SayHello();
			Console.Read();
		}
		private void Compose()
		{
			//var assemblyCatalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
			//var compositionContainer = new CompositionContainer(assemblyCatalog);
			//compositionContainer.ComposeParts(this);

			var catalog = new DirectoryCatalog(Environment.CurrentDirectory);
			var container = new CompositionContainer(catalog);
			container.ComposeParts(this);
		}
	}
	public interface IGreeter
	{
		void SayHello();
	}

	[Export(typeof(IGreeter))]
	public class Greeter : IGreeter
	{
		public void SayHello()
		{
			Console.WriteLine("Hello MEF World!");
		}
	}
}
