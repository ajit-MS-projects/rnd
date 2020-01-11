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
using Affilinet.Tools.GZIP;
using CommonUtilities = Affili.ProductData.Common.Utilities;
using System.Globalization;

namespace Affilinet.Business.ProductExport.Converters
{
    public abstract class BaseConverter
    {
        protected ExportAttributes ObjExportAttributes { get; set; }
        protected StreamReader SrCsvSource { get; set; }
        protected List<String> SourceCsvHeader = null;
        protected List<String> ColumnsDefaultWithXXX = null;
        public bool IsZipped { get; set; }
        protected NamedGZipStream ZippedStream {get; set;}
        protected StreamWriter ZippedTextWriter { get; set; }
        protected bool IsHeaderWritten  { get; set; }
        private Dictionary<String, String> targetproductFields = null;
        protected BaseConverter(ExportAttributes objExportAttributes, bool isZipped, String outputFile)
        {
            ObjExportAttributes = objExportAttributes;

            this.IsZipped = isZipped;
            if (IsZipped)
            {
                ZippedStream = new NamedGZipStream(ObjExportAttributes.HttpContext.Response.OutputStream, outputFile, true);
                ZippedTextWriter = new StreamWriter(ZippedStream);
            }
            ColumnsDefaultWithXXX = new List<string>();
        }
        #region Virtual Methods
        public virtual void ProcessDocumemt()
        {
            try
            {
                int fileNumber = 0;
                FillColumnsDefaultWithOtherColumn();
                if (ObjExportAttributes.ProgramFiles == null || ObjExportAttributes.ProgramFiles.FileNames == null || ObjExportAttributes.ProgramFiles.FileNames.Count == 0)
                    WriteFieldHeaders(null);
                else
                {
                    foreach (String fileName in ObjExportAttributes.ProgramFiles.FileNames)
                    {
                        OpenFileForRead(fileName);
                        WriteDataLines(fileNumber);
                        fileNumber++;
                    } //end foreach
                }
                if (!IsHeaderWritten) WriteFieldHeaders(null);
                WriteFileFooters();
            }
            catch(AffiliGenericException ex)
            {
                throw new AffiliGenericException(ex.Message + GetExceptionInfo(), ex.InnerException);
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("BaseConverter.ProcessDocumemt()" + GetExceptionInfo(),ex);
            }
            finally
            {
                if (ZippedStream != null)
                {
                    ZippedStream.Flush();
                 //   ZippedStream.Close();
                }
                ObjExportAttributes.HttpContext.Response.Flush();
                //ObjExportAttributes.HttpContext.Response.End();
                if (ZippedTextWriter != null)
                {
                    ZippedTextWriter.Flush();
                    ZippedTextWriter.Close();
                }
                if (SrCsvSource != null)
                {
                    SrCsvSource.Close();
                    SrCsvSource.Dispose();
                    SrCsvSource = null;
                }
            }
        }
        #endregion
        #region Abstract Methods
        protected abstract void WriteToOutputStream(Dictionary<String, String> targetproductFields);
        protected abstract void WriteFieldHeaders(Dictionary<String, String> targetproductFields);
        protected abstract void WriteFileFooters();
        #endregion

        protected void WriteDataLines(int fileNumber)
        {
            try
            {
                String strLine = String.Empty;
                if (fileNumber > 0)
                    SrCsvSource.ReadLine();
                      
                while ((strLine = SrCsvSource.ReadLine()) != null)
                {
                    if (SourceCsvHeader == null) { CreateSourceCsvColumns(strLine); continue; }//Skiping the header of source csv
                    
                    if (!String.IsNullOrEmpty(strLine))
                    {
                        Dictionary<String, String> SourceProductFields = GetSourceCsvFields(strLine);
                        targetproductFields = new Dictionary<string, string>();

                        foreach (ExportColumn objExportColumn in ObjExportAttributes.ExportColumns)
                        {
                            if (!IgnoreCol(objExportColumn))
                            {
                                String fieldValue = String.Empty;
                                if (SourceProductFields.ContainsKey(objExportColumn.ProductTableColumn))
                                    fieldValue = SourceProductFields[objExportColumn.ProductTableColumn];
                                fieldValue = PrepareField(objExportColumn, fieldValue, SourceProductFields);
                                targetproductFields.Add(objExportColumn.ExportCsvColumn, fieldValue);
                            }
                        }
                       
                        SetMiscelleneousFields(SourceProductFields, targetproductFields);
                        if (!IsHeaderWritten) WriteFieldHeaders(targetproductFields);
                        WriteToOutputStream(targetproductFields);
                        SourceProductFields.Clear();
                        targetproductFields.Clear();
                    }
                } //end while

            }
            catch(AffiliGenericException ex)
            {
                throw ex;    
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("BaseConverter.WriteDataLines()", ex);
            }
        }

        
              
