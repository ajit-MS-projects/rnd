using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC3Test24.Models;

namespace MVC3Test24.Controllers
{
    public class CustomerController : Controller
    {
        //
        // GET: /Customer/

        public ActionResult Index()
        {
            Customer c = new Customer();
            c.Id = 111;
            c.Name = "Ajit";
            return View(c);
        }

    }
}
