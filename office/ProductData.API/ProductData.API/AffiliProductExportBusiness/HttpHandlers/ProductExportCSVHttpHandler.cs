using System;
using System.Collections.Generic;
using System.Linq;
using Affilinet.Business.ProductExport.Common;
using System.Web;
using Affilinet.Exceptions;

namespace Affilinet.Business.ProductExport.HttpHandlers
{
    public class ProductExportCSVHttpHandler : ProductExportRequestHandler
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
                String outputFile = Utilities.GetOutputFilePath(context, Constants.ExportFileExtensions.CSV);

                SetupContectType(context, outputFile);
                base.SetRequestParametersFromFilePath(outputFile);

                ExportRequestManager objExportRequestManager = new ExportRequestManager();
                objExportRequestManager.StartExport(ShopID, publisherID, ExportfileTypesEnum.CSV, false, context, outputFile + Constants.ExportFileExtensions.CSV);

                //context.Response.End();
            }
            catch (Exception ex)
            {
                new AffiliGenericException("ProductExportXmlHttpHandler.GenerateRequest()", ex).CreateLog();
                throw;
            }
        }
        private void SetupContectType(HttpContext context, String outputFile)
        {
            if (!IsAutoDownload)
            {
                context.Response.ContentType = Constants.OutputHTTPHeaders.ContentTypeCsv;
                context.Response.AppendHeader(Constants.OutputHTTPHeaders.HeaderContentDisposition,
                                              String.Format(Constants.OutputHTTPHeaders.HeaderContentDispositionFileAttach,
                                              outputFile + Constants.ExportFileExtensions.CSV));//affilinet_products_1048_651.xml
            }
        }
    }
}