        private void FillColumnsDefaultWithOtherColumn()
        {
            foreach (ExportColumn tmpExpCols in ObjExportAttributes.ExportColumns)
            {
                if (tmpExpCols.IsDefault || //Cols which are mendatory to transmit
                    ObjExportAttributes.PubSettings.Columns.Contains(tmpExpCols.ExportCsvColumn.ToUpper())) //Cols selected by publisher)
                {
                    foreach (String col in tmpExpCols.ColumnsDefault)
                    {
                        ColumnsDefaultWithXXX.Add(col);
                    }
                }
            }
        }
        

        protected void SetMiscelleneousFields(Dictionary<String, String> SourceProductFields, Dictionary<String, String> targetproductFields)
        {
            if (targetproductFields.ContainsKey(Constants.ExportProduct.Deeplink1))
                targetproductFields[Constants.ExportProduct.Deeplink1] = GetDeepLink(Constants.Product.DeepLink1, SourceProductFields);
            
            if (targetproductFields.ContainsKey(Constants.ExportProduct.Deeplink2))
                targetproductFields[Constants.ExportProduct.Deeplink2] = GetDeepLink(Constants.Product.DeepLink2, SourceProductFields);

            //Set disp shipping to empty if shipping is empty
            if (targetproductFields.ContainsKey(Constants.ExportProduct.DisplayShipping) &&
                SourceProductFields.ContainsKey(Constants.ExportProduct.Shipping) &&
                (String.IsNullOrEmpty(SourceProductFields[Constants.ExportProduct.Shipping]) || SourceProductFields[Constants.ExportProduct.Shipping] == "null"))
                targetproductFields[Constants.ExportProduct.DisplayShipping] = ObjExportAttributes.PubSettings.ReplaceEmptyWithNull?"null":String.Empty;

            //Set disp price to empty if price is empty
            if (targetproductFields.ContainsKey(Constants.ExportProduct.DisplayPrice) &&
                SourceProductFields.ContainsKey(Constants.ExportProduct.Price) &&
                (String.IsNullOrEmpty(SourceProductFields[Constants.ExportProduct.Price]) || SourceProductFields[Constants.ExportProduct.Price] == "null"))
                targetproductFields[Constants.ExportProduct.DisplayPrice] = ObjExportAttributes.PubSettings.ReplaceEmptyWithNull ? "null" : String.Empty;

            //Set disp base price to empty if base price is empty
            if (targetproductFields.ContainsKey(Constants.ExportProduct.DisplayBasePrice) &&
                SourceProductFields.ContainsKey(Constants.ExportProduct.BasePrice) &&
                (String.IsNullOrEmpty(SourceProductFields[Constants.ExportProduct.BasePrice]) || SourceProductFields[Constants.ExportProduct.BasePrice] == "null"))
                targetproductFields[Constants.ExportProduct.DisplayBasePrice] = ObjExportAttributes.PubSettings.ReplaceEmptyWithNull ? "null" : String.Empty;


            if (targetproductFields.ContainsKey(Constants.ExportProduct.ProgramID))
                targetproductFields[Constants.ExportProduct.ProgramID] = ObjExportAttributes.ProgSettings.ProgramID.ToString();
            
            if(targetproductFields.ContainsKey(Constants.ExportProduct.ProductProgramID))
                targetproductFields[Constants.ExportProduct.ProductProgramID] = ObjExportAttributes.ProgramFiles.ProdProgId;

        }
        
