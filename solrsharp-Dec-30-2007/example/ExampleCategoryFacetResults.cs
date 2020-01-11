using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using org.apache.solr.SolrSharp.Results;

namespace Example
{
    public class ExampleCategoryFacetResults : FacetResults<string>
    {

        public ExampleCategoryFacetResults(XmlNode xn)
            : base("cat", xn)
        {
        }

        protected override string InitFacetObject(object key)
        {
            return key.ToString();
        }
    }
}
