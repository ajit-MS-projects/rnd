using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace UmbracoLearn.Controllers
{
    public class NewsLetterController : RenderMvcController 
    {
        public ActionResult Index(string s)
        {
            return View();

        }

        public ActionResult Register(string s)
        {
            return View();
        }

        public ActionResult UnRegister()
        {
            return View();
        }

    }
}
