using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverloadingOperators
{
    public struct Fraction
    {
        #region Static Members
        #region Static Functions
        public static long GCD(long x, long y)
        {
            return y == 0 ? x : GCD(y, (x % y));
        }
        public static long LCM(long x, long y)
        {
            return (x*y) / GCD(x, y);
        }
        private static void CommonizeBase(ref Fraction a, ref Fraction b)
        {
            long lcm = LCM(a.Denominator, b.Denominator);
            long aDelta = lcm / a.Denominator;
            long bDelta = lcm / b.Denominator;

            a = new Fraction(a.WholeNumber, a.Numerator * aDelta * (a.isNegative?-1:1), lcm);
            b = new Fraction(b.WholeNumber, b.Numerator * bDelta * (b.isNegative ? -1 : 1), lcm);
        }
        #endregion

        #region Parcing Functions
        //RealToFraction function found via Stack Overflow.
        //Credit to Stack Exchange user Kay Zed for this C# translation of the Ian Richards / John Kennedy algorithm
        //link here: https://stackoverflow.com/questions/5124743/algorithm-for-simplifying-decimal-to-fractions/42085412#42085412
        public static Fraction RealToFraction(double value, double accuracy = 0.0001)
        {
            if (accuracy <= 0.0 || accuracy >= 1.0)
            {
                throw new ArgumentOutOfRangeException("accuracy", "Must be > 0 and < 1.");
            }

            int sign = Math.Sign(value);

            if (sign == -1)
            {
                value = Math.Abs(value);
            }

            // Accuracy is the maximum relative error; convert to absolute maxError
            double maxError = sign == 0 ? accuracy : value * accuracy;

            int n = (int)Math.Floor(value);
            value -= n;

            if (value < maxError)
            {
                return new Fraction(sign * n, 0, 1);
            }

            if (1 - maxError < value)
            {
                return new Fraction(sign * (n + 1), 0, 1);
            }

            double z = value;
            int previousDenominator = 0;
            int denominator = 1;
            int numerator;

            do
            {
                z = 1.0 / (z - (int)z);
                int temp = denominator;
                denominator = denominator * (int)z + previousDenominator;
                previousDenominator = temp;
                numerator = Convert.ToInt32(value * denominator);
            }
            while (Math.Abs(value - (double)numerator / denominator) > maxError && z != (int)z);

            return new Fraction(n, numerator * sign, denominator);
        }
        #endregion
        #endregion

        #region Instance Members
        #region Constructors
        public Fraction(long whole = 0, long numer = 0, long denom = 1)
        {
            WholeNumber = Math.Abs(whole);
            Numerator = Math.Abs(numer);

            if (denom == 0)
            {
                throw new ArgumentException("Denominator cannot be 0");
            }
            denominator = Math.Abs(denom);

            bool wneg = (whole < 0);
            bool nneg = (numer < 0);
            bool dneg = (denom < 0);
            isNegative = (wneg == nneg) == dneg;

        }
        #endregion
       
        #region Fields and Properties
        private readonly bool isNegative;
        public long WholeNumber { get; set; }
        public long Numerator { get; set; }
        
        private long denominator;
        public long Denominator
        {
            get { return denominator; }
            set {
                if(value == 0)
                {
                    throw new ArgumentException("Denominator cannot be 0");
                }
                denominator = value;
            }
        }
        #endregion

        #region Instance Functions
        public void MakeImproper()
        {
            Numerator = WholeNumber * Denominator + Numerator;
            WholeNumber = 0;
        }
        public void MakeProper()
        {
            MakeImproper();
            WholeNumber = Numerator / Denominator;
            Numerator = Numerator % Denominator;
        }
        public void Reduce()
        {
            long gcd = GCD(Numerator, Denominator);
            Numerator /= gcd;
            Denominator /= gcd;
        }
        public void Simplify()
        {
            MakeProper();
            Reduce();
        }
        #endregion
       
        #region Object Override Functions
        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(Fraction)) return false;

            return ((Fraction)obj) == this;
        }
        public override int GetHashCode()
        {
            int w1 = (int)WholeNumber, w2 = (int)(WholeNumber >> 32);
            int n1 = (int)Numerator, n2 = (int)(Numerator >> 32);
            int d1 = (int)Denominator, d2 = (int)(Denominator >> 32);

            return (w1 ^ w2) ^ ~(n1 ^ n2) ^ (d1 ^ d2);
        }
        public override string ToString()
        {
            return string.Format(   ((isNegative) ? "-" : "") +
                                    ((WholeNumber != 0) ? "{0}" : "") +
                                    ((WholeNumber != 0 && (Numerator != 0)) ? " " : "") +
                                    ((Numerator != 0)?"{1}/{2}":"") +
                                    ((WholeNumber == 0 && Numerator == 0)?"0":"")
                                , WholeNumber, Numerator, Denominator);
        }
        #endregion
        #endregion

        #region Operators
        #region Operators on other Fractions
        public static Fraction operator +(Fraction a, Fraction b)
        {
            a.MakeImproper();
            b.MakeImproper();

            CommonizeBase(ref a, ref b);

            Fraction frac = new Fraction(0,
                (a.Numerator * (a.isNegative ? -1 : 1)) + (b.Numerator * (b.isNegative ? -1 : 1)),
                a.Denominator);
            frac.Simplify();

            return frac;
        }
        public static Fraction operator -(Fraction a, Fraction b)
        {
            a.MakeImproper();
            b.MakeImproper();
            
            CommonizeBase(ref a, ref b);

            Fraction frac = new Fraction(0,
                (a.Numerator * (a.isNegative ? -1 : 1)) - (b.Numerator * (b.isNegative ? -1 : 1)),
                a.Denominator);
            frac.Simplify();

            return frac;
        }
        public static Fraction operator *(Fraction a, Fraction b)
        {
            a.MakeImproper();
            b.MakeImproper();

            Fraction frac = new Fraction(0,
                a.Numerator * b.Numerator * (a.isNegative ? -1 : 1) * (b.isNegative ? -1 : 1),
                a.Denominator * b.Denominator);
            frac.Simplify();

            return frac;
        }
        public static Fraction operator /(Fraction a, Fraction b)
        {
            a.MakeImproper();
            b.MakeImproper();

            Fraction frac = new Fraction(0,
                a.Numerator * b.Denominator * (a.isNegative ? -1 : 1) * (b.isNegative ? -1 : 1),
                a.Denominator * b.Numerator);
            frac.Simplify();

            return frac;
        }
        public static long operator %(Fraction a, Fraction b)
        {
            long c = b % 1;
            return a % c;
        }
        #endregion

        #region Boolean Operators
        public static bool operator ==(Fraction a, Fraction b)
        {
            a.MakeImproper();
            b.MakeImproper();

            CommonizeBase(ref a, ref b);
            return a.Numerator == b.Numerator && a.isNegative == b.isNegative;
        }
        public static bool operator >(Fraction a, Fraction b)
        {
            a.MakeImproper();
            b.MakeImproper();

            CommonizeBase(ref a, ref b);
            return a.Numerator * (a.isNegative ? -1 : 1) > b.Numerator * (b.isNegative ? -1 : 1);
        }

        public static bool operator !=(Fraction a, Fraction b)
        {
            return !(a == b);
        }
        public static bool operator <(Fraction a, Fraction b)
        {
            return !(a > b) && a != b;
        }
        public static bool operator <=(Fraction a, Fraction b)
        {
            return !(a > b);
        }
        public static bool operator >=(Fraction a, Fraction b)
        {

            return a > b || a == b;
        }
        #endregion

        #region Operators on long
        public static Fraction operator +(Fraction a, long b)
        {
            Fraction frac = new Fraction(a.WholeNumber * (a.isNegative ? -1 : 1) + b, a.Numerator, a.Denominator);
            frac.Simplify();
            return frac;
        }
        public static Fraction operator -(Fraction a, long b)
        {
            Fraction frac = new Fraction(a.WholeNumber * (a.isNegative ? -1 : 1) - b, a.Numerator, a.Denominator);
            frac.Simplify();
            return frac;
        }
        public static Fraction operator *(Fraction a, long b)
        {
            a.MakeImproper();
            Fraction frac = new Fraction(0, a.Numerator * (a.isNegative ? -1 : 1) * b, a.Denominator);
            frac.Simplify();
            return frac;
        }
        public static Fraction operator /(Fraction a, long b)
        {
            a.MakeImproper();
            Fraction frac = new Fraction(0, a.Numerator, a.Denominator * (a.isNegative ? -1 : 1) * b);
            frac.Simplify();
            return frac;
        }
        //mod of a fraction implemented as defined here: https://math.stackexchange.com/questions/174676/solving-simple-congruences-by-hand/174687#174687
        public static long operator %(Fraction a, long b)
        {
            return FracMod(a, b).Numerator;
        }
        private static Fraction FracMod(Fraction a, long b)
        {
            if (b <= 0)
            {
                throw new ArgumentException("Mod zero undefined");
            }

            a.MakeImproper();
            if (b == 1)
            {
                return new Fraction(0, a.Numerator * a.Denominator, 1);
            }
            else if (a.isNegative)
            {
                long num = -a.Numerator;
                while (num < 0)
                {
                    num += a.Denominator;
                }
                return FracMod(new Fraction(0, num, a.Denominator), b);
            }
            else
            {

                long mult = 1;
                while (a.Denominator * mult <= b)
                {
                    mult++;
                }
                
                Fraction r = new Fraction(0, (a.Numerator * mult) % b, (a.Denominator * mult) % b);
                if (r.Denominator != 1)
                {
                    return FracMod(r, b);
                }
                else
                {
                    return r;
                }
            }
        }
        #endregion
        #endregion
    }
}
