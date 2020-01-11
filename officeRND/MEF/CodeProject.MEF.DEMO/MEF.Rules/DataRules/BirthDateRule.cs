using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Xml.Linq;
using MEF.DEMO;

namespace MEF.Rules.DataRules
{
    [Export(typeof(IDataRule))]
    public class BirthDateRule : IDataRule
    {
        public bool IsValid(XDocument document)
        {
            var dateOfBirthValue = document.Descendants("DateOfBirth").FirstOrDefault().Value;
            DateTime birthDate;
            DateTime.TryParse(dateOfBirthValue, out birthDate);

            return birthDate != DateTime.MinValue;
        }
    }
}


