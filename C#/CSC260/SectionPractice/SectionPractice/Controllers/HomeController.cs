using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SectionPractice.Controllers {
    public class HomeController : Controller {
        private Dictionary<string, string> layoutNamesToFiles = new Dictionary<string, string>() {
            {"Bootstrap", @"\Views\Shared\Bootstrap_Layout.cshtml"},
            {"Boxy", @"\Views\Shared\ABox_Layout.cshtml"},
            {"Lazy", @"\Views\Shared\Lazy_Layout.cshtml"}
        };
        public ActionResult Index(string layoutName) {
            if (layoutName == null || !layoutNamesToFiles.ContainsKey(layoutName)) layoutName = "Bootstrap";

            ViewBag.LayoutFile = layoutNamesToFiles[layoutName];
            return View();
        }
    }
}