        #region Output fields prepration
        protected String PrepareField(ExportColumn objExportColumn, String fieldValue, Dictionary<String, String> SourceProductFields)
        {
            try
            {
                
                    fieldValue = fieldValue.Trim();
                    //Start: if a specific value is setup for this particular field, use that
                    if (!String.IsNullOrEmpty(objExportColumn.ColumnValue))
                        return objExportColumn.ColumnValue;
                    //End: if a specific value is setup for this particular field, use that

                    //Start: if in config defined that a cobination of culmns make value for this field 
                    if (objExportColumn.IsCalculated)
                        fieldValue = PrepareCombinationOfFields(objExportColumn, SourceProductFields);
                    //End: if in config defined that a cobination of culmns make value for this field 

                    if (fieldValue == null) fieldValue = string.Empty;
                    switch (objExportColumn.ExportCsvColumn)
                    {
                        case Constants.ExportProduct.Price:
                        case Constants.ExportProduct.Price_old:
                        case Constants.ExportProduct.Shipping:
                        case Constants.ExportProduct.DisplayShipping:
                        case Constants.ExportProduct.DisplayPrice:
                            fieldValue = fieldValue.Replace(",", String.Empty);
                            fieldValue = fieldValue.Replace(".", ObjExportAttributes.PubSettings.DeciSeperator);
                            break;
                        case Constants.ExportProduct.Img_Url:
                            switch(ObjExportAttributes.ProgSettings.ImageNumToUse)
                            {
                                case 1:
                                    fieldValue = SourceProductFields[Constants.Product.ImageURL1];
                                    break;
                                case 2:
                                    fieldValue = SourceProductFields[Constants.Product.ImageURL2];
                                    break;
                                case 3:
                                    fieldValue = SourceProductFields[Constants.Product.ImageURL3];
                                    break;
                            }
                            break;
                        case Constants.ExportProduct.img30_url:
                        case Constants.ExportProduct.img60_url:
                        case Constants.ExportProduct.img90_url:
                        case Constants.ExportProduct.img120_url:
                        case Constants.ExportProduct.img180_url:
                            String imageId = SourceProductFields[Constants.Product.ImageID];
                            fieldValue = GetImageUrl(imageId, SourceProductFields, GetImageUrlPrefix(objExportColumn.ExportCsvColumn));
                            break;
                        case Constants.ExportProduct.valid_to:
                        case Constants.ExportProduct.valid_from:
                        case Constants.ExportProduct.update_date:
                            fieldValue = GetFormatedDate(fieldValue);
                            break;
                        case Constants.ExportProduct.ImgWidth:
                        case Constants.ExportProduct.ImgHeight:
                        case Constants.ExportProduct.Img30Width:
                        case Constants.ExportProduct.Img30Height:
                        case Constants.ExportProduct.Img60Width:
                        case Constants.ExportProduct.Img60Height:
                        case Constants.ExportProduct.Img90Width:
                        case Constants.ExportProduct.Img90Height:
                        case Constants.ExportProduct.Img120Width:
                        case Constants.ExportProduct.Img120Height:
                        case Constants.ExportProduct.Img180Width:
                        case Constants.ExportProduct.Img180Height:
                            if (fieldValue == "-1") fieldValue = String.Empty;
                            break;
                        case Constants.ExportProduct.AffilinetProductCategoryID:
                            if (string.IsNullOrEmpty(fieldValue)) fieldValue = "0";
                            break;
                    }


                if (ObjExportAttributes.PubSettings.ReplaceEmptyWithNull && String.IsNullOrEmpty(fieldValue))
                    fieldValue = "null";

                return fieldValue;
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("BaseConverter.PrepareField()", ex);
            }
        }

