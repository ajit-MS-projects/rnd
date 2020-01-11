	using System;
	using System.Drawing;
	using System.Linq;
	using System.Text.RegularExpressions;
	using System.Web;
	using System.Web.Hosting;

	using WURFL;
	using WURFL.Config;
	using WURFL.Request;


namespace WurflMobileLib
{


		public static class MobileDetectionUtility
		{
			public static IWURFLManager Manager { get; set; }

			private static Regex androidUserAgent = new Regex(@"(;\sAndroid\s)",
				RegexOptions.IgnoreCase
				| RegexOptions.CultureInvariant
				| RegexOptions.IgnorePatternWhitespace
				| RegexOptions.Compiled
				);

			private static Regex iOSUserAgent = new Regex(@"\((iPhone|iPod|iPad)",
				RegexOptions.IgnoreCase
				| RegexOptions.CultureInvariant
				| RegexOptions.IgnorePatternWhitespace
				| RegexOptions.Compiled
				);

			private static string wurflDataFile = "~/App_Data/wurfl-latest.zip";

			private const string MobileStatusCookie = "MobileStatus";
			private const string MobileAppStatusCookie = "MobileAppStatus";

			private const string IsMobileVersionKey = "isMobileVersion";
			private const string IsMobileAppVersionKey = "isMobileAppVersionKey";

			/// <summary>
			/// Initializes the <see cref="MobileDetectionUtility"/> class.
			/// </summary>
			static MobileDetectionUtility()
			{
				var configurer = new InMemoryConfigurer().MainFile(HostingEnvironment.MapPath(wurflDataFile));
				Manager = WURFLManagerBuilder.Build(configurer);
			}

			public static void Init()
			{

			}


			/// <summary>
			/// Resolutions the specified HTTP context.
			/// </summary>
			/// <param name="httpContext">The HTTP context.</param>
			/// <returns></returns>
			public static Point Resolution(this HttpContext httpContext)
			{
				IDevice currentDevice = null;
				if (!string.IsNullOrWhiteSpace(httpContext.Request.UserAgent))
				{
					currentDevice = Manager.GetDeviceForRequest(httpContext.Request.UserAgent);
				}

				int width;
				int height;
				if (currentDevice == null || !int.TryParse(currentDevice.GetCapability("resolution_width"), out width))
				{
					width = 320;
				}
				if (currentDevice == null || !int.TryParse(currentDevice.GetCapability("resolution_height"), out height))
				{
					height = 480;
				}

				return new Point(width, height);

			}

			/// <summary>
			/// Determines whether the specified HTTP context is tablet.
			/// </summary>
			/// <param name="httpContext">The HTTP context.</param>
			/// <returns>
			///   <c>true</c> if the specified HTTP context is tablet; otherwise, <c>false</c>.
			/// </returns>
			public static bool IsTablet(this HttpContext httpContext)
			{
				if (!string.IsNullOrWhiteSpace(httpContext.Request.UserAgent))
				{
					var currentDevice = Manager.GetDeviceForRequest(httpContext.Request.UserAgent);
					return currentDevice != null && currentDevice.GetCapability("is_tablet") == "true";
				}
				return false;
			}

			/// <summary>
			/// Determines whether [is mobile version].
			/// </summary>
			/// <returns>
			///   <c>true</c> if [is mobile version]; otherwise, <c>false</c>.
			/// </returns>
			public static bool IsMobileVersion()
			{
				if (HttpContext.Current != null)
				{
					return IsMobileVersion(HttpContext.Current);
				}
				return false;
			}

