using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using WcfWinService.MetaDataServices;

namespace WcfWinServiceClient
{
    public partial class _Default : System.Web.UI.Page
    {
        private IMetaDataService metaDataServiceChannel = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            EndpointAddress productAddress = new EndpointAddress("net.tcp://localhost:9010/ProductService");
            metaDataServiceChannel = ChannelFactory<IMetaDataService>.CreateChannel(new NetTcpBinding(), productAddress);
            //EndpointAddress productAddress = new EndpointAddress("http://localhost:9010/ProductService");
            //metaDataServiceChannel = ChannelFactory<IMetaDataService>.CreateChannel(new WSHttpBinding(), productAddress);
            ServiceMetadata objServiceMetadata = metaDataServiceChannel.WhatAreYouDoing();

            Response.Write(objServiceMetadata.HealthCheckCounter);
            Response.Write("<br>");
            Response.Write(objServiceMetadata.LastStamp);
            Response.Write("<br>");
            Response.Write(objServiceMetadata.LastMetaDataServiceFault);
        }
    }
}
