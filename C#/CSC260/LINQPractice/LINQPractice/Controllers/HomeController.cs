using LINQPractice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LINQPractice.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            return View(@"~\Views\StudentCollectionView.cshtml", Student.s_DefaultStudents.OrderBy(x => x._NameLast));
        }
        public ActionResult Ladies() {
            return View(@"~\Views\StudentCollectionView.cshtml", Student.s_DefaultStudents.Where(x => x._Gender == Student.Gender.Female).OrderBy(x => x._NameLast));
        }
        public ActionResult Adults() {
            return View(@"~\Views\StudentCollectionView.cshtml", Student.s_DefaultStudents.Where(x => x._Age >= 18).OrderBy(x => x._NameLast));
        }
        public ActionResult ReverseFirst() {
            return View(@"~\Views\StudentCollectionView.cshtml", Student.s_DefaultStudents.OrderBy(x => x._NameLast).ThenByDescending(x => x._NameFirst));
        }
        public ActionResult ModZero(int? id) {
            int mod = id.HasValue ? id.Value : 1;
            return View(@"~\Views\StudentCollectionView.cshtml", Student.s_DefaultStudents.Where(x => x._ID % mod == 0));
        }
    }
}