using System;
using System.Linq;
using AlgoDataStructures;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1 {
    [TestClass]
    public class BSTTests {
        private string ArrayToString<T>(T[] er) {
            return new string(er.Select(x => x.ToString() + ", ").SelectMany(x => x).Reverse().Skip(2).Reverse().ToArray());
        }
        [TestMethod]
        public void AddManyValuesWithDuplicate() {
            BinarySearchTree<int> bst = new BinarySearchTree<int>();
            string expectedInOrder = "7, 8, 9, 10, 11, 12, 13, 24, 24, 90, 100, 110, 1337, 1350, 1400, 1500";
            int[] expectedArray = new int[] { 7, 8, 9, 10, 11, 12, 13, 24, 24, 90, 100, 110, 1337, 1350, 1400, 1500 };

            bst.Add(24);
            bst.Add(10);
            bst.Add(1337);
            bst.Add(8);
            bst.Add(12);
            bst.Add(100);
            bst.Add(1400);
            bst.Add(7);
            bst.Add(9);
            bst.Add(11);
            bst.Add(13);
            bst.Add(90);
            bst.Add(110);
            bst.Add(1350);
            bst.Add(1500);
            bst.Add(24);

            int[] actualArray = bst.ToArray();

            Assert.AreEqual(16, bst.Count);
            Assert.AreEqual(ArrayToString(expectedArray), ArrayToString(actualArray));
            Assert.AreEqual(expectedInOrder, bst.InOrder());
        }
        [TestMethod]
        public void AddManyValuesToEmptyTree() {
            BinarySearchTree<int> bst = new BinarySearchTree<int>();
            string expectedInOrder = "7, 8, 9, 10, 11, 12, 13, 24, 90, 100, 110, 1337, 1350, 1400, 1500";
            int[] expectedArray = new int[] { 7, 8, 9, 10, 11, 12, 13, 24, 90, 100, 110, 1337, 1350, 1400, 1500 };

            bst.Add(24);
            bst.Add(10);
            bst.Add(1337);
            bst.Add(8);
            bst.Add(12);
            bst.Add(100);
            bst.Add(1400);
            bst.Add(7);
            bst.Add(9);
            bst.Add(11);
            bst.Add(13);
            bst.Add(90);
            bst.Add(110);
            bst.Add(1350);
            bst.Add(1500);

            int[] actualArray = bst.ToArray();

            Assert.AreEqual(15, bst.Count);
            Assert.AreEqual(ArrayToString(expectedArray), ArrayToString(actualArray));
            Assert.AreEqual(expectedInOrder, bst.InOrder());
        }
        [TestMethod]
        public void HeightOfWorstCaseRightTree() {
            BinarySearchTree<int> bst = new BinarySearchTree<int>();
            int expected = 15;

            bst.Add(1500);
            bst.Add(1400);
            bst.Add(1350);
            bst.Add(1337);
            bst.Add(110);
            bst.Add(100);
            bst.Add(90);
            bst.Add(24);
            bst.Add(13);
            bst.Add(12);
            bst.Add(11);
            bst.Add(10);
            bst.Add(9);
            bst.Add(8);
            bst.Add(7);

            int actual = bst.Height();

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void RemoveRightBranchRoot() {
            BinarySearchTree<int> bst = new BinarySearchTree<int>();
            int[] expectedArray = new int[] { 7, 8, 9, 10, 11, 12, 13, 24, 90, 100, 110, 1350, 1400, 1500 };
            int expectedCount = expectedArray.Length;

            bst.Add(24);
            bst.Add(10);
            bst.Add(1337);
            bst.Add(8);
            bst.Add(12);
            bst.Add(100);
            bst.Add(1400);
            bst.Add(7);
            bst.Add(9);
            bst.Add(11);
            bst.Add(13);
            bst.Add(90);
            bst.Add(110);
            bst.Add(1350);
            bst.Add(1500);

            bst.Remove(1337);

            int actualCount = bst.Count;
            int[] actualArray = bst.ToArray();

            Assert.AreEqual(expectedCount, actualCount);
            Assert.AreEqual(ArrayToString(expectedArray), ArrayToString(bst.ToArray()));
        }
    }
}
