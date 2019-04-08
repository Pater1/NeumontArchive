using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OverloadingOperators;

namespace FracMathTest
{
    public class Class1
    {
        public static void Main(string[] args)
        {
            Fraction a = new Fraction(0, 5, 3);
            a.MakeProper();

            Console.WriteLine(a);

            Console.WriteLine(new Fraction(5,0,2));
        }
    }
}
