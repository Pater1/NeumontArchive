using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Routing.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AboutUs() {
            return View();
        }
        public ActionResult EatMoreChicken() {
            return Redirect("https://www.chick-fil-a.com");
        }

    }
}