using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HtmlAgilityPack;
using System.Reflection;

namespace ReadMetaTags
{
	public partial class WebForm1 : System.Web.UI.Page
	{
		//http://htmlagilitypack.codeplex.com/wikipage?title=Examples
		protected void Page_Load(object sender, EventArgs e)
		{
			MetaInformation meta =
				GetMetaInformation(
					"http://www.amazon.de/gp/product/B003DTLUYK/ref=s9_cartx_gw_d99_g263_ir02?pf_rd_m=A3JWKAKR8XB7XF&pf_rd_s=center-7&pf_rd_r=1BY33XWB7JV104NVF1J2&pf_rd_t=101&pf_rd_p=463772813&pf_rd_i=301128");
			//foreach (PropertyInfo prop in typeof(MetaInformation).GetProperties())
			//{
			//    Response.Write(prop.Name + ": " + prop.GetValue(meta, null) + "<br>");
			//}
			Response.Write("<br><br>");

			meta = GetMetaInformation("http://www.lucky-bike.de/.cms/Bulls_Wild_One_2011/152-1-3525");

			Response.Write("<br><br>");

			meta = GetMetaInformation("http://www.redcoon.de/B268404-Scott-XSE-60-Phuket_Anlage-mit-DVD");
			//foreach (PropertyInfo prop in typeof(MetaInformation).GetProperties())
			//{
			//    Response.Write(prop.Name + ": " + prop.GetValue(meta, null) + "<br>");
			//}

			Response.Write("<br><br>");

			meta = GetMetaInformation("http://www.sparacuda.de");
			//foreach (PropertyInfo prop in typeof(MetaInformation).GetProperties())
			//{
			//    Response.Write(prop.Name + ": " + prop.GetValue(meta, null) + "<br>");
			//}

			Response.Write("<br><br>");
			meta = GetMetaInformation("http://www.groupon.de/deals/online-deal/Allyouneed/7659955");
			//foreach (PropertyInfo prop in typeof(MetaInformation).GetProperties())
			//{
			//    Response.Write(prop.Name + ": " + prop.GetValue(meta, null) + "<br>");
			//}

			Response.Write("<br><br>");
			meta = GetMetaInformation("http://dailydeal.de/gutscheine/kassel/gutschein-shopping-ballon4you-260612");
			//foreach (PropertyInfo prop in typeof(MetaInformation).GetProperties())
			//{
			//    Response.Write(prop.Name + ": " + prop.GetValue(meta, null) + "<br>");
			//}
		}

		private const string UserAgent = "Sparacudabot/1.0 (+http://www.sparacuda.de/ueber-sparacuda)";


		protected MetaInformation GetMetaInformation(string url)
		{
			MetaInformation meta = new MetaInformation();

			TextReader textReader = new StringReader(GetHtml(url));
			HtmlDocument doc = new HtmlDocument();
			doc.Load(textReader);

			GetLinkTagInformation(meta, doc);

			GetMetaTagInformation(meta, doc);

			GetOpenGraphMataTagInformation(meta, doc);

			if (string.IsNullOrWhiteSpace(meta.OgImage))
			{
				GetImageFromImageTags(meta, doc);
			}
			if (string.IsNullOrWhiteSpace(meta.OgImage))
			{
				DownloadAndDetectImageFromImageTags(meta, doc);
			}
			Response.Write("<img src='" + meta.OgImage + "'/>");

			return meta;
		}

		private void DownloadAndDetectImageFromImageTags(MetaInformation meta, HtmlDocument doc)
		{
			if (doc.DocumentNode.SelectNodes("//img[@src]") != null)
			{
				int i = 0;
				int largestWidth = 0;
				LimitedQueue<string> imageQueue = new LimitedQueue<string>(3);

				foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//img[@src]"))
				{
					i++;
					if (node.Attributes["src"] != null)
					{
						int tmpWidth = 0;
						int tmpHeight = 0;

						System.Drawing.Image objImage = DownloadImage(node.Attributes["src"].Value);
						if (objImage != null)
						{
							if (IsTransparentPalette(objImage.Palette)) continue;
							tmpWidth = objImage.Width;
							tmpHeight = objImage.Height;
							objImage.Dispose();
						}
						if ((largestWidth == 0 || tmpWidth > largestWidth) && (tmpHeight > 0 && tmpWidth/tmpHeight <= 2))
						{
							largestWidth = tmpWidth;
							imageQueue.Enqueue(node.Attributes["src"].Value);
						}
					}
				}

				meta.DetectedImages = imageQueue.ToList();

				foreach (var v in meta.DetectedImages)
				{
					Response.Write("<img src='" + v + "'/><br/>");
				}
				Response.Write("<br/><br/><br/><br/>");

			}
		}

