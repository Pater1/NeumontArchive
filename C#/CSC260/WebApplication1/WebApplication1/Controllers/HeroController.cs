using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class HeroController : Controller {
        public ActionResult Heros() {
            return View();
        }

        public ActionResult Maps() {
            return View();
        }
    }
}