        private String GetFormatedDate(String fieldValue)
        {
            String result = String.Empty;
            if(!String.IsNullOrEmpty(fieldValue))
            {
                //DateTime cdate = Convert.ToDateTime(fieldValue.Trim());
                // added a fixed culture because of problems on german test-machines
                DateTime cdate = DateTime.ParseExact(fieldValue, "M/d/yyyy h:m:s tt", CultureInfo.CreateSpecificCulture("en-US"));

                switch (ObjExportAttributes.PubSettings.DateFormat)
                {
                    case "1":
                        result = String.Format("{0:MM}/{0:dd}/{0:yy}", cdate);
                        break;
                    case "2":
                        result = String.Format("{0:yy}.{0:MM}.{0:dd}", cdate);
                        break;
                    case "3":
                        result = String.Format("{0:dd}/{0:MM}/{0:yy}", cdate);
                        break;
                    case "4":
                        result = String.Format("{0:dd}.{0:MM}.{0:yy}", cdate);
                        break;
                    case "5":
                        result = String.Format("{0:dd}-{0:MM}-{0:yy}", cdate);
                        break;
                    case "6":
                        result = String.Format("{0:dd} {0:MMM} {0:yy}", cdate);
                        break;
                    case "10":
                        result = String.Format("{0:MM}-{0:dd}-{0:yy}", cdate);
                        break;
                    case "11":
                        result = String.Format("{0:yy}/{0:MM}/{0:dd}", cdate);
                        break;
                    case "12":
                        result = String.Format("{0:yy}{0:MM}{0:dd}", cdate);
                        break;
                    case "101":
                        result = String.Format("{0:MM}/{0:dd}/{0:yyyy}", cdate);
                        break;
                    case "102":
                        result = String.Format("{0:yyyy}.{0:MM}.{0:dd}", cdate);
                        break;
                    case "103":
                        result = String.Format("{0:dd}/{0:MM}/{0:yyyy}", cdate);
                        break;
                    case "104":
                        result = String.Format("{0:dd}.{0:MM}.{0:yyyy}", cdate);
                        break;
                    case "105":
                        result = String.Format("{0:dd}-{0:MM}-{0:yyyy}", cdate);
                        break;
                    case "106":
                        result = String.Format("{0:dd} {0:MMM} {0:yyyy}", cdate);
                        break;
                    case "107":
                        result = String.Format("{0:MMM} {0:dd}, {0:yyyy}", cdate);
                        break;
                    case "110":
                        result = String.Format("{0:MM}-{0:dd}-{0:yyyy}", cdate);
                        break;
                    case "111":
                        result = String.Format("{0:yyyy}/{0:MM}/{0:dd}", cdate);
                        break;
                    case "112":
                        result = String.Format("{0:yyyy}{0:MM}{0:dd}", cdate);
                        break;
                    case "20":
                        result = String.Format("{0:yyyy}-{0:MM}-{0:dd}", cdate);
                        break;

                    default:
                        break;
                }
            }
            return result;
        }
       private String GetImageUrlPrefix(String colName)
       {
           switch (colName)
           {
               case Constants.ExportProduct.img30_url:
                   return Constants.ImageUrlPrefixesAndSuffixes.images30;
                   break;
               case Constants.ExportProduct.img60_url:
                   return Constants.ImageUrlPrefixesAndSuffixes.images60;
                   break;
               case Constants.ExportProduct.img90_url:
                   return Constants.ImageUrlPrefixesAndSuffixes.images90;
                   break;
               case Constants.ExportProduct.img120_url:
                   return Constants.ImageUrlPrefixesAndSuffixes.images120;
                   break;
               case Constants.ExportProduct.img180_url:
                   return Constants.ImageUrlPrefixesAndSuffixes.images180;
                   break;
               default:
                   return "";
           }
       }

        protected String GetDeepLink(String deepLinkField, Dictionary<String, String> SourceProductFields)
        {
            try
            {
                String deepLink = String.Empty;
                if (!String.IsNullOrEmpty(SourceProductFields[deepLinkField]) || !ObjExportAttributes.ProgSettings.DiURL)
                {
                    deepLink = ObjExportAttributes.PlateFormClickUrls[ObjExportAttributes.ProgSettings.PlatformID];
                    deepLink += Constants.Generic.ClickAspPrefix + ObjExportAttributes.PublisherId;
                    deepLink += "&site=" + ObjExportAttributes.ProgSettings.ProgramID;
                    deepLink += "&type=text";
                    deepLink += "&tnb=" + ObjExportAttributes.ProgSettings.TextLinkNb;
                    if (ObjExportAttributes.ProgSettings.DiURL)
                        deepLink += "&diurl=" + SourceProductFields[deepLinkField];
                    else
                        deepLink += "&pid=" + SourceProductFields[Constants.Product.ArtikelNumber].Replace(" ", "%20");

                }

                deepLink = deepLink.Replace("$ref$", ObjExportAttributes.PublisherId);//if advertiser needs affilinet publisher id
                return deepLink;
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("BaseConverter.GetDeepLink()", ex);
            }
        }
        
