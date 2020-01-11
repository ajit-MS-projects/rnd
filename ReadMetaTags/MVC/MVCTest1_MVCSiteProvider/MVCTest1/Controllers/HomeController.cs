using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using MvcSiteMapProvider;

namespace MVCTest1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Impression()
        {
            return View();
        }

        //
        //GET: News/Article/x
        //[MvcSiteMapNode(Title = "Article", ParentKey = "imp")]
        public ActionResult Article(int id,int ids)
        {
            ViewBag.id = id;
            ViewBag.ids = ids;
            return View();
        }

    }
}
