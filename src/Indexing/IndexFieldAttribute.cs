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
using System.Reflection;
using System.Text;
using System.Xml;
using org.apache.solr.SolrSharp.Results;
using org.apache.solr.SolrSharp.Configuration.Schema;

namespace org.apache.solr.SolrSharp.Indexing
{
    /// <summary>
    /// Mapping attribute class that provides late-binding of a solr search query results xml
    /// payload to a derived instance of SearchRecord.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class IndexFieldAttribute : Attribute
    {

        /// <summary>
        /// Constructs an instance of an IndexFieldAttribute.  The fieldName parameter
        /// must use the equivalent SolrField name, as defined in the SolrSchema and found
        /// in a solr webapp instance schema.xml file.  The fieldName parameter is case-sensitive.
        /// </summary>
        /// <param name="fieldName">string representing the solr field name to map against</param>
        public IndexFieldAttribute(string fieldName)
        {
            this.fieldname = fieldName;
        }

        private string fieldname;
        /// <summary>
        /// The solr field name to map against
        /// </summary>
        public string FieldName
        {
            get { return this.fieldname; }
        }

        private PropertyInfo propertyInfo;
        /// <summary>
        /// Reflected property for this instance
        /// </summary>
        public PropertyInfo PropertyInfo
        {
            get { return this.propertyInfo; }
            set { this.propertyInfo = value; }
        }

        private string XnodeExpression
        {
            get
            {
                if (this.PropertyInfo != null)
                {
                    Type type = this.PropertyInfo.PropertyType;
                    return SolrField.GetXpathExpression(type, this.FieldName);
                }
                return null;
            }
        }

        /// <summary>
        /// Binds values to natively defined properties on an inherited instance of SearchRecord.
        /// </summary>
        /// <param name="searchRecord">Instance of SearchRecord to bind the values against</param>
        public void SetValue(SearchRecord searchRecord)
        {
            XmlNodeList xnlvalues = searchRecord.XNodeRecord.SelectNodes(this.XnodeExpression);
            if (xnlvalues.Count == 1)   //single value
            {
                XmlNode xnodevalue = xnlvalues[0];
                this.PropertyInfo.SetValue(searchRecord, Convert.ChangeType(xnodevalue.InnerText, this.PropertyInfo.PropertyType) , null);
            }
            else if (xnlvalues.Count > 1)   //array
            {
                Type basetype = this.PropertyInfo.PropertyType.GetElementType();
                Array valueArray = Array.CreateInstance(basetype, xnlvalues.Count);
                for (int i = 0; i < xnlvalues.Count; i++)
                {
                    valueArray.SetValue(Convert.ChangeType(xnlvalues[i].InnerText, basetype), i);
                }
                this.PropertyInfo.SetValue(searchRecord, valueArray, null);
            }
        }

    }
}
