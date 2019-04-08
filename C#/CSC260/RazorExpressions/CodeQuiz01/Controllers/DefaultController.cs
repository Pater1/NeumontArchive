using CodeQuiz01.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeQuiz01.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index(string id)
        {
            Vehicle v = null;

            if (id != null) {
                Type t = null;
                switch (id) {
                    case "Car":
                        t = typeof(Car);
                        break;
                    case "Boat":
                        t = typeof(Boat);
                        break;
                    case "Plane":
                        t = typeof(Plane);
                        break;
                }
                if (t != null)
                    v = Activator.CreateInstance(t) as Vehicle;
            }

            return View(v);
        }
    }
}