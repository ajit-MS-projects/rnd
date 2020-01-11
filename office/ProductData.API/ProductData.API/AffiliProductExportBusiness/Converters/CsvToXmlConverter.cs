using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Affilinet.Business.ProductExport.Common;
using Affilinet.Business.ProductExport.Entity;
using Affilinet.Exceptions;

namespace Affilinet.Business.ProductExport.Converters
{
    public class CsvToXmlConverter : BaseConverter
    {
        public CsvToXmlConverter(ExportAttributes objProgsFilesCache, bool isZipped, String outputFile) : base(objProgsFilesCache, isZipped, outputFile) { }
    
        protected override void WriteToOutputStream(Dictionary<String, String> targetproductFields)
        {
            try
            {
                StringBuilder destLine = new StringBuilder();
                destLine.Append(String.Format(Constants.ExportXMLTags.ProductStartTag, ObjExportAttributes.ProgSettings.ProgramID, targetproductFields[Constants.ExportProduct.ArtNumber]));
                foreach (String xmlGrp in ObjExportAttributes.XmlTagGroups)
                {
                    destLine.Append("<" + xmlGrp + ">");
                    IEnumerable<ExportColumn> objExportColumns = from item in ObjExportAttributes.ExportColumns
                                                                 where item.XmlGroup == xmlGrp
                                                                 select item;
                    foreach (ExportColumn objExpCol in objExportColumns)
                    {
                        if (targetproductFields.ContainsKey(objExpCol.ExportCsvColumn))
                        {
                            switch (xmlGrp)
                            {
                                case Constants.ExportXMLTagGroups.Images:
                                    PrepareImageFields(targetproductFields, destLine, objExpCol.ExportCsvColumn);
                                    break;
                                case Constants.ExportXMLTagGroups.Properties:
                                    if (!String.IsNullOrEmpty(targetproductFields[objExpCol.ExportCsvColumn]))
                                        targetproductFields[objExpCol.ExportCsvColumn] =
                                            targetproductFields[objExpCol.ExportCsvColumn].Replace(Constants.Generic.SourceCsvPropertyTag, Constants.Generic.TargetXmlPropertyTag);
                                    destLine.Append(targetproductFields[objExpCol.ExportCsvColumn]);
                                    break;
                                default:
                                    destLine.Append("<" + objExpCol.ExportXmlColumn + ">" + SanitizeValue(targetproductFields[objExpCol.ExportCsvColumn]) + "</" + objExpCol.ExportXmlColumn + ">");
                                    break;
                            }
                        }
                    }
                    destLine.Append("</" + xmlGrp + ">");

                }
                destLine.Append(Constants.ExportXMLTags.ProductEndTag);
                destLine.Append(Environment.NewLine);
                WriteToOutput(destLine.ToString());
            }
            catch(AffiliGenericException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("CsvToXmlConverter.WriteToOutputStream()", ex);
            }
        }
       
        private void PrepareImageFields(Dictionary<String, String> targetproductFields,StringBuilder destLine, String grpField)
        {
            try
            {
                String fieldInit = grpField.Replace("_url", "");
                if (targetproductFields.ContainsKey(grpField))
                {
                    destLine.Append(String.Format(Constants.ExportXMLTags.ImgTag, (fieldInit == "Img" ? "Adv" : fieldInit)));
                    destLine.Append(String.Format(Constants.ExportXMLTags.ImgUrlTag, SanitizeValue(targetproductFields[grpField])));
                }
                else
                    return;
                if(targetproductFields.ContainsKey(fieldInit + "Width"))
                    destLine.Append(String.Format(Constants.ExportXMLTags.ImgWidthTag,  SanitizeValue(targetproductFields[fieldInit + "Width"])));
                if (targetproductFields.ContainsKey(fieldInit + "Height"))
                    destLine.Append(String.Format(Constants.ExportXMLTags.ImgHeightTag, SanitizeValue(targetproductFields[fieldInit + "Height"])));
                destLine.Append("</Img>");
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("CsvToXmlConverter.PrepareImageFields()", ex);
            }
        }
        private String SanitizeValue(String fieldVal)
        {
            if (fieldVal == "null")
                fieldVal = String.Empty;
          return HttpUtility.HtmlEncode(fieldVal);
        }

        protected override void WriteFieldHeaders(Dictionary<String, String> targetproductFields)
        {
            try
            {
                try
                {
                    if (!IsZipped)
                        ObjExportAttributes.HttpContext.Response.AppendHeader(Constants.OutputHTTPHeaders.HeaderLastModified, ObjExportAttributes.ProgramFiles.ModifiedDateTime);
                }
                catch (Exception ex)
                {
                    new AffiliGenericException().CreateLog(ex.ToString());
                }
                IsHeaderWritten = true;
                String products = String.Format(Constants.ExportXMLTags.ProductsStartTag,
                                                ObjExportAttributes.ProgSettings.ProgramID,
                                                ObjExportAttributes.PublisherId);
                WriteToOutput(Constants.ExportXMLTags.XMLHeader);
                WriteToOutput(products);
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("CsvToXmlConverter.WriteFieldHeaders()", ex);
            }
        }

        protected override void WriteFileFooters()
        {
            try
            {
                WriteToOutput(Constants.ExportXMLTags.ProductsEndTag);
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("CsvToXmlConverter.WriteFileFooters()", ex);
            }
        }
    }
}
