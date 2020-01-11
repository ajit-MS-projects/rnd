using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MEF.HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new Program();
            program.Run();
        }

        private void Run()
        {
            Compose();
            ProgrammGreeter.SayHello();
        }

        //We need guide MEF how to Compose all we need
        private void Compose()
        {
            //Catalog says where to search for capabilities (Where do exports live)
            var assemblyCatalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            //CompositionContainer holds defined dependencies and coordinates creation 
            var compositionContainer = new CompositionContainer(assemblyCatalog);

            //CompositionBatch object holds references to all objects that need to be composed
            var compositionBatch = new CompositionBatch();
            //one of such objects is Programm instance (this), it needs to be composed
            compositionBatch.AddPart(this);

            //And finally Container has method called Compose to do actuall Composing
            compositionContainer.Compose(compositionBatch);
        }

        [Import]
        protected IGreeter ProgrammGreeter { get; set; }
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
