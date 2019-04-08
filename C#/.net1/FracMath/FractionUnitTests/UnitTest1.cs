using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OverloadingOperators;

namespace FractionUnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestEquals()
        {
            Fraction a = new Fraction(-3, 1, 4);
            Fraction b = new Fraction(-3, 1, 4);

            Assert.IsTrue(a == b);
        }
        [TestMethod]
        public void TestNotEquals()
        {
            Fraction a = new Fraction(-3, 1, 4);
            Fraction b = new Fraction(-3, 2, 4);

            Assert.IsTrue(a != b);
        }

        [TestMethod]
        public void TestFracSubtractFrac()
        {
            Fraction a = new Fraction(-3, 1, 4);
            Fraction b = new Fraction(2, 1, 2);
            Fraction actual = a - b;

            Fraction expected = new Fraction(-5, 3, 4);

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void TestFracAddFrac()
        {
            Fraction a = new Fraction(-3, 1, 4);
            Fraction b = new Fraction(2, 1, 2);
            Fraction actual = a + b;

            Fraction expected = new Fraction(0, -3, 4);

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void TestFracMultFrac()
        {
            Fraction a = new Fraction(-4, 1, 2);
            Fraction b = new Fraction(2, 1, 2);
            Fraction actual = a * b;

            Fraction expected = new Fraction(-11, 1, 4);

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void TestFracDivFrac()
        {
            Fraction a = new Fraction(-4, 1, 2);
            Fraction b = new Fraction(2, 1, 2);
            Fraction actual = a / b;

            Fraction expected = new Fraction(-1, 4, 5);

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void TestFracModFrac()
        {
            Fraction a = new Fraction(0, 7, 9);
            Fraction b = new Fraction(0, 13, 1);
            long actual = a % b;

            long expected = 8;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestFracModLong()
        {
            Fraction a = new Fraction(0, 7, 9);
            long b = 13;
            long actual = a % b;

            long expected = 8;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestMakeProper()
        {
            Fraction actual = (new Fraction(0, 12, 3));
            actual.MakeProper();

            Fraction expected = new Fraction(4, 0, 3);

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void TestMakeImproper()
        {
            Fraction actual = new Fraction(4, 0, 3);
            actual.MakeImproper();

            Fraction expected = new Fraction(0, 12, 3);

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void TestReduce()
        {
            Fraction actual = new Fraction(0, 12, 16);
            actual.Reduce();

            Fraction expected = new Fraction(0, 3, 4);

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void TestSimplify()
        {
            Fraction actual = new Fraction(0, 16, 12);
            actual.Simplify();

            Fraction expected = new Fraction(1, 1, 3);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestGCF()
        {
            long a = 21;
            long b = 15;
            long actual = Fraction.GCD(a, b);

            long expected = 3;

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void TestLCM()
        {
            long a = 12;
            long b = 3;
            long actual = Fraction.LCM(a, b);

            long expected = 12;

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void TestDecimalToFraction()
        {
            double a = 0.33333333333;
            Fraction actual = Fraction.RealToFraction(a);

            Fraction expected = new Fraction(0, 1, 3);
            Assert.AreEqual(expected, actual);

            //-------------------------------//

            a = -0.33333333333;
            actual = Fraction.RealToFraction(a);

            expected = new Fraction(0, -1, 3);

            Assert.AreEqual(expected, actual);

            //-------------------------------//

            a = -5;
            actual = Fraction.RealToFraction(a);

            expected = new Fraction(-5, 0, 1);

            Assert.AreEqual(expected, actual);
        }
    }
}
