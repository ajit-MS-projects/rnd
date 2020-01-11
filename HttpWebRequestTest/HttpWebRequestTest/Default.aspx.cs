using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Net;
using System.IO;

namespace HttpWebRequestTest
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String url = "http://195.189.237.104:8080/solr/DE4/select?";
            String param="version=2.2&indent=on&shards=195.189.237.101:8080/solr/DE1,195.189.237.102:8080/solr/DE2,195.189.237.103:8080/solr/DE3,195.189.237.104:8080/solr/DE4,195.189.237.105:8080/solr/DE5,195.189.237.106:8080/solr/DE6,195.189.237.107:8080/solr/DE7,195.189.237.108:8080/solr/DE8,195.189.237.109:8080/solr/DE9,195.189.237.110:8080/solr/DE10,195.189.237.116:8080/solr/DE11,195.189.237.117:8080/solr/DE12,195.189.237.118:8080/solr/DE13,195.189.237.119:8080/solr/DE14,195.189.237.120:8080/solr/DE15,195.189.237.121:8080/solr/DE16,195.189.237.122:8080/solr/DE17,195.189.237.123:8080/solr/DE18,195.189.237.124:8080/solr/DE19,195.189.237.125:8080/solr/DE20&publisherId=651&start=0&rows=0&facet=true&facet.sort=count&facet.mincount=1&facet.limit=20000&facet.field=AffilinetCategoryPathFacet&shopIdFilterPos=698";
            //CustomWebClient c = new CustomWebClient();
            //c.DownloadString("http://www.google.com");
            Response.Write(GetXmlDocumentFromPost(url,param));
        }
        public static byte[] GetContentToPost(string content, Encoding oEncoding)
        {
            byte[] byteXml = oEncoding.GetBytes(content);
            return byteXml;
        }
        public static String GetXmlDocumentFromPost(string url, string queryparameters)
        {
            //XmlDocument xdoc = new XmlDocument();
            HttpWebRequest oRequest = (HttpWebRequest)WebRequest.Create(url);
            byte[] postBytes = GetContentToPost(queryparameters, Encoding.UTF8);
            oRequest.Method = "POST";
            oRequest.ContentType = "application/x-www-form-urlencoded";
            oRequest.ContentLength = postBytes.Length;
            Stream dataStream = null;
            try
            {
                dataStream = oRequest.GetRequestStream();
            }
            catch (Exception ex)
            {
                string errmessage = "Error in SolrSearchers-GetXml-GetRequestStream" + Environment.NewLine;
                errmessage += "Url: " + url + Environment.NewLine;
                errmessage += "QueryParameters: " + queryparameters + Environment.NewLine;
                throw new Exception(errmessage, ex);
            }
            finally
            {
                if (dataStream != null)
                {
                    dataStream.Write(postBytes, 0, postBytes.Length);
                    dataStream.Close();
                }
            }
            HttpWebResponse oResponse = null;
            try
            {
                oResponse = (HttpWebResponse)oRequest.GetResponse();
            }
            catch (WebException e)
            {
                HttpStatusCode intCode = 0;
                if (oResponse != null)
                    intCode = oResponse.StatusCode;

                Stream errstream = e.Response.GetResponseStream();
                string errsr = "";
                if (errstream != null)
                {
                    StreamReader errreader = new StreamReader(errstream);
                    errsr = errreader.ReadToEnd();
                }
                string errmessage = "Error in SolrSearchers-GetXml-GetResponseStream" + Environment.NewLine;
                if (intCode != 0)
                    errmessage += "HttpStatusCode: " + intCode.ToString() + Environment.NewLine;
                if (errsr != "")
                    errmessage += "StreamResponse: " + errsr + Environment.NewLine + "End-of-stream-response" + Environment.NewLine;
                throw new Exception(errmessage, e);
            }
            StreamReader reader = new StreamReader(oResponse.GetResponseStream());
            return reader.ReadToEnd();
            
        }
    }
    class CustomWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest webRequest = (HttpWebRequest)base.GetWebRequest(address);
            webRequest.ServicePoint.ConnectionLimit = 450;
            webRequest.KeepAlive = false;
            return webRequest;
        }
    }
}
