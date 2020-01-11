using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Check24.Contracts.Services;
using Check24.Data.Models;
using Check24.Hpb.Models;
using Check24.Hpb.Services;

namespace Check24.Hpb.Controllers
{
    public class BiddingController : Controller
    {
        IAuctionService auctionService = new AuctionService();
        public JsonResult PlaceBid(BidModel bid)
        {
            var placedBid = auctionService.PlaceBid(bid);
            return Json(placedBid, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAuction(int auctionId)
        {
            var auction = auctionService.GetAuction(auctionId);
            return Json(auction, JsonRequestBehavior.AllowGet);
        }
        public JsonResult PlaceAuction(int auctionId)
        {
            var auction = auctionService.PlaceAuction(new Auction());
            return Json(auction, JsonRequestBehavior.AllowGet);
        }
    }
}
