using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC3_app.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult Index1()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC 111111111!";

            return View();
        }
        public ActionResult About()
        {
            return View();
        }
    }
}
