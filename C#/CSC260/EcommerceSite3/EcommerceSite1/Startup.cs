using EcommerceSite1.Migrations;
using EcommerceSite1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Newtonsoft.Json;
using Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;

[assembly: OwinStartupAttribute(typeof(EcommerceSite1.Startup))]
namespace EcommerceSite1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            CreateRolesAndUsers();

            using(GameDB db = new GameDB()) {
                if(db.Games.Count() <= 0) {
                    foreach(Game g in Game._defaultGamesList) {
                        db.Games.Add(g);
                    }
                    db.SaveChanges();
                }
            }

            #region Contained and MigrateDatabases function from Stack Overflow
            //Credit to Stack Overflow user: maxmantz
            //https://stackoverflow.com/questions/20907826/entity-framework-code-first-migration-to-multiple-database
            string json;
            var path = HttpRuntime.AppDomainAppPath;
            using (var reader = new StreamReader(path + @"Config\clients.json")) {
                json = reader.ReadToEnd();
            }

            var databases = JsonConvert.DeserializeObject<IDictionary<string, string>>(json);
            MigrateDatabases(databases);
            #endregion
        }

        #region Roles
        private void CreateRolesAndUsers() {
            using (UsersDB context = new UsersDB()) {
                RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                // Creating first Admin Role and creating a default Admin User    
                if (!roleManager.RoleExists("Admin")) {

                    // first we create Admin rool   
                    IdentityRole role = new IdentityRole();
                    role.Name = "Admin";
                    roleManager.Create(role);
                    
                    //Here we create a Admin super user who will maintain the website               
                    ApplicationUser user = new ApplicationUser();
                    user.UserName = "admin";
                    user.Email = "admin@admin.com";

                    string userPWD = "IAmAdmin@01";

                    IdentityResult chkUser = UserManager.Create(user, userPWD);

                    //Add default User to Role Admin   
                    if (chkUser.Succeeded) {
                        IdentityResult result1 = UserManager.AddToRole(user.Id, "Admin");
                    }
                }

                if (!roleManager.RoleExists("Mod")) {
                    IdentityRole role = new IdentityRole();
                    role.Name = "Mod";
                    roleManager.Create(role);
                }

                if (!roleManager.RoleExists("User")) {
                    IdentityRole role = new IdentityRole();
                    role.Name = "User";
                    roleManager.Create(role);
                }
            }
        }
#endregion

        public static void MigrateDatabases(IDictionary<string, string> databaseConfigs) {
            foreach (var db in databaseConfigs) {
                var config = new Configuration {
                    //TODO: reconfigure to not require configs in Web.config
//                    TargetDatabase = new DbConnectionInfo(db.Value, "System.Data.SqlClient")
                    TargetDatabase = new DbConnectionInfo(db.Value)
                };

                var migrator = new DbMigrator(config);
                migrator.Update();
            }
        }
    }
}
