using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Affilinet.Exceptions;
using Affilinet.ProductExport.Authentication;
using Affilinet.Business.ProductExport.Common;

namespace Affilinet.Business.ProductExport.HttpHandlers
{
    public abstract class ProductExportRequestHandler : IHttpHandler
    {
        #region IHttpHandler Members
        private bool isAutoDownload;
        public bool IsReusable
        {
            get { return true; }
        }
        protected String publisherID { get; set; }
        protected String ShopID { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance is auto download.
        /// Auto download true means xml/csv is returned as stream else as a downloadable file
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is auto download; otherwise, <c>false</c>.
        /// </value>
        public bool IsAutoDownload
        {
            get { return isAutoDownload; }
        }
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                context.Response.Buffer = false;
                context.Response.ClearHeaders();
                context.Response.Clear();
                AuthenticationManager objAuthenticationManager = new AuthenticationManager();
                // Authenticate the user(request)
                if (objAuthenticationManager.IsAuthenticated(context, out isAutoDownload))
                {
                    Utilities.CreateInfoLog("ProductExportRequestHandler.ProcessRequest() Authenticated! - Start UserDownload", ApplicationEventsEnum.StartUserDownload);
                    GenerateRequest(context);
                }
                else
                {
                    Utilities.CreateInfoLog("ProductExportRequestHandler.ProcessRequest() Not authenticated! - RawUrl: " + context.Request.RawUrl, ApplicationEventsEnum.StartUserDownload);
                    context.Response.Write("Credentials could not be verified!");
                    context.Response.End();
                    //context.Response.WriteFile(context.Server.MapPath("~/Error/Error.htm"));
                }
            }
            catch (AffiliGenericException ex)
            {
                ex.CreateLog();
                //context.Response.WriteFile(context.Server.MapPath("~/Error/Error.htm"));
            }
            catch (Exception ex)
            {
                new AffiliGenericException("ProcessRequest.ProcessRequest()", ex).CreateLog();
                //context.Response.WriteFile(context.Server.MapPath("~/Error/Error.htm"));
            }

        }
        protected void SetRequestParametersFromFilePath(String outputFile)
        {
            try
            {
                String[] param = outputFile.Split('_');
                publisherID = param[3];
                ShopID = param[2];
            }catch(Exception ex)
            {
                throw new AffiliGenericException("Invalid publisher and shopid",ex);
            }
        }

        #endregion

        protected abstract void GenerateRequest(HttpContext context);
    }
}
