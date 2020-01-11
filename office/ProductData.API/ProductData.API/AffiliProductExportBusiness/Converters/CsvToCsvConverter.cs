using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Affilinet.Business.ProductExport.Common;
using Affilinet.Business.ProductExport.Entity;
using Affilinet.Exceptions;

namespace Affilinet.Business.ProductExport.Converters
{
    public class CsvToCsvConverter : BaseConverter
    {

        public CsvToCsvConverter(ExportAttributes objExportAttributes, bool isZipped, String outputFile) : base(objExportAttributes, isZipped, outputFile) { }
        

        protected override void WriteToOutputStream(Dictionary<String, String> targetproductFields)
        {
            try
            {
                List<String> targetVals = targetproductFields.Values.ToList();
                List<String> targetCols = targetproductFields.Keys.ToList();
                StringBuilder destLine = new StringBuilder();
                for (int i = 0; i < targetVals.Count; i++)
                {
                    if (IsNumeric(targetCols[i]))// || targetVals[i]=="null")
                        destLine.Append(targetVals[i] +
                                    (i < (targetVals.Count - 1) ? ObjExportAttributes.PubSettings.FieldSeperator : ""));
                    else
                    {
                        if (targetCols[i] == Constants.ExportProduct.Properties)
                            CreateColumnsFromPropertyXml(destLine, targetproductFields, PropertiesAttributesEnum.TextOrNumber);
                        else
                            destLine.Append(ObjExportAttributes.PubSettings.FieldQualifier + targetVals[i] +
                                    ObjExportAttributes.PubSettings.FieldQualifier +
                                    (i < (targetVals.Count - 1) ? ObjExportAttributes.PubSettings.FieldSeperator : ""));
                    }
                }
                String strDestLine = destLine.ToString();
                if (strDestLine.EndsWith(ObjExportAttributes.PubSettings.FieldSeperator))
                    strDestLine = strDestLine.Remove(strDestLine.Length - 1, 1);
                WriteToOutput(strDestLine + Environment.NewLine);
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("CsvToCsvConverter.WriteToOutputStream()", ex);
            }
        }

        private bool IsNumeric(String exportCsvCol)
        {
            IEnumerable<ExportColumn> objExportColumns = from item in ObjExportAttributes.ExportColumns
                                                   where item.ExportCsvColumn == exportCsvCol
                                                   select item;
            foreach (ExportColumn objExpCol in objExportColumns)
            {
                String colDataType = objExpCol.DataType.ToUpper();
                if (colDataType == "DECIMAL" || colDataType == "INT" || colDataType == "BIGINT")
                    return true;
            }
            return false;
        }

        protected override void WriteFieldHeaders(Dictionary<String, String> targetproductFields)
        {
            try
            {
                try
                {
                    if (!IsZipped)
                        ObjExportAttributes.HttpContext.Response.AppendHeader(Constants.OutputHTTPHeaders.HeaderLastModified, ObjExportAttributes.ProgramFiles.ModifiedDateTime);
                }catch(Exception ex)
                {
                    new AffiliGenericException().CreateLog(ex.ToString());
                }
                IsHeaderWritten = true;
                StringBuilder destLine = new StringBuilder();

                for (int i = 0; i < ObjExportAttributes.ExportColumns.Count; i++)
                {
                    if (!IgnoreCol(ObjExportAttributes.ExportColumns[i]))
                        destLine.Append(ObjExportAttributes.PubSettings.FieldQualifier + ObjExportAttributes.ExportColumns[i].ExportCsvColumn +
                                        ObjExportAttributes.PubSettings.FieldQualifier +
                                        (i < (ObjExportAttributes.ExportColumns.Count - 1) ? ObjExportAttributes.PubSettings.FieldSeperator : ""));
                }


                if (targetproductFields!=null)
                    CreateColumnsFromPropertyXml(destLine, targetproductFields, PropertiesAttributesEnum.Title);
                WriteToOutput(destLine + Environment.NewLine);
                destLine.Remove(0, destLine.Length);
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("CsvToCsvConverter.WriteFieldHeaders()", ex);
            }
        }

        private void CreateColumnsFromPropertyXml(StringBuilder destLine, Dictionary<string, string> targetproductFields, PropertiesAttributesEnum attributeIndex)
        {
            /*SJI: ObjExportAttributes.PubSettings.FieldSeperator +  removed from the destLine.Replace function below
            as this was causing a problem of missing field separator in the CSV header.
            */
            if (attributeIndex == PropertiesAttributesEnum.Title)//Properties from header
                destLine.Replace(ObjExportAttributes.PubSettings.FieldQualifier +
                                 Constants.ExportProduct.Properties + ObjExportAttributes.PubSettings.FieldQualifier, String.Empty);
            if (/*destLine.ToString().Contains(Constants.ExportProduct.Properties)&&*/
                targetproductFields.ContainsKey(Constants.ExportProduct.Properties) &&
                targetproductFields[Constants.ExportProduct.Properties]!="null")
            {
                
                String properties = targetproductFields[Constants.ExportProduct.Properties];
                if(!String.IsNullOrEmpty(properties))
                {
                    properties = String.Format(Constants.ExportXMLTags.PropertiesRootTag, properties);
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.LoadXml(properties);
                    foreach(XmlNode xNode in xDoc)
                    {
                        foreach (XmlNode xChildNode in xNode)
                        {
                            if (xChildNode.Attributes[(int)attributeIndex].Name.ToUpper() == Constants.Generic.PropertiesXmlNumericAttributeName)
                                destLine.Append(xChildNode.Attributes[(int)attributeIndex].Value + ObjExportAttributes.PubSettings.FieldSeperator);
                            else
                                destLine.Append(ObjExportAttributes.PubSettings.FieldQualifier +
                                            xChildNode.Attributes[(int)attributeIndex].Value +
                                            ObjExportAttributes.PubSettings.FieldQualifier + ObjExportAttributes.PubSettings.FieldSeperator);
                        }
                    }
                }
            }
            // SJI: Added to remove the last extra field separator in the end of each row.
            destLine.Remove(destLine.Length - 1, 1);
        }

        protected override void WriteFileFooters()
        {
            
        }
     
    }

    

}
