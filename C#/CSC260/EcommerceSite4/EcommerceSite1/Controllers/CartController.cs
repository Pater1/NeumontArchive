using EcommerceSite1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcommerceSite1.Controllers
{
    public class CartController : Controller
    {
        public ActionResult Checkout(decimal? name) {
            if (UserHelpers.IsLoggedIn && name != null) {
                using (UsersDB udb = new UsersDB()) {
                    ApplicationUser u = udb.Users.Where(x => UserHelpers.Current.Identity.Name == x.UserName).FirstOrDefault();
                    if (u != null) {
                        using (CartDB cdb = new CartDB()) {
                            ViewBag.Cost = name;
                            cdb.Purge(u);
                            return View();
                        }
                    }
                }
            }
            return RedirectToAction("LoginOrRegister", "Account", null);
        }

        public ActionResult Index()
        {
            if (UserHelpers.IsLoggedIn) {
                using (UsersDB udb = new UsersDB()) {
                    ApplicationUser u = udb.Users.Where(x => UserHelpers.Current.Identity.Name == x.UserName).FirstOrDefault();
                    if (u != null) {
                        using (CartDB cdb = new CartDB()) {
                            return View("Cart", cdb.TuplizeFor(u));
                        }
                    }
                }
            }
            return RedirectToAction("LoginOrRegister", "Account", null);
        }

        public ActionResult Add(string name, int? count) {
            if (count == null) count = 1;
            if (UserHelpers.IsLoggedIn) {
                using (UsersDB udb = new UsersDB()) {
                    ApplicationUser u = udb.Users.Where(x => UserHelpers.Current.Identity.Name == x.UserName).FirstOrDefault();
                    if (u != null) {
                        using (GameDB gdb = new GameDB()) {
                            Game g = gdb.Games.Where(x => x.Title_Spaceless == name).FirstOrDefault();
                            if (g != null) {
                                using (CartDB cdb = new CartDB()) {
                                    cdb.Add(g, u, count.Value);
                                }
                            }
                        }
                    }
                }
            }

            return RedirectToAction("Index");
        }
        public ActionResult Remove(string name, int? count) {
            if (count == null) count = 1;
            return Add(name, -count);
        }
    }
}