			/// <summary>
			/// Determines whether [is mobile version] [the specified HTTP context].
			/// </summary>
			/// <param name="httpContext">The HTTP context.</param>
			/// <returns>
			///   <c>true</c> if [is mobile version] [the specified HTTP context]; otherwise, <c>false</c>.
			/// </returns>
			public static bool IsMobileVersion(this HttpContext httpContext)
			{
				HttpCookie mobileCookie = null;
				bool isMobileVersion = false;

				if (httpContext.Request.Cookies.AllKeys.Contains(MobileStatusCookie))
				{
					mobileCookie = httpContext.Request.Cookies[MobileStatusCookie];
				}
				if (httpContext.Response.Cookies.AllKeys.Contains(MobileStatusCookie))
				{
					mobileCookie = httpContext.Response.Cookies[MobileStatusCookie];
				}

				if (mobileCookie != null && bool.TryParse(mobileCookie.Values[IsMobileVersionKey], out isMobileVersion))
				{

				}
				else
				{
					if (!string.IsNullOrWhiteSpace(httpContext.Request.UserAgent))
					{
						var currentDevice = Manager.GetDeviceForRequest(httpContext.Request.UserAgent);
						if (currentDevice != null)
						{
							var wurflRequest = new WURFLRequest(currentDevice.UserAgent);

							isMobileVersion = (currentDevice.GetCapability("is_tablet") == "false") && (wurflRequest != null && wurflRequest.IsMobileRequest);
						}
					}
				}

				if (!httpContext.Response.Cookies.AllKeys.Contains(MobileStatusCookie))
				{
					mobileCookie = new HttpCookie(MobileStatusCookie)
					{
						Expires = DateTime.UtcNow.AddMonths(1),
						HttpOnly = true,
					};

					mobileCookie.Values.Add(IsMobileVersionKey, isMobileVersion.ToString());

					httpContext.Response.Cookies.Add(mobileCookie);
				}

				return isMobileVersion;
			}


			/// <summary>
			/// Determines whether [is mobile app version].
			/// </summary>
			/// <returns>
			///   <c>true</c> if [is mobile app version]; otherwise, <c>false</c>.
			/// </returns>
			public static bool IsMobileAppVersion()
			{
				if (HttpContext.Current != null)
				{
					return IsMobileAppVersion(HttpContext.Current);
				}
				return false;
			}

			/// <summary>
			/// Determines whether [is mobile app version] [the specified HTTP context].
			/// </summary>
			/// <param name="httpContext">The HTTP context.</param>
			/// <returns>
			///   <c>true</c> if [is mobile app version] [the specified HTTP context]; otherwise, <c>false</c>.
			/// </returns>
			public static bool IsMobileAppVersion(this HttpContext httpContext)
			{
				HttpCookie mobileAppCookie = null;
				bool isMobileAppVersion = false;

				if (httpContext.Request.Cookies.AllKeys.Contains(MobileAppStatusCookie))
				{
					mobileAppCookie = httpContext.Request.Cookies[MobileAppStatusCookie];
				}
				if (httpContext.Response.Cookies.AllKeys.Contains(MobileAppStatusCookie))
				{
					mobileAppCookie = httpContext.Response.Cookies[MobileAppStatusCookie];
				}

				if (mobileAppCookie != null && bool.TryParse(mobileAppCookie.Values[IsMobileAppVersionKey], out isMobileAppVersion))
				{

				}
				return isMobileAppVersion;
			}

			/// <summary>
			/// Sets the mobile app version.
			/// </summary>
			public static void SetMobileAppVersion()
			{
				var httpContext = HttpContext.Current;
				if (httpContext != null)
				{
					if (IsMobileVersion(httpContext))
					{
						if (!httpContext.Request.Cookies.AllKeys.Contains(MobileAppStatusCookie))
						{
							var mobileAppCookie = new HttpCookie(MobileAppStatusCookie)
							{
								Expires = DateTime.UtcNow.AddMonths(1),
								HttpOnly = true,
							};
							mobileAppCookie.Values.Add(IsMobileAppVersionKey, (true).ToString());
							httpContext.Response.Cookies.Add(mobileAppCookie);
						}
					}
				}
			}

			/// <summary>
			/// Determines if is android app.
			/// </summary>
			/// <returns>
			///   <c>true</c> if [is android app]; otherwise, <c>false</c>.
			/// </returns>
			public static bool IsAndroidApp()
			{
				return HttpContext.Current.Request.UserAgent != null && androidUserAgent.Match(HttpContext.Current.Request.UserAgent).Success;
			}

			/// <summary>
			/// Determines if is iOS app.
			/// </summary>
			/// <returns>
			///   <c>true</c> if [is iOS app]; otherwise, <c>false</c>.
			/// </returns>
			public static bool IsiOSApp()
			{
				return HttpContext.Current.Request.UserAgent != null && iOSUserAgent.Match(HttpContext.Current.Request.UserAgent).Success;
			}
		}
	}
}
