using System.ComponentModel.Composition;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using MEF.DEMO;

namespace MEF.Rules.SecurityRules
{
    [Export(typeof(ISecurityRule))]
    public class AutenteficationRule : ISecurityRule
    {
        public bool PassesValidation(XDocument document)
        {
            var email = document.Descendants("SenderEmail").FirstOrDefault().Value;
            var key = document.Descendants("SecreteKey").FirstOrDefault().Value;

            var cryptoServiceProvider = new MD5CryptoServiceProvider();
            var originalBytes = Encoding.UTF8.GetBytes(email);
            var encodedBytes = cryptoServiceProvider.ComputeHash(originalBytes);

            var returnStringBulder = new StringBuilder();
            foreach (byte bytes in encodedBytes)
            {
                returnStringBulder.Append(bytes.ToString("x2").ToLower());
            }
            var computedHash = returnStringBulder.ToString();

            return computedHash == key;
        }
    }
}


