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
using org.apache.solr.SolrSharp.Configuration;
using org.apache.solr.SolrSharp.Indexing;
using org.apache.solr.SolrSharp.Query.Parameters;

namespace org.apache.solr.SolrSharp.Configuration.Schema
{
    /// <summary>
    /// Maps the schema for a SolrServer instance.
    /// </summary>
    public class SolrSchema
    {
        private List<SolrType> solrTypes = new List<SolrType>();
        private List<SolrField> solrFields = new List<SolrField>();
        private List<SolrDynamicField> solrDynamicFields = new List<SolrDynamicField>();
        private List<SolrCopyField> solrCopyFields = new List<SolrCopyField>();

        /// <summary>
        /// Creates an instance of a strongly-typed schema for a given solr webapp instance.
        /// </summary>
        /// <param name="solrSearcher">Underlying SolrSearcher for this schema</param>
        public SolrSchema(SolrSearcher solrSearcher)
        {
            XmlDocument xdocConfig = solrSearcher.GetConfigurationXml(ConfigurationFile.Schema);
            XmlNodeList xnlTypes = xdocConfig.SelectNodes("schema/types/fieldType");
            foreach (XmlNode xnSolrType in xnlTypes)
            {
                SolrType solrType = new SolrType(xnSolrType);
                this.solrTypes.Add(solrType);
            }

            XmlNodeList xnlFields = xdocConfig.SelectNodes("schema/fields/field");
            foreach (XmlNode xnField in xnlFields)
            {
                SolrField solrField = new SolrField(xnField, this);
                this.solrFields.Add(solrField);
            }

            XmlNodeList xnlDynamicFields = xdocConfig.SelectNodes("schema/fields/dynamicField");
            foreach (XmlNode xnDynamicField in xnlDynamicFields)
            {
                SolrDynamicField solrDynamicField = new SolrDynamicField(xnDynamicField, this);
                this.solrDynamicFields.Add(solrDynamicField);
            }

            XmlNodeList xnlCopyFields = xdocConfig.SelectNodes("schema/copyField");
            foreach (XmlNode xnCopyField in xnlCopyFields)
            {
                SolrCopyField solrCopyField = new SolrCopyField(xnCopyField, this.SolrFields);
                this.solrCopyFields.Add(solrCopyField);
            }

            if (xdocConfig.SelectSingleNode("schema/uniqueKey") != null)
            {
                this.uniqueKey = xdocConfig.SelectSingleNode("schema/uniqueKey").InnerText;
            }
            this.defaultSearchField = xdocConfig.SelectSingleNode("schema/defaultSearchField").InnerText;
            this.defaultOperator = (ParameterJoin)Enum.Parse(typeof(ParameterJoin), xdocConfig.SelectSingleNode("schema/solrQueryParser/@defaultOperator").InnerText);
        }

        /// <summary>
        /// Read-only array of SolrType objects for this solr instance
        /// </summary>
        public SolrType[] SolrTypes
        {
            get { return this.solrTypes.ToArray(); }
        }

        /// <summary>
        /// Returns a SolrType instance whose name matches the fieldType.
        /// </summary>
        /// <param name="fieldType">string field name to retrieve from this schema</param>
        /// <returns>SolrType</returns>
        public SolrType GetSolrType(string fieldType)
        {
            foreach (SolrType solrType in this.SolrTypes)
            {
                if (solrType.Name == fieldType)
                {
                    return solrType;
                }
            }
            return null;
        }

        /// <summary>
        /// Read-only array of SolrField objects for this solr instance
        /// </summary>
        public SolrField[] SolrFields
        {
            get { return this.solrFields.ToArray(); }
        }

        /// <summary>
        /// Gets the SolrField instance for the referenced fieldname
        /// </summary>
        /// <param name="fieldname"></param>
        /// <returns></returns>
        public SolrField GetSolrField(string fieldname)
        {
            foreach (SolrField sf in this.solrFields)
            {
                if (sf.Name == fieldname)
                {
                    return sf;
                }
            }
            return null;
        }

        /// <summary>
        /// Read-only array of SolrDynamicField objects for this solr instance
        /// </summary>
        public SolrDynamicField[] SolrDynamicFields
        {
            get { return this.solrDynamicFields.ToArray(); }
        }

        private SolrDynamicField GetMatchingSolrDynamicField(string fieldname)
        {
            foreach (SolrDynamicField solrDynamicField in this.solrDynamicFields)
            {
                if (solrDynamicField.IsMatch(fieldname))
                {
                    return solrDynamicField;
                }
            }
            return null;
        }

        /// <summary>
        /// Read-only array of SolrCopyField objects for this solr instance
        /// </summary>
        public SolrCopyField[] SolrCopyFields
        {
            get { return this.solrCopyFields.ToArray(); }
        }

        private string uniqueKey;
        /// <summary>
        /// The field to use to determine and enforce document uniqueness.
        /// </summary>
        public string UniqueKey
        {
            get { return this.uniqueKey; }
        }

        private string defaultSearchField;
        /// <summary>
        /// Field for the QueryParser to use when an explicit fieldname is absent
        /// in a search query
        /// </summary>
        public string DefaultSearchField
        {
            get { return this.defaultSearchField; }
        }

        private ParameterJoin defaultOperator = ParameterJoin.OR;
        /// <summary>
        /// Specifies a default query join basis, if absent in search queries
        /// </summary>
        public ParameterJoin DefaultOperator
        {
            get { return this.defaultOperator; }
        }

        /// <summary>
        /// Returns the C# native type for the given solr class name
        /// </summary>
        /// <param name="solrClass">string representing the solr class name</param>
        /// <returns>C# native Type</returns>
        public static Type GetNativeType(string solrClass)
        {
            Type type = null;
            switch (solrClass)
            {
                case "solr.TextField":
                case "solr.StrField":
                    type = typeof(System.String);
                    break;
                case "solr.BoolField":
                    type = typeof(System.Boolean);
                    break;
                case"solr.SortableIntField":
                case "solr.IntField":
                    type = typeof(System.Int16);
                    break;
                case "solr.SortableLongField":
                case "solr.LongField":
                    type = typeof(System.Int32);
                    break;
                case "solr.SortableFloatField":
                case "solr.FloatField":
                    type = typeof(System.Single);
                    break;
                case "solr.SortableDoubleField":
                case "solr.DoubleField":
                    type = typeof(System.Double);
                    break;
                case "solr.DateField":
                    type = typeof(System.DateTime);
                    break;
            }
            return type;
        }

        /// <summary>
        /// When true, indicates an UpdateIndexDocument is valid for application against
        /// this schema.
        /// </summary>
        /// <param name="updateIndexDocument">UpdateIndexDocument instance</param>
        /// <returns>bool</returns>
        public bool IsValidUpdateIndexDocument(UpdateIndexDocument updateIndexDocument)
        {
            List<string> docfieldnames = new List<string>();
            foreach (IndexFieldValue indexFieldValue in updateIndexDocument.FieldValues)
            {
                if (!docfieldnames.Contains(indexFieldValue.Name))
                {
                    docfieldnames.Add(indexFieldValue.Name);
                }
            }
            foreach (SolrField solrField in this.solrFields)
            {
                if ((!solrField.IsCopied) && 
                    (!solrField.IsDefaulted) && 
                    (!docfieldnames.Contains(solrField.Name))
                    )
                {
                    return false;
                }
                else
                {
                    docfieldnames.Remove(solrField.Name);
                }
            }
            if (docfieldnames.Count > 0)
            {
                foreach (string fieldname in docfieldnames)
                {
                    if (this.GetMatchingSolrDynamicField(fieldname) == null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

    }
}
