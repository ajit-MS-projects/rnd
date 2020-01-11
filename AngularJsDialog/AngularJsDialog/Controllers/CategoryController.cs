using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AngularJsDialog.Controllers
{
    public class CategoryController : ApiController
    {
        [HttpGet]
        public List<string> GetCategories()
        {
            var retVal = new List<string>();
            retVal.Add("C1");
            retVal.Add("C2");
            retVal.Add("C3");
            retVal.Add("C4");

            return retVal;
        }
    }
}
