using System.ComponentModel.Composition;
using System.Linq;
using System.Xml.Linq;
using MEF.DEMO;

namespace MEF.Rules.DataRules
{
    [Export(typeof(IDataRule))]
    public class NameDataRule : IDataRule
    {
        public bool IsValid(XDocument document)
        {
            var firstName = document.Descendants("FirstName").FirstOrDefault().Value;
            var lastName = document.Descendants("LastName").FirstOrDefault().Value;

            return ! (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName));
        }
    }
}


