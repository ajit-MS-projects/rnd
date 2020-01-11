using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC3_app.Controllers
{
    public class HelloController : Controller
    {
        //
        // GET: /Hello/

        public ActionResult Index1()
        {
            return View();
        }

        public String Welcome(String name,int num=1)
        {
            return HttpUtility.HtmlEncode("Hello " + name + ", NumTimes is: " + num);
        }
    }
}
