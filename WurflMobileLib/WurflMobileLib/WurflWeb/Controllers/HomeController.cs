using System;
using System.Linq;
using System.Web.Mvc;

namespace WurflWeb.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
        	ViewBag.OsName = System.Web.HttpContext.Current.GetOsName();
        	ViewBag.IsTablet = System.Web.HttpContext.Current.IsTablet();
			Response.Write(System.Web.HttpContext.Current.GetAllCapabilities());
			//Capabilities capabilities = new Capabilities();
			//capabilities.CapabilitiyNames.Add("test1");
			//capabilities.CapabilitiyNames.Add("test2");
			//capabilities.CapabilitiyNames.Add("test3");

			//ViewBag.json = Newtonsoft.Json.JsonConvert.SerializeObject(capabilities);
			return View();
        }

    }
}
