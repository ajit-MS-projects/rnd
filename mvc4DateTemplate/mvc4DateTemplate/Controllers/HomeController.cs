using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mvc4DateTemplate.Models;

namespace mvc4DateTemplate.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Product()
        {
            ProductViewModel p = new ProductViewModel() { CreateDate = DateTime.Now.AddDays(22),Id=4,Title = "sony camera"};
            return View(p);
        }
        public ActionResult EditProduct()
        {
            ProductViewModel p = new ProductViewModel() { CreateDate = DateTime.Now.AddDays(22), Id = 4, Title = "sony camera for edit" };
            return View(p);
        }
        [HttpPost]
        public ActionResult EditProduct(ProductViewModel p)
        {
            p.Title = "abcdefgh";
            return View(p);
        }
    }
}
