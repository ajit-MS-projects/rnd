using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace MEF.DEMO
{
    public class Programm
    {
        public static void Main(string[] args)
        {
            var programm = new Programm();
            programm.Run();
        	Console.Read();
        }

        [Import]
        public ISecurityRule SecurityRule { get; set; }

        [ImportMany]
        public IDataRule[] DataRules { get; set; }

        public void Run()
        {
            Console.WriteLine("Programm run.");
            Compose();
            Console.WriteLine("Composition completed.");

            var document = XDocument.Load("developer.xml");
            Console.WriteLine(document.ToString());

            var passesValidation = SecurityRule.PassesValidation(document);
            Console.WriteLine(string.Format("Rule {0}: {1}", SecurityRule.GetType(), passesValidation));

            foreach (var d in DataRules)
            {
                var valid = d.IsValid(document);
                Console.WriteLine(string.Format("Rule {0}: {1}", d.GetType(), valid));
            }           
        }

        private void Compose()
        {
            var catalog = new DirectoryCatalog(Environment.CurrentDirectory);
            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);
        }
    }
}
