using System;
using System.Collections.Generic;
using System.Linq;
using AlgoDataStructures;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1 {
    [TestClass]
    public class LinkedListTests {
        private string ArrayToString<T>(T[] er) {
            return new string(er.Select(x => x.ToString() + ", ").SelectMany(x => x).Reverse().Skip(2).Reverse().ToArray());
        }
        [TestMethod]
        public void SLL_EmptyList() {
            SingleLinkedList<int> list = new SingleLinkedList<int>();
            int expectedCount = 0;
            int actualCount = list.Count;

            string expectedString = "";
            string actualString = list.ToString();

            Assert.AreEqual(expectedCount, actualCount);
            Assert.AreEqual(expectedString, actualString);
        }

        [TestMethod]
        public void SLL_Methods() {
            SingleLinkedList<int> list = new SingleLinkedList<int>();
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
        public void SLL_ListOfTen_InsertAt0() {
            SingleLinkedList<int> list = new SingleLinkedList<int>(24, 3, 6, 0, 6, 17, 100, 2014, 122778, 42);
            int expectedCount = 11;
            string expectedString = "711, 24, 3, 6, 0, 6, 17, 100, 2014, 122778, 42";

            list.Insert(711, 0);
            int actualCount = list.Count;
            string actualString = list.ToString();

            Assert.AreEqual(expectedCount, actualCount);
            Assert.AreEqual(expectedString, actualString);
        }
        [TestMethod]
        public void SLL_ListOfTen_InsertAtEnd1() {
            SingleLinkedList<int> list = new SingleLinkedList<int>(24, 3, 6, 0, 6, 17, 100, 2014, 122778, 42);
            int expectedCount = 11;
            string expectedString = "24, 3, 6, 0, 6, 17, 100, 2014, 122778, 711, 42";

            list.Insert(711, list.Count - 1);
            int actualCount = list.Count;
            string actualString = list.ToString();

            Assert.AreEqual(expectedCount, actualCount);
            Assert.AreEqual(expectedString, actualString);
        }

        [TestMethod]
        public void SLL_Add() {
            SingleLinkedList<int> list = new SingleLinkedList<int>(24, 3, 6, 0, 6, 17, 100, 2014, 122778, 42);
            int expectedCount = 11;
            string expectedString = "24, 3, 6, 0, 6, 17, 100, 2014, 122778, 42, 711";

            list.Add(711);
            int actualCount = list.Count;
            string actualString = list.ToString();

            Assert.AreEqual(expectedCount, actualCount);
            Assert.AreEqual(expectedString, actualString);
        }


        [TestMethod]
        public void SLL_ListOfTen_Remove() {
            SingleLinkedList<int> list = new SingleLinkedList<int>(24, 3, 6, 0, 6, 17, 100, 2014, 122778, 42);
            int expectedCount = 9;
            string expectedString = "24, 3, 6, 0, 6, 17, 100, 122778, 42";

            list.Remove(2014);
            int actualCount = list.Count;
            string actualString = list.ToString();

            Assert.AreEqual(expectedCount, actualCount);
            Assert.AreEqual(expectedString, actualString);
        }
        [TestMethod]
        public void SLL_ListOfTen_RemoveAt() {
            SingleLinkedList<int> list = new SingleLinkedList<int>(24, 3, 6, 0, 6, 17, 100, 2014, 122778, 42);
            int expectedCount = 9;
            string expectedString = "24, 3, 6, 0, 6, 17, 100, 122778, 42";

            list.RemoveAt(7);
            int actualCount = list.Count;
            string actualString = list.ToString();

            Assert.AreEqual(expectedCount, actualCount);
            Assert.AreEqual(expectedString, actualString);
        }
    }
}