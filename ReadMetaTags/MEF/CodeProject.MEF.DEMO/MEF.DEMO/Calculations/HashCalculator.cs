using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MEF.DEMO.Calculations
{
    class HashCalculator
    {
        public static string HelpWithHash()
        {
            var valueToHash = "andriybuday@gmail.com";
            var cryptoServiceProvider = new MD5CryptoServiceProvider();
            var originalBytes = Encoding.UTF8.GetBytes(valueToHash);
            var encodedBytes = cryptoServiceProvider.ComputeHash(originalBytes);

            var returnStringBulder = new StringBuilder();
            foreach (byte bytes in encodedBytes)
            {
                returnStringBulder.Append(bytes.ToString("x2").ToLower());
            }
            var computedHash = returnStringBulder.ToString();
            return computedHash;
        }
    }
}