        protected String GetImageUrl(String imageId, Dictionary<String, String> SourceProductFields, String urlPrefix)
        {
            String imgURL = "";
            if (!String.IsNullOrEmpty(imageId) && imageId!="-1") //&& SourceProductFields[Constants.Product.ImageOK] == "True")
            {
                //String last3DigitsOfImageId = imageId.Substring(imageId.Length - 1 - Constants.Generic.ImageIdLastCharsToGenImageUrls);
                //imgURL = urlPrefix + last3DigitsOfImageId + "/" + imageId + "_" + ObjExportAttributes.ProgSettings.ImageNumToUse + ".jpg";
                String imageUrl = targetproductFields[Constants.ExportProduct.Img_Url];
                String md5Hash = Utilities.GetMd5HashBytes(imageUrl);
                String FolderName = md5Hash[md5Hash.Length - 1].ToString();
                imgURL = urlPrefix + CommonUtilities.PadProdProgId(ObjExportAttributes.ProgramFiles.ProdProgId) + "/" + FolderName + "/" + md5Hash + ".jpg";
            }
            else
                return urlPrefix + Constants.ImageUrlPrefixesAndSuffixes.Suffix404;

            return imgURL;
        }
        #endregion

        #region output field selection
        protected String PrepareCombinationOfFields(ExportColumn objExportColumn, Dictionary<String, String> SourceProductFields)
        {
                StringBuilder sbFieldValue = new StringBuilder();
                List<String> ComputedColumns = objExportColumn.ComputedColumns;
                for (int i = 0; i < ComputedColumns.Count() ; i++)
                {
                    bool isSpaceNeeded = i < ComputedColumns.Count() - 1 && !String.IsNullOrEmpty(SourceProductFields[ComputedColumns[i]]);
                    sbFieldValue.Append(SourceProductFields[ComputedColumns[i]] + (isSpaceNeeded ? " " : ""));
                }
                return sbFieldValue.ToString();
        }

        //Commented due to bug 13929, ImagexxUrl must not contain width & height along with as default culs with url, they are transmitted only if selected by publisher
        //protected bool IgnoreImageCol(String imgColName)
        //{
        //    switch (imgColName)
        //    {
        //        case Constants.ExportProduct.ImgWidth:
        //        case Constants.ExportProduct.ImgHeight:
        //            return !ObjExportAttributes.PubSettings.ImageWithSizeOnly;
        //        case Constants.ExportProduct.Img30Width:
        //        case Constants.ExportProduct.Img30Height:
        //            return !(ObjExportAttributes.PubSettings.Columns.Contains(
        //                     Constants.ExportProduct.img30_url.ToUpper()) &&
        //                     ObjExportAttributes.PubSettings.ImageWithSizeOnly);
        //        case Constants.ExportProduct.Img60Width:
        //        case Constants.ExportProduct.Img60Height:
        //            return !(ObjExportAttributes.PubSettings.Columns.Contains(
        //                     Constants.ExportProduct.img60_url.ToUpper()) &&
        //                     ObjExportAttributes.PubSettings.ImageWithSizeOnly);
        //        case Constants.ExportProduct.Img90Width:
        //        case Constants.ExportProduct.Img90Height:
        //            return !(ObjExportAttributes.PubSettings.Columns.Contains(
        //                     Constants.ExportProduct.img90_url.ToUpper()) &&
        //                     ObjExportAttributes.PubSettings.ImageWithSizeOnly);
        //        case Constants.ExportProduct.Img120Width:
        //        case Constants.ExportProduct.Img120Height:
        //            return !(ObjExportAttributes.PubSettings.Columns.Contains(
        //                     Constants.ExportProduct.img120_url.ToUpper()) &&
        //                     ObjExportAttributes.PubSettings.ImageWithSizeOnly);
        //        case Constants.ExportProduct.Img180Width:
        //        case Constants.ExportProduct.Img180Height:
        //            return !(ObjExportAttributes.PubSettings.Columns.Contains(
        //                     Constants.ExportProduct.img180_url.ToUpper()) &&
        //                     ObjExportAttributes.PubSettings.ImageWithSizeOnly);
        //        default:
        //            return true;
        //    }
        //}

