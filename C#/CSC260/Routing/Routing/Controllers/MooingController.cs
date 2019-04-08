using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Routing.Controllers
{
    public class MooingController : Controller {
        // GET: Mooing
        public string AtYou(int count) {
            return $"The cow DefaultCow moos at you {count} times.";
        }

        public string WhoAtYou(int count, string cow) {
            return $"The cow {cow} moos at you {count} times.";
        }
        public string AboutCow(string cow) {
            return $"This is a page all about {cow}!";
        }

        
        public string Gallery() {
            return GalleryCountPage(cowNames.Count, 0);
        }
        public string GalleryCount(int count) {
            return GalleryCountPage(count, 0);
        }
        public string GalleryPage(int page) {
            return GalleryCountPage(5, page);
        }
        private static readonly Random rand = new Random();
        private static readonly List<string> cowNames = new List<string>() {
            "Bully",
            "Duke",
            "Hydro",
            "Homer",
            "Earl",
            "Diesel",
            "Horns",
            "Armor",
            "Bob",
            "Sampson",
            "Tank",
            "Kristof",
            "Angus",
            "Midnight",
            "Nitrous",
            "Rose",
            "Darla",
            "Meg",
            "Dahlia",
            "Margie",
            "Lois",
            "Flower",
            "Maggie",
            "Jasmine",
            "Minnie",
            "Esmeralda",
            "Bella",
            "Daisy",
            "Shelly",
            "Candie",
            "Bessie",
            "Clarabelle",
            "Betty Sue",
            "Emma",
            "Henrietta",
            "Ella",
            "Penelope",
            "Nettie",
            "Anna",
            "Bella",
            "Annabelle",
            "Dorothy",
            "Molly",
            "Gertie",
            "Annie",
            "Honeybun",
            "Cookie",
            "Pinky",
            "Sweetie",
            "Sunny",
            "Blue",
            "LovaBull",
            "Sunshine",
            "Sugar",
            "Cupcake",
            "Cocoa",
            "Booboo",
            "Baby",
            "Muffin",
            "Princess",
            "Big Mac",
            "Moscow",
            "MooMoo",
            "Madonna",
            "Mud Pie",
            "Cowlick",
            "Bertha",
            "Shrimp",
            "Ineda Bunn",
            "Waffles",
            "Brown Cow",
            "Mickey D.",
            "Hamburger",
            "Humphry",
            "Heifer"
        }.OrderBy(a => Guid.NewGuid()).ToList();
        private static string TagWrap(string raw, string tag) => $"<{tag}>{raw}</{tag}>";
        public string GalleryCountPage(int count, int page) {
            string ret = "";
            int start = count * page;
            if (start >= cowNames.Count) {
                return "There are no cows here. The count and page are boyond the bounds of the number of cows on this site!";
            }
            int end = count * (page + 1);
            end = end < cowNames.Count ? end : cowNames.Count;
            for (int i = start; i < end; i++) {
                ret += TagWrap(cowNames[i], "p");
            }
            return ret;
        }


        public string MooCount(int count) {
            string ret = "";
            for(int i = 0; i < count; i++) {
                ret += "moo ";
            }
            return ret;
        }
    }
}