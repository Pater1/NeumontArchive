using EcommerceSite1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcommerceSite1.Controllers {
    //[RequireHttps]
    public class HomeController : Controller {
        public ActionResult Index() {
            return View();
        }
    }
}