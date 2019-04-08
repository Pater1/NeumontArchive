using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Security.Claims;
using System.Web;

namespace EcommerceSite1 {
    public static class UserHelpers {
        public static IPrincipal Current {
            get {
                return System.Web.HttpContext.Current.User;
            }
        }
        public static bool IsInRole(string role) {
            if (IsLoggedIn) {
                return Current.IsInRole(role);
            } else {
                return false;
            }
        }
        public static bool IsLoggedIn {
            get {
                return (Current != null) && Current.Identity.IsAuthenticated;
            }
        }
        public static string LoggedInName {
            get {
                if (IsLoggedIn) {
                    string ret = Current.Identity.Name;
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