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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace org.apache.solr.SolrSharp.Configuration.Schema
{
    /// <summary>
    /// Maps a "field" node entry in a solr config.xml file to a strongly-typed object.
    /// While mapping all attributes, SolrField also provides a translation between solr 
    /// field class types and C# native types.
    /// </summary>
    public class SolrField
    {
        private SolrSchema solrschema;

        /// <summary>
        /// Gets an xpath-type query expression using the native .Net type and solr
        /// fieldname. Used by result and record classes in querying xml results.
        /// </summary>
        /// <param name="type">native .Net type to be evaluated</param>
        /// <param name="fieldname">solr fieldname to be used in the expression</param>
        /// <returns>string formed as an xpath-style query</returns>
        public static string GetXpathExpression(Type type, string fieldname)
        {
            string expression = SolrType.TypeExpression(type) + "[@name='" + fieldname + "']";
            if (type.IsArray)
            {
                Type arrayType = type.GetElementType();
                return expression += "/" + SolrType.TypeExpression(arrayType);
            }
            return expression;
        }

        /// <summary>
        /// Constructs an object by xpath query of an xml node representing a field in solr.
        /// </summary>
        /// <param name="xnSolrField">XmlNode representing one field</param>
        /// <param name="solrSchema">The underlying SolrSchema</param>
        public SolrField(XmlNode xnSolrField, SolrSchema solrSchema)
        {
            this.name = xnSolrField.Attributes["name"].Value;
            SolrType solrType = solrSchema.GetSolrType(xnSolrField.Attributes["type"].Value);
            if (solrType != null)
            {
                this.type = solrType.Type;
            }

            if (xnSolrField.Attributes["omitNorms"] != null)
            {
                this.omitNorms = Convert.ToBoolean(xnSolrField.Attributes["omitNorms"].Value);
            }
            if (xnSolrField.Attributes["indexed"] != null)
            {
                this.indexed = Convert.ToBoolean(xnSolrField.Attributes["indexed"].Value);
            }
            if (xnSolrField.Attributes["stored"] != null)
            {
                this.stored = Convert.ToBoolean(xnSolrField.Attributes["stored"].Value);
            }
            if (xnSolrField.Attributes["multiValued"] != null)
            {
                this.multiValued = Convert.ToBoolean(xnSolrField.Attributes["multiValued"].Value);
            }
            if (xnSolrField.Attributes["default"] != null)
            {
                this.isDefaulted = true;
            }
            this.solrschema = solrSchema;
        }

        private string name;
        /// <summary>
        /// The solr field name
        /// </summary>
        public string Name
        {
            get { return this.name; }
        }

        private Type type = null;
        /// <summary>
        /// The C# native type equivalent for the solr field
        /// </summary>
        public Type Type
        {
            get { return this.type; }
        }

        private bool omitNorms = false;
        /// <summary>
        /// When enabled in solr, OmitNorms disables length normalization and index-time 
        /// boosting for this field, which saves memory.  Only full-text fields or fields 
        /// that need an index-time boost need norms.
        /// </summary>
        public bool OmitNorms
        {
            get { return this.omitNorms; }
        }

        private bool indexed = false;
        /// <summary>
        /// When enabled, search queries can use this field for searching or sorting.
        /// </summary>
        public bool IsIndexed
        {
            get { return this.indexed; }
        }

        private bool stored = false;
        /// <summary>
        /// When enabled, search queries can retrieve this field in search results
        /// </summary>
        public bool IsStored
        {
            get { return this.stored; }
        }

        private bool multiValued = false;
        /// <summary>
        /// When enabled, this field may contain more than one value per document.
        /// </summary>
        public bool IsMultiValued
        {
            get { return this.multiValued; }
        }

        private string isCopiedName = null;
        private bool isCopied = false;
        private object syncLock = new object();
        /// <summary>
        /// When true, this field exists in the index by having values copied from a
        /// source field. This eliminates the requirement for a copied field to be
        /// included in an update request, as solr will take care of copying the values
        /// over from a source field to this field.
        /// </summary>
        public bool IsCopied
        {
            get
            {
                if (isCopiedName == null)
                {
                    lock (syncLock)
                    {
                        this.isCopiedName = "";
                        foreach (SolrCopyField solrCopyField in this.solrschema.SolrCopyFields)
                        {
                            if (solrCopyField.DestinationField == this)
                            {
                                this.isCopiedName = this.Name;
                                this.isCopied = true;
                                break;
                            }
                        }
                    }
                }
                return this.isCopied;
            }
        }

        private bool isDefaulted = false;
        /// <summary>
        /// When true, this field may contain a default value for inclusion in the index.
        /// This eliminates the requirement for a copied field to be included in an update 
        /// request, as solr will take care of generating the default value for the field,
        /// per the definition in the schema.xml file. If the field is present in an update
        /// request, the requested field value will be used.
        /// </summary>
        public bool IsDefaulted
        {
            get { return this.isDefaulted; }
        }

        /// <summary>
        /// An xpath-style expression for this instance.
        /// </summary>
        public string XpathExpression
        {
            get
            {
                if (this.IsMultiValued)
                {
                    return SolrField.GetXpathExpression(Array.CreateInstance(this.Type, 0).GetType(), this.Name);
                }
                return SolrField.GetXpathExpression(this.Type, this.Name);
            }
        }

    }
}
