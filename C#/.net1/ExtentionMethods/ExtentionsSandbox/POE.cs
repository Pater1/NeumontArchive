using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Collections;

namespace ExtentionMethods
{
    public class MainClass
    {
        public static void Main(string[] args)
        {
            bool ro = args.Contains("--RandOnly");
            List<object> strs = new List<object>()
            {
                "Hi",
                "Heloo",
                3456,
                "Werlfd",
                3.1415,
                "Pi"
            };

if(!ro)     Console.Write("Print() on {\"Hi\", \"Heloo\", 3456, \"Werlfd\", 3.1415, \"Pi\"}:\n\t");
            strs.Print();

if(!ro){    Console.WriteLine("");
            Console.WriteLine("\"tacocat\".IsPalindrome() = " + "tacocat".IsPalindrome());
            Console.WriteLine("\"tAcoCat\".IsPalindrome() = " + "tAcoCat".IsPalindrome());
            Console.WriteLine("\"taco cat\".IsPalindrome() = " + "taco cat".IsPalindrome());
            Console.WriteLine("\"Dave\".IsPalindrome() = " + "Dave".IsPalindrome());

            Console.WriteLine("");

            Console.WriteLine("/* int.ToPower(int) Truncates to nearest int in the event of a non-integer result, and does not validate for overflow!\n * Negative numbers are treated as calls for that number's Nth root, not as the traditional mathamatical definition of 1/(x^n).\n */This behavior was decided upon, as Nth root operations seem more valuable to support than a shorthand for \"1/int.ToPower(int)\".");
            Console.WriteLine("5.ToPower(3) = " + 5.ToPower(3));
            Console.WriteLine("500.ToPower(0) = " + 500.ToPower(0));
            Console.WriteLine("500.ToPower(1) = " + 500.ToPower(1));
            Console.WriteLine("25.ToPower(-2) = " + 25.ToPower(-2));

            Console.WriteLine("");

            Console.WriteLine("Write { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 } to XML, and Inject into provided doc.");
}           List<int> Is = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
//            IEnumerable<int> lnq = Is.Where(n => n % 2 == 0).OrderBy(n => n).Select(n => n);
            XmlDocument doc = new XmlDocument();
            doc.Inject(Is);
            //below just for printing XML to console
            StringWriter tw = new StringWriter();
            doc.WriteTo(new XmlTextWriter(tw));
            Console.WriteLine(tw.GetStringBuilder().ToString());

            Console.WriteLine("");

            Console.WriteLine("Eject same list from XML doc (into List<int> js).");
            List<int> Js = doc.Eject<List<int>>();
if(!ro)     Js.Print();

if(!ro)     Console.WriteLine("");

            Console.WriteLine("Get a random value from the list.");
            int j = Js.GetRandom();
            Console.WriteLine("Js.GetRandom() = " + j);

            Console.WriteLine("");

            Console.WriteLine("This happens to use an easy rand extention...");
            int k = 50.GetRandom(24);
            Console.WriteLine("50.GetRandom(24) = " + k);

            Console.WriteLine("");

            Console.WriteLine("It also works with any type (using original Print() List)");
            object o = strs.GetRandom();
            Console.WriteLine("strs.GetRandom() = " + o);

            Console.WriteLine("");

            Console.WriteLine("Let's check if that first random value we got was prime (manually)");
            bool b = j.EqualsAny(2, 3, 5, 7);
            Console.WriteLine("j.EqualsAny(2,3,5,7) = " + b);

            Console.WriteLine("");

            Console.WriteLine("Or if the strs.GetRandom() was one of the numbers");
            bool c = o.EqualsAny(3456, 3.1415, "Pi");
            Console.WriteLine("o.EqualsAny(3456, 3.1415, \"Pi\") = " + c);

            if (ro) return;
            Console.WriteLine("");

            Console.WriteLine("Let's add some values to Js (Homemade syntactic sugar!)");
            Js.AddRange(0, 500, 314);
            Console.WriteLine("Js.AddRange(0, 500, 314));"); Js.Print();

            Console.WriteLine("");

            Console.WriteLine("Wait... is Js null? (MORE homemade syntactic sugar!)");
            Js.ThrowIfNull("Js");
            Console.WriteLine("Js.ThrowIfNull(\"Js\");");

            Console.WriteLine("");

            Console.WriteLine("I'm not sure if that worked... Maybe we should test it fir-");
            //just a random type to demonstrate how the default throw message works
            System.Threading.Barrier thisDefinitelyNullVariable = null;
            Console.WriteLine("thisDefinitelyNullVariable.ThrowIfNull();\n");
            try
            {
                thisDefinitelyNullVariable.ThrowIfNull();
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine("");
        }
    }
}