using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TicTacToe.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Tic(string gameId)
        {
            return View();
        }
        public ActionResult PlayerSelection()
        {
            var games = TicHub.GetAvaialbleHosts();

            return View(games);
        }
        public string GameOptions(string gameId)
        {

            return View();
        }
    }
    public class GameOptions
    {
        public string GameId { get; set; }
    }
}