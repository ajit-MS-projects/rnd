using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Traffective.Services;

namespace Traffective.Tracking.Controllers
{
    public class TrackingController : Controller
    {
        //
        // GET: /Tracking/

        public ActionResult Click(Tracking.Models.Tracking tracking)
        {
            new TrackingService().SaveTrackingInfo(tracking);
            return View();
        }

       
        public ActionResult TView()
        {
            return View();
        }
    }
}
