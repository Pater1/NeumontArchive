using EcommerceSite1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EcommerceSite1.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            return View();
        }
        public ActionResult Catalog(string name) {
            using (GameDB db = new GameDB()) {
                if (name != null) {
                    //int id = 0;
                    Game g = null;

                    //if (int.TryParse(name, out id)) {
                    //    g = db.Games.Where(x => x.ID == id).FirstOrDefault();
                    //} else {
                        g = db.Games.Where(x => x.Title_Spaceless == name).FirstOrDefault();
                    //}

                    if (g != null) {
                        return View("Game", g);
                    }
                }

                //bool done = false;
                //while (!done) {
                //    db.SaveChanges();
                //    bool check = false;
                //    foreach (Game g in db.Games) {
                //        int count = db.Games.Where(x => x.Title == g.Title).ToArray().Length;
                //        if (count > 1) {
                //            db.Games.Remove(g);
                //            check = true;
                //            break;
                //        }
                //    }
                //    if (!done && !check) {
                //        done = !done;
                //    }
                //}
                //db.SaveChanges();

                foreach (Game g in db.Games) {
                    g.Link = g.Title_Spaceless;
                    db.Games.AddOrUpdate(g);
                }
                db.SaveChanges();

                return View(db.Games.Take(100).ToList());
            }
        }

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
        public ActionResult NewGame(Game formData) {
            using (GameDB db = new GameDB()) {
                //if (formData.ID == 0) {
                //    Random rand = new Random();
                //    do {
                //        formData.ID = rand.Next();
                //    } while (db.Games.Where(x => x.ID == formData.ID).FirstOrDefault() != null);
                //}

                db.Games.AddOrUpdate(formData);
                db.SaveChanges();
            }

            return Catalog(formData.Title_Spaceless.ToString());
        }
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