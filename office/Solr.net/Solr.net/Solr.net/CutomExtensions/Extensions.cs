using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solr.net.CutomExtensions
{
    public static class Extensions
    {
        public static string ToDelimetedString(this IEnumerable<int> obj)
        {
            string[] array = obj.Where(n => n != null).Select(n => n.ToString()).ToArray();

            return string.Join(",", array);

        }
    }
}
