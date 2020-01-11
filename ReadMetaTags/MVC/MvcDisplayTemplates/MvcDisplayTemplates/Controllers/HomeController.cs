using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcDisplayTemplates.Models;

namespace MvcDisplayTemplates.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ViewResult Details([DefaultValue(0)] int id)
        {
            Contact c = new Contact() {Age = 5, FirstName = "Mario", LastName = "Gomez"};
            return View(c);
        }
    }
}
