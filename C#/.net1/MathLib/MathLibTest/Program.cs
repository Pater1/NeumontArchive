using System;
using MathLib;

namespace MathLibTest
{
    class Program
    {
        static void Main(string[] args)
        {
            double a = 5.0.Subtract(10.0f, 10.0f).Multiply(54).Add(15.125f).Divide(2).Power(3, 4).Modulo(4000);
            Console.WriteLine(a);
        }
    }
}