		private void GetImageFromImageTags(MetaInformation meta, HtmlDocument doc)
		{
			if (doc.DocumentNode.SelectNodes("//img[@src]") != null)
			{
				int i = 0;
				int largestWidth = 0;
				LimitedQueue<string> imageQueue = new LimitedQueue<string>(3);

				foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//img[@src]"))
				{
					i++;
					if (node.Attributes["src"] != null)
					{
						int tmpWidth = 0;
						int tmpHeight = 0;

						if (node.Attributes["width"] != null && int.TryParse(node.Attributes["width"].Value, out tmpWidth) &&
						    tmpWidth > 20 &&
						    node.Attributes["height"] != null && int.TryParse(node.Attributes["height"].Value, out tmpHeight))
						{

							if ((largestWidth == 0 || tmpWidth > largestWidth) && (tmpHeight > 0 && tmpWidth/tmpHeight <= 2))
							{
								largestWidth = tmpWidth;
								meta.OgImage = node.Attributes["src"].Value;
							}
						}
					}
				}
				if (!String.IsNullOrWhiteSpace(meta.OgImage))
				{
					System.Drawing.Image objImage = DownloadImage(meta.OgImage);
					if (objImage != null)
					{
						if (IsTransparentPalette(objImage.Palette)) meta.OgImage = null;
						objImage.Dispose();
					}
				}
			}
		}


		private static void GetLinkTagInformation(MetaInformation meta, HtmlDocument doc)
		{
			if (doc.DocumentNode.SelectNodes("//link[@rel]") != null)
			{
				foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//link[@rel]"))
				{
					if (node.Attributes["rel"] != null && node.Attributes["href"] != null)
					{
						switch (node.Attributes["rel"].Value.ToLower())
						{
							case "image_src":
								meta.OgImage = node.Attributes["href"].Value;
								break;
						}
					}
				}
			}
		}

		public bool IsTransparentPalette(System.Drawing.Imaging.ColorPalette palette)
		{
			if (palette.Flags != 1)
				return false;

			int total_colors = palette.Entries.GetLength(0);
			for (int i = 0; i < total_colors - 1; i++)
			{
				if (palette.Entries[i].A != 0)
				{
					return false;
				}
			}
			return true;
		}

		private static void GetOpenGraphMataTagInformation(MetaInformation meta, HtmlDocument doc)
		{
			if (doc.DocumentNode.SelectNodes("//meta[@property]") != null)
			{
				foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//meta[@property]"))
				{
					if (node.Attributes["property"] != null && node.Attributes["content"] != null)
					{
						switch (node.Attributes["property"].Value.ToLower())
						{
							case "og:title":
								meta.OgTitle = node.Attributes["content"].Value;
								break;
							case "og:type":
								meta.OgType = node.Attributes["content"].Value;
								break;
							case "og:image":
								meta.OgImage = node.Attributes["content"].Value;
								break;
							case "og:url":
								meta.OgUrl = node.Attributes["content"].Value;
								break;
							case "og:description":
								meta.OgDescription = node.Attributes["content"].Value;
								break;
							case "og:site_name":
								meta.OgSiteName = node.Attributes["content"].Value;
								break;
						}
					}
				}
			}
		}

		private static void GetMetaTagInformation(MetaInformation meta, HtmlDocument doc)
		{
			if (doc.DocumentNode.SelectNodes("//meta[@name]") != null)
			{
				foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//meta[@name]"))
				{
					switch (node.Attributes["name"].Value.ToLower())
					{
						case "geo.region":
							meta.GeoRegion = node.Attributes["content"].Value;
							break;
						case "geo.placename":
							meta.GeoPlaceName = node.Attributes["content"].Value;
							break;
						case "geo.position":
							meta.GeoPosition = node.Attributes["content"].Value;
							break;
						case "icbm":
							meta.Icbm = node.Attributes["content"].Value;
							break;
						case "title":
							meta.OgTitle = node.Attributes["content"].Value;
							break;
						case "keywords":
							meta.Keywords = node.Attributes["content"].Value;
							break;
						case "description":
							meta.OgDescription = node.Attributes["content"].Value;
							break;
					}
				}
			}
		}


