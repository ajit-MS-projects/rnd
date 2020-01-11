using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Affilinet.Business.ProductImport.Cache;
using Affilinet.Business.ProductImport.Common;
using Affilinet.Business.ProductImport.Entity;
using Affilinet.Business.ProductImport.Processor;
using Affilinet.Exceptions;

namespace Affilinet.Business.ProductImport.Processor
{
    public class XMLProcessor : CSVProcessor
    {

        public XMLProcessor(DocumentAttributes docAttribs, CacheManager oCacheManager)
            : base(docAttribs, oCacheManager)
        {

        }

        #region IProcessor Members

        public override void ProcessDocumemt()
        {
            try
            {
                if (File.Exists(DocAttribs.FileFullPath))
                {
                    DocAttribs.ProgSettings.FieldSeperator = Constants.Generic.DestFieldSeperator;
                    DocAttribs.ProgSettings.TextQualifier = Constants.Generic.DestFieldQualifier;
                    DocAttribs.ProgSettings.FldSep = DocAttribs.ProgSettings.FieldSeperator[0];
                    if (string.IsNullOrEmpty(DocAttribs.ProgSettings.CategorySeperator))
                        DocAttribs.ProgSettings.CategorySeperator = Constants.Generic.XmlCategorySeperator;

                    NoOfLinesWithExtraColumns = 0;
                    OpenFileForRead();
                    base.PrepareHeader();
                    ReadDataLines();
                    base.FinishWritingCSVFiles(); //Write to csv files
                    //if (NoOfLinesWithExtraColumns > 0) DocAttribs.Report.Attention = " :: Number of columns in CSV are more than expected, total such lines:" + NoOfLinesWithExtraColumns;
                }
                else
                {
                    throw new AffiliGenericException("Source CSV File not found PPID:" + DocAttribs.ProdProgId + ":" + DocAttribs.FileFullPath, (int)ApplicationEventsEnum.DocProcessing);
                }
            }
            catch (AffiliBaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("Error in XMLProcessor.ProcessDocumemt() PPID:" + DocAttribs.ProdProgId, ex, (int)ApplicationEventsEnum.DocProcessing);
            }
            finally
            {
                if (SrCsvSource != null)
                {
                    SrCsvSource.Close();
                    SrCsvSource.Dispose();
                    SrCsvSource = null;
                }
            }
        }

        #endregion
        #region CSv Members overrides

        protected override bool ReadLineFromFile(out string strLine)
        {
            bool retVal = false;
            try
            {
                Dictionary<string, string> xmlProducts = new Dictionary<string, string>();
                xmlProducts.Clear();
                string nodeName = "";
                do
                {
                    if (XrXmlSource.NodeType == XmlNodeType.Element &&
                        XrXmlSource.Name.ToUpper() == DocAttribs.ProgSettings.XmlProductTag.ToUpper())
                    {
                        bool isRead = false;
                        retVal = true;
                        do
                        {
                            ReadAttributes(xmlProducts);

                            //Read tag value
                            while ((isRead = XrXmlSource.Read()))
                            {
                                if (XrXmlSource.NodeType == XmlNodeType.Text ||
                                    XrXmlSource.NodeType == XmlNodeType.CDATA)
                                {
                                    if (nodeName != "")
                                    {
                                        if (xmlProducts.ContainsKey(nodeName))
                                        {
                                            if (IsCategoryfield(nodeName))
                                                xmlProducts[nodeName] += DocAttribs.ProgSettings.CategorySeperator + XrXmlSource.Value;
                                            else
                                                xmlProducts[nodeName] += " " + XrXmlSource.Value;
                                        }
                                        else
                                            xmlProducts.Add(nodeName, XrXmlSource.Value.Trim());
                                    }
                                }
                                else if (XrXmlSource.NodeType == XmlNodeType.Element && XrXmlSource.Name != "")
                                {
                                    nodeName = XrXmlSource.Name.ToUpper();
                                    break;//For every element, read its attributes first and then it's text value(if present)
                                }
                            }
                        } while (isRead && XrXmlSource.NodeType != XmlNodeType.EndElement &&
                                 XrXmlSource.Name.ToUpper() != DocAttribs.ProgSettings.XmlProductTag.ToUpper());
                        break; //read 1 product only
                    } //if

                } while (XrXmlSource.Read());


                strLine = ConvertDictionaryToProductString(xmlProducts).ToString();
                xmlProducts.Clear();
            }
            catch (AffiliBaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("Error in XMLProcessor.ReadLineFromFile()", ex);
            }
            return retVal;
        }

        /// <summary>
        /// Determines whether the specified node name is categoryfield.
        /// </summary>
        /// <param name="nodeName">Name of the node.</param>
        /// <returns>
        /// 	<c>true</c> if the specified node name is categoryfield; otherwise, <c>false</c>.
        /// </returns>
        private bool IsCategoryfield(String nodeName)
        {
            foreach (ProgramFields field in DocAttribs.ProgFields)
            {
                if (field.Title.ToUpper().Equals(nodeName.ToUpper()) && field.DestField == Constants.Product.ProductCategoryID)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Reads the attributes.
        /// </summary>
        /// <param name="xmlProducts">The XML products.</param>
        private void ReadAttributes(Dictionary<string, string> xmlProducts)
        {
            if (XrXmlSource.HasAttributes)
            {
                while (XrXmlSource.MoveToNextAttribute())
                {
                    if (xmlProducts.ContainsKey(XrXmlSource.Name.ToUpper()))
                        xmlProducts[XrXmlSource.Name.ToUpper()] += DocAttribs.ProgSettings.CategorySeperator + XrXmlSource.Value;
                    else
                        xmlProducts.Add(XrXmlSource.Name.ToUpper(), XrXmlSource.Value.Trim());
                }
            }
        }

        /// <summary>
        /// Converts dictionary to product string line in the sequesnce of setting fields
        /// </summary>
        /// <param name="xmlProducts">The XML products.</param>
        private StringBuilder ConvertDictionaryToProductString(Dictionary<string, string> xmlProducts)
        {
            string f = DocAttribs.ProgSettings.FieldSeperator;
            string q = DocAttribs.ProgSettings.TextQualifier;
            StringBuilder str = new StringBuilder();
            foreach (ProgramFields objProgFld in DocAttribs.ProgFields)
            {

                if (xmlProducts.ContainsKey(objProgFld.Title.ToUpper()))
                {
                    str.Append((str.Length > 0 ? f : "") + q + xmlProducts[objProgFld.Title.ToUpper()] + q);
                }
                else
                {
                    str.Append((str.Length > 0 ? f : "") + q + "" + q);
                }
            }
            return str;
        }

        /// <summary>
        /// Opens the file for read.
        /// </summary>
        protected override void OpenFileForRead()
        {
            try
            {
                XrXmlSource = new XmlTextReader(new StreamReader(DocAttribs.FileFullPath, DocAttribs.ProgSettings.Encoding));
            }
            catch (AffiliBaseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("Error in XMLProcessor.OpenFileForRead()", ex);
            }
        }
        #endregion


    }
}