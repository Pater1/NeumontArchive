using System.Data.Entity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceSite1.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager) {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class UsersDB : IdentityDbContext<ApplicationUser> {
        public UsersDB() { }
        public UsersDB(string connection_name) : base(connection_name) { }

        public static UsersDB Create()
        {
            return new UsersDB();
        }
    }

    public class UsersDBInit<T> : DropCreateDatabaseAlways<UsersDB> where T : new() {
        protected override void Seed(UsersDB context) {
            //ApplicationUserManager aum = new ApplicationUserManager();

            //ApplicationUser user = new ApplicationUser { UserName = "Admin", Email = "noMail@Admin.cmd" };

            //Task<IdentityResult> t = aum.CreateAsync(user, "letTheAdminIn");
            //t.RunSynchronously();
            //IdentityResult result = t.Result;

            //base.Seed(context);
        }
    }
}