using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using org.apache.solr.SolrSharp.Results;

namespace Example
{
    public class ExampleSearchResults : SearchResults<ExampleSearchRecord>
    {
        private ExampleCategoryFacetResults _categoryfacetresults = null;

        public ExampleSearchResults(ExampleQueryBuilder queryBuilder)
            : base(queryBuilder)
        {
        }

        protected override ExampleSearchRecord InitSearchRecord(XmlNode xn)
        {
            return new ExampleSearchRecord(xn);
        }

        public override void InitFacetResults(XmlNode xn)
        {
            this._categoryfacetresults = new ExampleCategoryFacetResults(xn);
        }

        public ExampleCategoryFacetResults ExampleCategoryFacetResults
        {
            get { return this._categoryfacetresults; }
        }

    }
}
