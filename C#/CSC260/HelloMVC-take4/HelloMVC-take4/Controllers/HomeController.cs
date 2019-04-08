using HelloMVC_take4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelloMVC_take4.Controllers
{
    public class HomeController : Controller {
        [HttpGet]
        public ActionResult Index() {
            return View("RandForm");
        }

        [HttpPost]
        public ViewResult Index(FormResponce formResponse) {
            if (ModelState.IsValid) {
                return View("Thanks", formResponse);
            } else {
                // there is a validation error
                return View("RandForm");
            }
        }

        [HttpGet]
        public ActionResult RandForm() {
            return View();
        }

        [HttpPost]
        public ViewResult RandForm(FormResponce formResponse) {
            if (ModelState.IsValid) {
                return View("Thanks", formResponse);
            } else {
                // there is a validation error
                return View();
            }
        }

//        public string StringingAlong(string id) {
//            string qstr = Request.QueryString["id"];
//            string formstrng = Request.Form["id"];

//            var rd = RouteData.Values["id"];
//            string route = rd==null?"":rd.ToString();

//            return 
//                $@"
//<form action='/Home/StringingAlong/14?id=92' method='post'>
//    <input name='id' type='number' value='5'>
//    <input type='submit' value='Submit Form'>
//</form>
//<p>passed in: {id}</p>
//<p>in queary string: {qstr}</p>
//<p>in form: {formstrng}</p>
//<p>in route: {route}</p>";
//        }
    }
}