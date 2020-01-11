//
//   Licensed to the Apache Software Foundation (ASF) under one or more
//   contributor license agreements.  See the NOTICE file distributed with
//   this work for additional information regarding copyright ownership.
//   The ASF licenses this file to You under the Apache License, Version 2.0
//   (the "License"); you may not use this file except in compliance with
//   the License.  You may obtain a copy of the License at
//  
//       http://www.apache.org/licenses/LICENSE-2.0
//  
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using org.apache.solr.SolrSharp.Query;
using org.apache.solr.SolrSharp.Query.Highlights;
using org.apache.solr.SolrSharp.Configuration;
using org.apache.solr.SolrSharp.Configuration.Schema;

namespace org.apache.solr.SolrSharp.Results
{
    /// <summary>
    /// SearchResults is an abstraction that hides the details of evaluating a solr index search
    /// response, and returns the results in an object-based structure. SearchResults provides the
    /// base ExecuteSearch method as well as public metadata properties about the results of the
    /// query: TotalResults, the starting record (StartAt), and the number of Rows available for this
    /// set of results.
    /// 
    /// SearchResults requires an implementation utilizing generics. This enables the base class to
    /// build search result objects directly. Using generics, the base class populates SearchRecord
    /// results and Facet results.  Inheriting classes need only define how an inherited SearchRecord
    /// is created and a Facet results are created.  The base class executes those inherited methods
    /// during ExecuteSearch.
    /// </summary>
    /// <typeparam name="T">Class inheritance of type SearchRecord</typeparam>
	public abstract class SearchResults<T>
	{
		private int _totalresults;
		private int _startat;
		private int _rows;

        private List<T> _searchrecords = new List<T>();
        private XmlDocument _xmlresults;
		private XmlNodeList _xnlsearchrecords;
		private XmlNode _xnlfacetresults;
        private XmlNodeList _xnlhighlights;
        private List<HighlightRecord> _highlightrecords = new List<HighlightRecord>();
        private DebugResults _debugResults = null;
        private XmlNode _xndebugresults;

		private static readonly string XPATH_RECORDS = "response/result/doc";
		private static readonly string XPATH_STARTAT = "response/result/@start";
		private static readonly string XPATH_TOTALRESULTS = "response/result/@numFound";
		private static readonly string XPATH_FACETRESULTS = "response/lst[@name='facet_counts']/lst[@name='facet_fields']";
        private static readonly string XPATH_HIGHLIGHTING = "response/lst[@name='highlighting']/lst";
        private static readonly string XPATH_DEBUG = "response/lst[@name='debug']";

        ///// <summary>
        ///// Empty public constructor
        ///// </summary>
        //public SearchResults()
        //{
        //}

        /// <summary>
        /// Generates search results for the QueryBuilder object. Using this constructor,
        /// search results are immediately available after control is returned to the calling
        /// client application.
        /// </summary>
        /// <param name="queryBuilder">QueryBuilder object to be executed as a search request</param>
		public SearchResults(QueryBuilder queryBuilder)
        {
            this.ExecuteSearch(queryBuilder);
        }