		private string GetHtml(string url)
		{
			string returnHtml = string.Empty;
			try
			{
				if (IsValidUrl(url))
				{
					HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
					request.UserAgent = UserAgent;
					request.Accept = "GET HTTP/1.1";
					request.Timeout = 4000;
					request.Credentials = CredentialCache.DefaultCredentials;
					HttpWebResponse response = (HttpWebResponse) request.GetResponse();

					using (Stream dataStream = response.GetResponseStream())
					{
						if (dataStream != null)
							using (StreamReader reader = new StreamReader(dataStream))
							{
								returnHtml = reader.ReadToEnd();
							}
					}
					response.Close();
				}
			}
			catch (Exception)
			{
				//todo: log
				throw;
			}

			return returnHtml;
		}

		private bool IsValidUrl(string url)
		{
			try
			{
				WebRequest request = WebRequest.Create(url);
				request.Timeout = 2000;
				request.Method = "HEAD";
				request.Credentials = CredentialCache.DefaultCredentials;
				HttpWebResponse response = (HttpWebResponse) request.GetResponse();
				response.Close();
			}
			catch (WebException ex)
			{
				HttpStatusCode returnedStatus = ((HttpWebResponse) ex.Response).StatusCode;
				if (returnedStatus != HttpStatusCode.MethodNotAllowed && returnedStatus != HttpStatusCode.Forbidden &&
				    returnedStatus != HttpStatusCode.NotAcceptable) //Head is not allowed so try Get any way
					return false;
			}

			return true;
		}

		/// <summary>
		/// Function that downloads images from a web urls
		/// </summary>
		/// <param name="url">URL address to download image</param>
		/// <returns>Image</returns>
		public System.Drawing.Image DownloadImage(string url)
		{
			System.Drawing.Image tmpImage = null;

			try
			{
				HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);

				request.AllowWriteStreamBuffering = true;

				request.UserAgent = UserAgent;
				request.Accept = "GET HTTP/1.1";

				request.Timeout = 2000;

				System.Net.WebResponse webResponse = request.GetResponse();

				System.IO.Stream webStream = webResponse.GetResponseStream();

				if (webStream != null) tmpImage = System.Drawing.Image.FromStream(webStream);

				webResponse.Close();
				webResponse.Close();
			}
			catch (Exception exception)
			{
				return null;
			}

			return tmpImage;
		}
	}


	public interface IMetaInformation
	{
		String OgTitle { get; set; }
		String OgType { get; set; }
		String OgImage { get; set; }
		String OgUrl { get; set; }
		String OgDescription { get; set; }
		String OgSiteName { get; set; }
		String GeoRegion { get; set; }
		String GeoPlaceName { get; set; }
		String GeoPosition { get; set; }
		String Icbm { get; set; }
		String Keywords { get; set; }
		List<String> DetectedImages { get; set; }
	}

	public class MetaInformation : IMetaInformation
	{
		public String OgTitle { get; set; }
		public String OgType { get; set; }
		public String OgImage { get; set; }
		public String OgUrl { get; set; }
		public String OgDescription { get; set; }
		public String OgSiteName { get; set; }

		public String GeoRegion { get; set; }
		public String GeoPlaceName { get; set; }
		public String GeoPosition { get; set; }
		public String Icbm { get; set; }

		public String Keywords { get; set; }
		public List<String> DetectedImages { get; set; }

		public MetaInformation()
		{
			DetectedImages = new List<string>();
		}
	}

	public class LimitedQueue<T> : Queue<T>
	{
		private int limit = -1;

		public int Limit
		{
			get { return limit; }
			set { limit = value; }
		}

		public LimitedQueue(int limit)
			: base(limit)
		{
			this.Limit = limit;
		}

		public new void Enqueue(T item)
		{
			if (this.Count >= this.Limit)
			{
				this.Dequeue();
			}
			base.Enqueue(item);
		}
	}
}
