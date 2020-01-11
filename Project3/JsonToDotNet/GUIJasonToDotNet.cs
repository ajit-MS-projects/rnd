using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using AffiliJsonAccessAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonToDotNet
{
    public partial class GUIJasonToDotNet : Form
    {
        public GUIJasonToDotNet()
        {
            InitializeComponent();
        }

        private void btnReadJason_Click(object sender, EventArgs e)
        {
            WebClient client = new WebClient();
            client.Headers.Add("User-Agent", "Nobody");
            String response = client.DownloadString(new Uri("http://www.hanselman.com/smallestdotnet/json.ashx"));

            txtRawJason.Text = response;
            JObject o = JObject.Parse(response);
            txtJsonObject.Text = o.ToString();
        }

        private void btnReadJsonToEnt_Click(object sender, EventArgs e)
        {
            WebClient client = new WebClient();
            client.Headers.Add("User-Agent", "Nobody"); //my endpoint needs this...
            String response = client.DownloadString(new Uri("file://d:/temp.txt"));//" http://www.hanselman.com/smallestdotnet/json.ashx"));

            txtRawJason.Text = response;
            SmallestDotNetThing j = JsonConvert.DeserializeObject<SmallestDotNetThing>(response);
            txtJsonObject.Text = j.downloadableVersions[0].url;
            //http://172.25.10.13:8080/solr/prodDEFULL/select/?q=macbook+pro&version=2.2&start=0&rows=10&indent=on
        }

        public class SmallestDotNetThing
        {
            public DotNetVersion latestVersion { get; set; }
            public List<DotNetVersion> allVersions { get; set; }
            public List<DotNetVersion> downloadableVersions { get; set; }
        }
        public class DotNetVersion
        {
            public int major { get; set; }
            public int minor { get; set; }
            public string profile { get; set; }
            public int? servicePack { get; set; }
            public string url { get; set; }
        }

        private void btnReadUsingAffiliApi_Click(object sender, EventArgs e)
        {
            AffiliGenericJson<SmallestDotNetThing>  objJsonAccess = new AffiliGenericJson<SmallestDotNetThing>();
            SmallestDotNetThing j = objJsonAccess.GetObjectFromJsonStream("http://www.hanselman.com/smallestdotnet/json.ashx","aa");
            txtJsonObject.Text = j.downloadableVersions[0].url;

            objJsonAccess.WriteObjectToJsonFileStream(j,"d:/temp1.txt");
        }
    }
}