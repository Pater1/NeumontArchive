using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcommerceSite1 {
    public static class UserHelpers {
        public static bool IsInRole(string role) {
            if (IsLoggedIn) {
                return System.Web.HttpContext.Current.User.IsInRole(role);
            } else {
                return false;
            }
        }
        public static bool IsLoggedIn {
            get {
                return (System.Web.HttpContext.Current.User != null) && System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            }
        }
        public static string LoggedInName {
            get {
                if (IsLoggedIn) {
                    string ret = System.Web.HttpContext.Current.User.Identity.Name;
                    if (ret.Contains('@')) {
                        ret = ret.Split('@')[0];
                    }
                    return ret;
                } else {
                    return null;
                }
            }
        }
    }
}