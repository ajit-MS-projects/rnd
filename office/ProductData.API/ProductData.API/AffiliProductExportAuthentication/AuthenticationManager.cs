using System;
using System.Web;
using Affilinet.Tools.Krypto;
using Affilinet.Exceptions;
using Affilinet.ProductExport.Authentication.Common;
using Affilinet.ProductExport.Authentication.DAO;

namespace Affilinet.ProductExport.Authentication
{
    public class AuthenticationManager
    {
        private AuthenticationDao authDao { get; set; }
        public AuthenticationManager()
        {
           authDao = new AuthenticationDao();
        }
   
        public bool IsAuthenticated(HttpContext context, out bool isAutoDownloadParam)
        {
            bool authenticated = false;
            isAutoDownloadParam = false;

            try
            {
                if (!context.Request.IsLocal)
                {
                    int publisherID;
                    int shopID;
                    SetGlobalParameters(context, out publisherID, out shopID);

                    isAutoDownloadParam = isAutoDownload(context);

                    if (isAutoDownloadParam)
                    {
                        //context.Response.Write("AUTO DOWNLOAD" + "<br/>");

                        authenticated = authDao.IsAuthorizedCSVPassword(publisherID, shopID, context.Request[Constants.Generic.PASSWORD_PARAMETER]);
                        if (!authenticated)
                        {
                            authenticated = handleManualDownload(context, publisherID, shopID);
                            if (authenticated)
                            {
                                isAutoDownloadParam = false;
                            }
                        }
                    }
                    else
                    {
                        authenticated = handleManualDownload(context, publisherID, shopID);
                    }


                    //Logger.Write(
                    //    "Authenticated: " + authenticated.ToString().ToUpper() + "! Publisher: " + publisherID +
                    //    " requests productlist for ShopID: " + shopID + "! Autodownload? " + isAutoDownloadParam + "!",
                    //    "UserDownload");
                }
                else
                {
                    isAutoDownloadParam = true;
                    authenticated = true;
                }
            }
            catch (AffiliGenericException ex)
            {
                new AffiliGenericException(ex.Message + context.Request.Url, ex.InnerException).CreateLog();
            }
            catch (Exception ex)
            {
                new AffiliGenericException("AuthenticationManager.IsAuthenticated() " + context.Request.Url, ex).CreateLog();
            }

            return authenticated;

        }

        private  bool handleManualDownload(HttpContext context, int publisherID, int shopID)
        {
            //Write("handleManualDownload?");
            bool authenticated = false;
            //Write("Has Context? -> " + (context != null));

            bool var_isAuthenticatedPublisher = isAuthenticatedPublisher(publisherID, context);
            //Write("Authenticated Publisher? -> " + var_isAuthenticatedPublisher);
            if (var_isAuthenticatedPublisher)
            {
                //context.Response.Write("IS AUTHENTICATED PUBLISHER" + "<br/>");
                authenticated = authDao.isAuthorizedForShopID(publisherID, shopID);
                //context.Response.Write("IS AUTHORIZED: " + authenticated + "<br/>");
            }

            //Write("handleManualDownload: " + authenticated);

            return authenticated;
        }

        private  bool isAuthenticatedPublisher(int publisherID, HttpContext context)
        {
            //Write("isAuthenticatedPublisher?");

            bool authenticated = false;
            if (isValidManualDownload(context))
            {
                Daedalos krypto = new Daedalos();
                string passwordParam = "";

                string passPara = context.Request[Constants.Generic.PASSWORD_PARAMETER];
                string hostAddress = context.Request.UserHostAddress;

                //  Write("PassParameter - " + passPara);
                // Write("HostAdd -> " + hostAddress);

                //passPara = context.Server.UrlDecode(passPara);
                //HttpContext.Current.Trace.Write("Decoded Passwort Parameter: " + passPara);

                try
                {
                    passwordParam = krypto.Decrypt(passPara, hostAddress);
                    //   Write("Password -> " + passwordParam);
                }
                catch (Exception ex)
                {
                    // Write(ex.Message);                    
                    throw new AffiliGenericException("AuthenticationManager.isAuthenticatedPublisher(). Error on krypto.Decrypt.", ex); 
                }
                passwordParam = passwordParam.Replace("\0", "");

                int verifiedID = 0;

                try
                {                    
                    verifiedID = authDao.VerifyCredentials(publisherID, passwordParam, false);
                }
                catch (Exception ex)
                {
                    throw new AffiliGenericException("AuthenticationManager.isAuthenticatedPublisher(). Error on VerifyCredentials.", ex); 
                }

                authenticated = (verifiedID == publisherID);
            }

            return authenticated;
        }
                
        private  bool isValidManualDownload(HttpContext context)
        {
            return CheckReferrerAndPassword(context, Constants.Generic.MANUAL_DOWNLOAD_REFERRER);
        }

        private  bool CheckReferrerAndPassword(HttpContext context, string checkReferrer)
        {
            bool result = false;

            bool hasRef = context.Request.UrlReferrer != null;

            //Write("Enter Method: CheckReferrerAndPassword()");
            if (hasRef/* || IsAffilinetIP(context)*/)
            {
                //Write("Has Referrer: " + context.Request.UrlReferrer);
                int length = context.Request.UrlReferrer.Segments.Length;
                string value = context.Request.UrlReferrer.Segments[length - 1];


                if (value.ToLower().Equals(checkReferrer.ToLower()))
                {
                    result = context.Request[Constants.Generic.PASSWORD_PARAMETER] != null;
                    //  Write("Has Password Parameter!");
                }
            }
            //Write("Leave method with result: " + result);
            return result;
        }

        private  void SetGlobalParameters(HttpContext context, out int publisherID, out int shopID)
        {
            int length = context.Request.Url.Segments.Length;
            string paramString = context.Request.Url.Segments[length - 1];
            paramString = paramString.Substring(0, paramString.IndexOf("."));
            string[] param = paramString.Split('_');

            publisherID = Convert.ToInt32(param[3]);
            shopID = Convert.ToInt32(param[2]);
        }

        private  bool isAutoDownload(HttpContext context)
        {
            bool isNotManualDownloadReferrer = false;

            if (context.Request.UrlReferrer != null)
            {
                try
                {
                    int length = context.Request.UrlReferrer.Segments.Length;
                    string value = context.Request.UrlReferrer.Segments[length - 1];

                    if (!value.ToLower().Equals(Constants.Generic.MANUAL_DOWNLOAD_REFERRER.ToLower()))
                    {
                        isNotManualDownloadReferrer = true;
                    }
                }                                
                catch (Exception ex)
                {
                    throw new AffiliGenericException("AuthenticationManager.isAutoDownload(). Error on get the context.", ex); 
                }
            }

            bool hasRef = (context.Request.UrlReferrer == null || isNotManualDownloadReferrer);

            return hasRef;
        }
    }
}
