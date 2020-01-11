using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WinAppFabricCacheClient.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            ProductViewModel p = new ProductViewModel(){};
            return View(p);
        }

        public ActionResult Save()
        {
            ProductViewModel p = new ProductViewModel() {Id=1,Title = "Ram 16gb"};

            return View(p);
        }

    }
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
