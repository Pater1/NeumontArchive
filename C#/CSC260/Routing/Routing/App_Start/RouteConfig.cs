using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Routing
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "EatMoreChicken",
                url: "EatMoreChicken",
                defaults: new { controller = "Home", action = "EatMoreChicken" }
            );

            routes.MapRoute(
                name: "About",
                url: "Moo",
                defaults: new { controller = "Home", action = "AboutUs" }
            );
            routes.MapRoute(
                name: "MooCount",
                url: "Moo{count}",
                defaults: new { controller = "Mooing", action = "MooCount" }
            );

            routes.MapRoute(
                name: "AllGallery",
                url: "AllCows/Gallery",
                defaults: new { controller = "Mooing", action = "Gallery" }
            );
            routes.MapRoute(
                name: "AllGalleryPage",
                url: "AllCows/Gallery/Page{page}",
                defaults: new { controller = "Mooing", action = "GalleryPage" }
            );
            routes.MapRoute(
                name: "AllGalleryCount",
                url: "AllCows/Gallery/{count}",
                defaults: new { controller = "Mooing", action = "GalleryCount" }
            );
            routes.MapRoute(
                name: "AllGalleryCountPage",
                url: "AllCows/Gallery/{count}/Page{page}",
                defaults: new { controller = "Mooing", action = "GalleryCountPage" }
            );
            routes.MapRoute(
                name: "AllGalleryCountPage2",
                url: "AllCows/Gallery/{count}/{page}",
                defaults: new { controller = "Mooing", action = "GalleryCountPage" }
            );
            routes.MapRoute(
                name: "WhoGallery",
                url: "{cow}/Gallery",
                defaults: new { controller = "Mooing", action = "AboutCow" }
            );

            routes.MapRoute(
                name: "WhoMoosAtYou",
                url: "{count}/{cow}",
                defaults: new { controller = "Mooing", action = "WhoAtYou" }
            );
            routes.MapRoute(
                name: "MoosAtYou",
                url: "{count}",
                defaults: new { controller = "Mooing", action = "AtYou" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
