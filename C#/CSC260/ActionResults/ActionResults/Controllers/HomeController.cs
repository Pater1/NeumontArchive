using ActionResults.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ActionResults.Controllers
{
    public class HomeController : Controller
    {
        Character cha = new Character();
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.Char = cha;
            return View();
        }

        public string AsString() {
            return cha.ToString();
        }

        public JsonResult AsJson() {
            return Json(cha, JsonRequestBehavior.AllowGet);
        }
        
        XmlSerializer xmls = new XmlSerializer(typeof(Character));
        public ContentResult AsXML() {
            StringWriter txt = new StringWriter();
            xmls.Serialize(txt, cha);
            return Content(txt.ToString(), "text/xml");
        }

        public ActionResult As404() {
            return new HttpNotFoundResult("Page no here!");
        }
        
        public FileContentResult AsDownload() {
            string path = Server.MapPath("~/Resources/A_Picture.jpg");
            byte[] fi;
            using (FileStream sourceStream = new FileStream(path, FileMode.Open)) {
                using (MemoryStream memoryStream = new MemoryStream()) {
                    sourceStream.CopyTo(memoryStream);
                    fi = memoryStream.ToArray();
                }
            }
            FileContentResult ret = new FileContentResult(fi, "image/jpeg");
            ret.FileDownloadName = "APictureFromThisServer.jpg";

            return ret;
        }
    }
}