        /// <summary>
        /// Executes a search request using the passed QueryBuilder object. This method populates
        /// the metadata properties about the search (total results, start-at, and available rows).
        /// Additionally, initialization routines for the given type of SearchRecord and any Facet
        /// results are called.
        /// </summary>
        /// <param name="queryBuilder">QueryBuilder object to be executed as a search request</param>
        protected void ExecuteSearch(QueryBuilder queryBuilder)
		{
            this._xmlresults = SolrSearcher.GetXmlDocumentFromPost(queryBuilder.SOLR_SEARCH, queryBuilder.QueryUrl);
            
			if (this._xmlresults != null)
			{
                this._totalresults = Convert.ToInt32(SolrSearcher.GetXmlValue(this._xmlresults, SearchResults<T>.XPATH_TOTALRESULTS));
                this._startat = Convert.ToInt32(SolrSearcher.GetXmlValue(this._xmlresults, SearchResults<T>.XPATH_STARTAT));
                this._xnlsearchrecords = SolrSearcher.GetXmlNodes(this._xmlresults, SearchResults<T>.XPATH_RECORDS);
                this._xnlfacetresults = SolrSearcher.GetXmlNode(this._xmlresults, SearchResults<T>.XPATH_FACETRESULTS);
				this._rows = this._xnlsearchrecords.Count;

                #region Evaluate highlighting
                if (queryBuilder.IsHighlighted)
                {
                    this._xnlhighlights = SolrSearcher.GetXmlNodes(this._xmlresults, SearchResults<T>.XPATH_HIGHLIGHTING);
                    if (this._xnlhighlights != null)
                    {
                        foreach (XmlNode xn in this._xnlhighlights)
                        {
                            this._highlightrecords.Add(new HighlightRecord(xn));
                        }
                    }
                }
                #endregion

                #region Instantiate each SearchRecord instance, per the Generic "T" type
                foreach (XmlNode xn in this._xnlsearchrecords)
                {
                    #region Substitute highlighting, if applicable
                    if (this._highlightrecords.Count > 0)
                    {
                        SolrField solrField_default =
                            queryBuilder.SolrSearcher.SolrSchema.GetSolrField(queryBuilder.SolrSearcher.SolrSchema.UniqueKey);
                        string xnPath = string.Format("{0}[@name='{1}']", SolrType.TypeExpression(solrField_default.Type), solrField_default.Name);
                        foreach (HighlightRecord hr in this._highlightrecords)
                        {
                            if (hr.RecordId == Convert.ToString(SolrSearcher.GetXmlValue(xn, xnPath)))
                            {
                                foreach (HighlightParameter hp in queryBuilder.GetHighlightParameterCollection())
                                {
                                    string[] hilitephrases = hr.GetHighlightedPhrases(hp.SolrField.Name);
                                    foreach (string phrase in hilitephrases)
                                    {
                                        //how to substitute?
                                        string stripped = phrase.Replace(hp.SimplePreText, "");
                                        stripped = stripped.Replace(hp.SimplePostText, "");

                                        //find the matching phrase in xn
                                        XmlNodeList xnlSwap = SolrSearcher.GetXmlNodes(xn, hp.SolrField.XpathExpression);
                                        foreach (XmlNode xnSwap in xnlSwap)
                                        {
                                            if (xnSwap.InnerText == stripped)
                                            {
                                                xnSwap.InnerText = phrase;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    this._searchrecords.Add(this.InitSearchRecord(xn));
                }
                #endregion

                #region Instantiate FacetRecord instances, if populated
                if (queryBuilder.Facets.Length > 0)
                {
                    this.InitFacetResults(this._xnlfacetresults);
                }
                #endregion

                #region Evaluate debug parameters
                if (queryBuilder.IsDebugEnabled)
                {
                    this._xndebugresults = SolrSearcher.GetXmlNode(this._xmlresults, SearchResults<T>.XPATH_DEBUG);
                    this._debugResults = new DebugResults(this._xndebugresults);
                }
                #endregion

            }
		}

        /// <summary>
        /// This method is called by ExecuteSearch in constructing type-specific objects
        /// of type SearchRecord.  This constructs the collection of SearchRecords automatically,
        /// using the definition in the inherited object.  Required implementation by the inheriting class.
        /// </summary>
        /// <param name="xn">XmlNode containing fields used for the resultant SearchRecord</param>
        /// <returns>SearchRecord of type T (the inherited type)</returns>
		protected abstract T InitSearchRecord(XmlNode xn);

        /// <summary>
        /// This method is called by ExecuteSearch in constructing type-specific objects
        /// of type FacetResults.  This constructs the collection of FacetResults automatically,
        /// using the definition in the inherited object.  Required implementation by the inheriting class.
        /// </summary>
        /// <param name="xn">XmlNode containing fields used for the resultant FacetResult</param>
        public abstract void InitFacetResults(XmlNode xn);

        /// <summary>
        /// The number of total results for this search request
        /// </summary>
		public int TotalResults
		{
			get { return this._totalresults; }
		}

        /// <summary>
        /// The starting record (zero-based) for this set of results
        /// </summary>
		public int StartAt
		{
			get { return this._startat; }
		}

        /// <summary>
        /// The number of rows returns for this set of results
        /// </summary>
		public int Rows
		{
			get { return this._rows; }
		}

        /// <summary>
        /// Type-specific set of SearchRecords, representing the results for this page.
        /// </summary>
        public T[] SearchRecords
        {
            get { return this._searchrecords.ToArray(); }
        }

        /// <summary>
        /// If the QueryBuilder object that constructs this SearchResults instance 
        /// has the <see cref="org.apache.solr.SolrSharp.Query.QueryBuilder.IsDebugEnabled"/>IsDebugEnabled property set to true, this object will be created
        /// (not null).
        /// </summary>
        public DebugResults DebugResults
        {
            get { return this._debugResults; }
        }

	}
}
