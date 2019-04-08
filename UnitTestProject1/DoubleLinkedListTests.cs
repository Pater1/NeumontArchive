using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AlgoDataStructures;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace LinkedListTester {
    [TestClass]
    public class LinkedListTests {
        private string ArrayToString<T>(T[] er) {
            return new string(er.Select(x => x.ToString() + ", ").SelectMany(x => x).Reverse().Skip(2).Reverse().ToArray());
        }
        [TestMethod]
        public void DLL_EmptyList() {
            DoubleLinkedList<int> list = new DoubleLinkedList<int>();
            int expectedCount = 0;
            int actualCount = list.Count;

            string expectedString = "";
            string actualString = list.ToString();

            Assert.AreEqual(expectedCount, actualCount);
            Assert.AreEqual(expectedString, actualString);
        }

        [TestMethod]
        public void DLL_Methods() {
            DoubleLinkedList<int> list = new DoubleLinkedList<int>();
            list.Add(1);
            list.Insert(1, 0);
            var count = list.Count;
            var value = list.Get(0);
            var removed = list.Remove();
            var last = list.RemoveLast();
            var listString = list.ToString();
            list.Clear();
            var index = list.Search(1);
        }
        [TestMethod]
        public void DLL_ListOfTen_InsertAt0() {
            DoubleLinkedList<int> list = new DoubleLinkedList<int>(24, 3, 6, 0, 6, 17, 100, 2014, 122778, 42);
            int expectedCount = 11;
            string expectedString = "711, 24, 3, 6, 0, 6, 17, 100, 2014, 122778, 42";

            list.Insert(711, 0);
            int actualCount = list.Count;
            string actualString = list.ToString();

            Assert.AreEqual(expectedCount, actualCount);
            Assert.AreEqual(expectedString, actualString);
        }
        [TestMethod]
        public void DLL_ListOfTen_InsertAtEnd1() {
            DoubleLinkedList<int> list = new DoubleLinkedList<int>(24, 3, 6, 0, 6, 17, 100, 2014, 122778, 42);
            int expectedCount = 11;
            string expectedString = "24, 3, 6, 0, 6, 17, 100, 2014, 122778, 711, 42";

            list.Insert(711, list.Count-1);
            int actualCount = list.Count;
            string actualString = list.ToString();

            Assert.AreEqual(expectedCount, actualCount);
            Assert.AreEqual(expectedString, actualString);
        }

        [TestMethod]
        public void DLL_Add() {
            DoubleLinkedList<int> list = new DoubleLinkedList<int>(24, 3, 6, 0, 6, 17, 100, 2014, 122778, 42);
            int expectedCount = 11;
            string expectedString = "24, 3, 6, 0, 6, 17, 100, 2014, 122778, 42, 711";

            list.Add(711);
            int actualCount = list.Count;
            string actualString = list.ToString();

            Assert.AreEqual(expectedCount, actualCount);
            Assert.AreEqual(expectedString, actualString);
        }


        [TestMethod]
        public void DLL_ListOfTen_Remove() {
            DoubleLinkedList<int> list = new DoubleLinkedList<int>(24, 3, 6, 0, 6, 17, 100, 2014, 122778, 42);
            int expectedCount = 9;
            string expectedString = "24, 3, 6, 0, 6, 17, 100, 122778, 42";

            list.Remove(2014);
            int actualCount = list.Count;
            string actualString = list.ToString();

            Assert.AreEqual(expectedCount, actualCount);
            Assert.AreEqual(expectedString, actualString);
        }
        [TestMethod]
        public void DLL_ListOfTen_RemoveAt() {
            DoubleLinkedList<int> list = new DoubleLinkedList<int>(24, 3, 6, 0, 6, 17, 100, 2014, 122778, 42);
            int expectedCount = 9;
            string expectedString = "24, 3, 6, 0, 6, 17, 100, 122778, 42";

            list.RemoveAt(7);
            int actualCount = list.Count;
            string actualString = list.ToString();

            Assert.AreEqual(expectedCount, actualCount);
            Assert.AreEqual(expectedString, actualString);
        }
        [TestMethod]
        public void DLL_ListOfTen_RemoveAll() {
            List<int> vals = new List<int>() { 24, 3, 6, 0, 6, 17, 100, 2014, 122778, 42 };
            DoubleLinkedList<int> list = new DoubleLinkedList<int>(vals);
            int expectedReturn = 0;
            int expectedCount = 9;
            string expectedString;

            for(int i = 0; i < 10; i++) {
                expectedReturn = vals[0];
                vals.RemoveAt(0);
                expectedCount = vals.Count;
                expectedString = ArrayToString(vals.ToArray());

                int actualReturn = list.Remove();
                int actualCount = list.Count;
                string actualString = list.ToString();

                Assert.AreEqual(expectedReturn, actualReturn);
                Assert.AreEqual(expectedCount, actualCount);
                Assert.AreEqual(expectedString, actualString);
            }
        }
    }
}
