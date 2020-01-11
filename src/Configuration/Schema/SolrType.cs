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

namespace org.apache.solr.SolrSharp.Configuration.Schema
{
    /// <summary>
    /// Maps a "fieldtype" node entry in a solr config.xml file to a strongly-typed object.
    /// While mapping all attributes, the primary purpose of SolrType is to provide a translation
    /// between solr field class types and C# native types.
    /// </summary>
    public class SolrType
    {
        /// <summary>
        /// Generates a string value that identifies the solr "type" based on the
        /// native .Net type.
        /// </summary>
        /// <param name="type">.Net type to evaluate</param>
        /// <returns>string representing an equivalent solr "type"</returns>
        public static string TypeExpression(Type type)
        {
            if (type.IsArray)
            {
                return "arr";
            }
            else
            {
                string descriptor;
                if (!typeMap.TryGetValue(type, out descriptor))
                    throw new InvalidOperationException("Unrecognized type " + type + "!");
                return descriptor;
            }
            return null;
        }

        protected static readonly Dictionary<Type, string> typeMap;

        /// <summary>
        /// Initializes the type map
        /// </summary>
        static SolrType()
        {
            typeMap = new Dictionary<Type, string>();
            typeMap.Add(typeof(string), "str");
            typeMap.Add(typeof(int), "int");
            typeMap.Add(typeof(DateTime), "date");
            typeMap.Add(typeof(float), "float");
            typeMap.Add(typeof(bool), "bool");
            typeMap.Add(typeof(long), "long");

            // As Java doesn't recognize unsigned types, they are marshalled as 
            // signed types.
            typeMap.Add(typeof(uint), "int");
            typeMap.Add(typeof(ulong), "long");

            // (s)byte and (u)short aren't recognized by Solr, and are therefore
            // considered integers; users are cautioned that if such fields are
            // long-typed in the Solr schema they will not be found when parsing
            // the result XML. Additionally, overflow erros may result in
            // exceptions, so choose your types wisely!
            typeMap.Add(typeof(sbyte), "int");
            typeMap.Add(typeof(byte), "int");
            typeMap.Add(typeof(short), "int");
            typeMap.Add(typeof(ushort), "int");
        }

        /// <summary>
        /// Constructs an object by xpath query of an xml node representing a fieldtype in solr.
        /// </summary>
        /// <param name="xnSolrType">XmlNode representing one field type</param>
        public SolrType(XmlNode xnSolrType)
        {
            this.name = xnSolrType.Attributes["name"].Value;
            this.type = SolrSchema.GetNativeType(xnSolrType.Attributes["class"].Value);
            if (xnSolrType.Attributes["omitNorms"] != null)
            {
                this.omitNorms = Convert.ToBoolean(xnSolrType.Attributes["omitNorms"].Value);
            }
            if (xnSolrType.Attributes["sortMissingLast"] != null)
            {
                this.sortMissingLast = Convert.ToBoolean(xnSolrType.Attributes["sortMissingLast"].Value);
            }
            if (xnSolrType.Attributes["sortMissingFirst"] != null)
            {
                this.sortMissingLast = Convert.ToBoolean(xnSolrType.Attributes["sortMissingFirst"].Value);
            }
        }

        private string name;
        /// <summary>
        /// The solr fieldtype name
        /// </summary>
        public string Name
        {
            get { return this.name; }
        }

        private Type type;
        /// <summary>
        /// The C# native type equivalent for the solr type
        /// </summary>
        public Type Type
        {
            get { return this.type; }
        }

        private bool omitNorms = false;
        /// <summary>
        /// When enabled in solr, OmitNorms disables length normalization and index-time 
        /// boosting for a field of this type, which saves memory.  Only full-text fields 
        /// or fields that need an index-time boost need norms.
        /// </summary>
        public bool OmitNorms
        {
            get { return this.omitNorms; }
        }

        private bool sortMissingLast = false;
        /// <summary>
        /// When enabled, a sort on fields of this type will force documents without the 
        /// field to be listed in search results after documents with the field, regardless 
        /// of the requested sort order.
        /// </summary>
        public bool SortMissingLast
        {
            get { return this.sortMissingLast; }
        }

        private bool sortMissingFirst = false;
        /// <summary>
        /// When enabled, a sort on field of this type will force documents without the 
        /// field to be listed in search results before documents with the field, regardless of 
        /// the requested sort order.
        /// </summary>
        public bool SortMissingFirst
        {
            get { return this.sortMissingFirst; }
        }
    }
}
