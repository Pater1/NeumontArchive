using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExtentionMethods
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestPalindrome0()
        {
            Assert.IsTrue("tacocat".IsPalindrome());
        }
        [TestMethod]
        public void TestPalindrome1()
        {
            Assert.IsTrue("tAcoCat".IsPalindrome());
        }
        [TestMethod]
        public void TestPalindrome2()
        {
            Assert.IsTrue("taco cat".IsPalindrome());
        }
        [TestMethod]
        public void TestNotPalindrome()
        {
            Assert.IsFalse("Dave".IsPalindrome());
        }

        [TestMethod]
        public void TestPow0()
        {
            int actual = 5.ToPower(2);
            int expected = 25;

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void TestPow1()
        {
            int actual = 500.ToPower(0);
            int expected = 1;

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void TestPow2()
        {
            int actual = 500.ToPower(1);
            int expected = 500;

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void TestPow3()
        {
            int actual = 25.ToPower(-2);
            int expected = 5;

            Assert.AreEqual(expected, actual);
        }
    }
}
