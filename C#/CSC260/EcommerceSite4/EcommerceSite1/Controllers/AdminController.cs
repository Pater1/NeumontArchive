using EcommerceSite1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EcommerceSite1.Controllers {
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult UserAdmin() {
            return View("~/Views/AccountManagement/UsersManagementConsole.cshtml");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateUser(UserRoleChangeModel model) {
            using (UsersDB context = new UsersDB()) {
                RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                
                if (roleManager.RoleExists(model.NewRole)) {
                    ApplicationUser user = context.Users.Where(x => x.UserName == model.UserName).FirstOrDefault();

                    if (user != null && user.UserName != System.Web.HttpContext.Current.User.Identity.Name) {
                        var userRoles = user.Roles.ToArray().Select(y => y.RoleId).ToArray();
                        string[] curroles = context.Roles.Where(x => userRoles.Contains(x.Id)).ToArray().Select(x => x.Name).ToArray();
                        UserManager.RemoveFromRoles(user.Id, curroles);

                        UserManager.AddToRole(user.Id, model.NewRole);
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}