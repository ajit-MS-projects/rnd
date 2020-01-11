using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using WURFL;
using WURFL.Config;
using WurflWeb.Controllers;

namespace WurflWeb
{
	public static class MobileDetectionUtility
	{
		public static IWURFLManager Manager { get; set; }
		private static string wurflDataFile = "~/App_Data/wurfl-latest.zip";

		static MobileDetectionUtility()
		{
			var configurer = new InMemoryConfigurer().MainFile(HostingEnvironment.MapPath(wurflDataFile));
			Manager = WURFLManagerBuilder.Build(configurer);
		}

		public static string  IsTablet(this HttpContext httpContext)
		{
			if (!string.IsNullOrWhiteSpace(httpContext.Request.UserAgent))
			{
				var currentDevice = Manager.GetDeviceForRequest(httpContext.Request.UserAgent);
				if (currentDevice != null) return currentDevice.GetCapability("is_tablet");
			}
			return "";
		}

		public static string GetOsName(this HttpContext httpContext)
		{
			if (!string.IsNullOrWhiteSpace(httpContext.Request.UserAgent))
			{
				var currentDevice = Manager.GetDeviceForRequest(httpContext.Request.UserAgent);
				if (currentDevice != null) return currentDevice.GetCapability("device_os") + " : " + currentDevice.GetCapability("device_os_version") + " : " + currentDevice.GetCapability("brand_name");
			}
			return "";
		}

		public static string GetAllCapabilities(this HttpContext httpContext)
		{
			StringBuilder retVal = new StringBuilder();

			if (!string.IsNullOrWhiteSpace(httpContext.Request.UserAgent))
			{
				var currentDevice = Manager.GetDeviceForRequest(httpContext.Request.UserAgent);
				if (currentDevice != null)
				{
					String json = string.Empty;
					using (var sr = new StreamReader(httpContext.Server.MapPath("~/App_Data/Capabilities.txt")))
					{
						json = sr.ReadToEnd();
					}
					Capabilities caps = Newtonsoft.Json.JsonConvert.DeserializeObject<Capabilities>(json);
					foreach (var cap in caps.CapabilitiyNames)
					{
						if (!string.IsNullOrWhiteSpace(currentDevice.GetCapability(cap)))
							retVal.Append("<b>"+cap + "</b>:" + currentDevice.GetCapability(cap)+"<br/>");
					}
					
				}
			}
			return retVal.ToString();
		}
	}
}