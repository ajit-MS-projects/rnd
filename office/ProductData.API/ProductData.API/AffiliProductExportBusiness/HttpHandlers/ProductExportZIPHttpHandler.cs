using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Affilinet.Business.ProductExport.Common;
using Affilinet.Exceptions;

namespace Affilinet.Business.ProductExport.HttpHandlers
{
    public class ProductExportZIPHttpHandler : ProductExportRequestHandler
    {
        /// <summary>
        /// http://publisher.affili.net/Download/AutoDownload.aspx?PartnerID=651&csvPW=cc6j$lwq(sZInWqOlA[&listID=376;
        /// http://publisher.affili.net/Tools/Download.aspx?prodprogid=210
        /// 
        /// http://productdata.download.affili.net/affilinet_products_1048_651.xml?auth=kQEGJ65eDOiLLSjtxBY88l68
        /// http://productdata.download.affili.net/affilinet_products_1049_651.csv?auth=wAGJtn1xyfCekbUvzIzA6w8b
        /// http://productdata.download.affili.net/affilinet_products_1048_651.gz?auth=wAFRGErK%2b9Ubz2G89DwFeIVv
        /// </summary>
        /// <param name="context"></param>
        protected override void GenerateRequest(HttpContext context)
        {
            try
            {
                String outputFile = Utilities.GetOutputFilePath(context, String.Empty);
                outputFile = Utilities.RemoveZipExtensions(outputFile);

                SetupContectType(context, outputFile);
                base.SetRequestParametersFromFilePath(outputFile);

                ExportRequestManager objExportRequestManager = new ExportRequestManager();

                if (context.Request["type"] != null && context.Request["type"].ToUpper() == "XML")
                {
                    objExportRequestManager.StartExport(ShopID, publisherID, ExportfileTypesEnum.XMLZip, false, context, outputFile + Constants.ExportFileExtensions.XML);
                }
                else
                {
                    objExportRequestManager.StartExport(ShopID, publisherID, ExportfileTypesEnum.CSVZip, false, context, outputFile + Constants.ExportFileExtensions.CSV);
                }
                //context.Response.End();
            }
            catch (Exception ex)
            {
                new AffiliGenericException("ProductExportZIPHttpHandler.GenerateRequest()", ex).CreateLog();
                throw;
            }
        }
        private void SetupContectType(HttpContext context, String outputFile)
        {
            //if (!IsAutoDownload)
            {
                context.Response.ContentType = Constants.OutputHTTPHeaders.ContentTypeGZip;
                context.Response.AppendHeader(Constants.OutputHTTPHeaders.HeaderContentDisposition, 
                                                String.Format(Constants.OutputHTTPHeaders.HeaderContentDispositionFileAttach, 
                                                outputFile + Constants.ExportFileExtensions.GZip));//affilinet_products_1048_651.gz
            } 
        }
    }
}
