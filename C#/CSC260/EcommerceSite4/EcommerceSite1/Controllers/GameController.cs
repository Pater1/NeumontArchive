using EcommerceSite1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Migrations;

namespace EcommerceSite1.Controllers {
    [Authorize]
    public class GameController : Controller {
        [AllowAnonymous]
        public ActionResult Catalog(string name) {
            using (GameDB db = new GameDB()) {
                if (name != null) {
                    IEnumerable<Game> gs = db.Games.Where(x => x.Title.ToUpper().Contains(name.ToUpper()));
                    if (gs.Count() == 1) {
                        return View("Game", gs.FirstOrDefault());
                    } else if (gs.Count() > 1) {
                        return View(gs.ToList());
                    } else {
                        ViewBag.message = "There are no game with " + name + " in our title database. Much sorry...";
                        return View(new Game[0]);
                    }
                }

                return View(db.Games.Take(100).ToList());
            }
        }
        [AllowAnonymous]
        public ActionResult Game(string name) {
            using (GameDB db = new GameDB()) {
                if (name != null) {
                    Game g = null;

                    g = db.Games.Where(x => x.Title_Spaceless == name).FirstOrDefault();

                    if (g != null) {
                        return View("Game", g);
                    } else {
                        return RedirectToAction("Catalog", name);
                    }
                }

                return RedirectToAction("Catalog", "");
            }
        }

        [Authorize(Roles = "Admin,Mod")]
        public ActionResult AddAGame(string name) {
            Game g = null;
            if (name != null) {
                using (GameDB db = new GameDB()) {
                    //int id = 0;

                    //if (int.TryParse(name, out id)) {
                    //    g = db.Games.Where(x => x.ID == id).FirstOrDefault();
                    //} else {
                    g = db.Games.Where(x => x.Title_Spaceless == name).FirstOrDefault();
                    //}
                }
            }
            return View("AddAGame", g);
        }
        [Authorize(Roles = "Admin,Mod")]
        public ActionResult NewGame(Game formData) {
            using (GameDB db = new GameDB()) {
                db.Games.AddOrUpdate(formData);
                db.SaveChanges();
            }

            return Catalog(formData.Title_Spaceless.ToString());
        }
        [Authorize(Roles = "Admin,Mod")]
        public ActionResult RemoveAGame(string name) {
            Game g = null;
            if (name != null) {
                using (GameDB db = new GameDB()) {
                    g = db.Games.Where(x => x.Title_Spaceless == name).FirstOrDefault();
                    db.Games.Remove(g);
                    db.SaveChanges();
                }
            }
            return View("RemoveAGame", g);
        }

    }
}