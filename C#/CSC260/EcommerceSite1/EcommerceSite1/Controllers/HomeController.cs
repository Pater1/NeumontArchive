using EcommerceSite1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcommerceSite1.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            return View();
        }
        public ActionResult Catalog(string name) {
            if (name == null) {
                return View(Game._defaultGamesList.ToList());
            } else {
                return View("Game",Game._defaultGames[name]);
            }
        }
    }
}