using System;
using System.Configuration;
using System.IO;
using System.Net;

namespace AffiliProductExportWeb
{
    public partial class Monitoring : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            String strShopIds = ConfigurationManager.AppSettings["ShopIds"];
            String[] arrShops = strShopIds.Split(',');
            int j = 0;
            for (int i = 0; i < arrShops.Length; i++)
            {
                StreamReader SrCsvSource = null;
                string downloadUrl = String.Format(ConfigurationManager.AppSettings["DownloadURL"], arrShops[i]);
                try
                {
                    WebRequest request = HttpWebRequest.Create(downloadUrl);

                    WebResponse response = request.GetResponse();

                    SrCsvSource = new StreamReader(response.GetResponseStream());
                    string strLine = SrCsvSource.ReadLine();
                    if (strLine.IndexOf("ArtNumber") > -1)
                    {
                        Response.Write("OKAY");
                        break;
                    }
                    else
                    {
                        j++;
                        continue;
                    }
                }
                catch (Exception exp)
                {
                    Response.Write("ERROR");
                    break;
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
            if (j > 0)
            {
                Response.Write("ERROR");
            }
        }
    }
}