        protected bool IgnoreCol(ExportColumn objExportColumn)
        {
            try
            {
                //if (!IgnoreImageCol(objExportColumn.ExportCsvColumn)) return false;        //Commented due to bug 13929, ImagexxUrl must not contain width & height along with as default culs with url, they are transmitted only if selected by publisher
                if (ObjExportAttributes.PubSettings.Columns.Count > 0)//If publisher has selected fields
                {
                    if ( //objExportColumn.IsDefault || //Cols wich are mendatory to transmit//2011-01-05 commented as default cols are transmitted only if pub has selected no settings
                        ObjExportAttributes.PubSettings.Columns.Contains(objExportColumn.ExportCsvColumn.ToUpper()) ||//Cols selected by publisher
                        ColumnsDefaultWithXXX.Contains(objExportColumn.ExportCsvColumn))//e.g: cols default with price field
                        return false;
                }
                else
                {//If publisher has NOT selected fields
                    if (objExportColumn.IsDefault)
                        return false;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return true;
        }

        #endregion


        #region Source CSV
        protected Dictionary<String, String> GetSourceCsvFields(String strLine)
        {
            try
            {
                Dictionary<String, String> productFields = new Dictionary<String, String>();
                String[] splitChrs = new String[]
                                                 {
                                                     Constants.Generic.DestFieldQualifier +
                                                     Constants.Generic.DestFieldSeperator +
                                                     Constants.Generic.DestFieldQualifier
                                                 }; // "~\";~\"" 
                String[] fields = strLine.Split(splitChrs, StringSplitOptions.None);
             
                List<String> fieldsList = fields.ToList();

                //Start: MD5 key often contains newline chars; so must read fields from next lines
                while (fieldsList.Count < SourceCsvHeader.Count)
                {
                    String strLineTmp = "";
                    while ((!SrCsvSource.EndOfStream) && (strLineTmp = SrCsvSource.ReadLine()) == ""){}//looping through if there are multiple line breaks in MD5 hash
                    if(String.IsNullOrEmpty(strLineTmp))
                        throw new AffiliGenericException("Source file contains less no. of columns then expected");
                    String[] fieldsTmp = strLineTmp.Split(splitChrs, StringSplitOptions.None);
                    fieldsList[fieldsList.Count - 1] += fieldsTmp;
                    for (int i = 1; i < fieldsTmp.Length; i++)
                    {
                        fieldsList.Add(fieldsTmp[i]);
                    }
                }
                //End: MD5 key often contains newline chars; so must read fields from next lines

                if (fieldsList.Count > 0)
                {
                    fieldsList[0] = fieldsList[0].Replace(Constants.Generic.DestFieldQualifier, "");
                    fieldsList[fieldsList.Count - 1] = fieldsList[fieldsList.Count - 1].Replace(Constants.Generic.DestFieldQualifier, "");
                }

                for (int i = 0; i < fieldsList.Count; i++)
                    productFields.Add(SourceCsvHeader[i], fieldsList[i]);

                return productFields;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
            protected void CreateSourceCsvColumns(String strLine)
        {
            //strLine = strLine.Replace("\"", "");
            SourceCsvHeader = new List<string>();
            String[] fields = strLine.Split(Constants.Generic.DestFieldSeperator[0]);
            foreach (String fld in fields)
            {
                SourceCsvHeader.Add(fld);
            }
        }
        #endregion

        protected void WriteToOutput(String text)
        {
            try
            {
                if (IsZipped)
                    ZippedTextWriter.Write(text);
                else
                    ObjExportAttributes.HttpContext.Response.Write(text);
            }catch(Exception ex)
            {
                throw new AffiliGenericException("BaseConverter.WriteToOutput() IsZipped:" + IsZipped.ToString(), ex);
            }
        }

        /// <summary>
        /// Opens the file for read.
        /// </summary>
        protected void OpenFileForRead(String fileName)
        {
            try
            {
                SrCsvSource = new StreamReader(fileName);
            }
            catch (Exception ex)
            {
                throw new AffiliGenericException("BaseConverter.OpenFileForRead() Error while opening source file:" + fileName,ex);
            }
        }
        protected String GetExceptionInfo()
        {
            return " Publisher:" + ObjExportAttributes.PublisherId + " Program:" + ObjExportAttributes.ProgramFiles.ProdProgId;
        }
        
    }
}
