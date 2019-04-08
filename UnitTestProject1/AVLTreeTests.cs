using System.Linq;
using AlgoDataStructures;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1 {
    [TestClass]
    public class AVLTreeTests {
        [TestMethod]
        public void AVL_TestCtor() {
            AVLTree<int> tree = new AVLTree<int>(1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 10);
            int expectedCount = 11;
            int actualCount = tree.Count;

            int expectedHeight = 4;
            int actualHeight = tree.Height();

            string expectedString = "4, 2, 8, 1, 3, 6, 9, 0, 5, 7, 10";
            string actualString = tree.ToString();

            Assert.AreEqual(expectedCount, actualCount);
            Assert.AreEqual(expectedHeight, actualHeight);
            Assert.AreEqual(expectedString, actualString);
        }
        [TestMethod]
        private string ArrayToString<T>(IEnumerable<T> er){
            return new string(er.Select(x => x.ToString() + ", ").SelectMany(x => x).Reverse().Skip(2).Reverse().ToArray());
        }
        [TestMethod]
        public void RemoveLeftLeaf() {
            AVLTree<int> avl = new AVLTree<int>();
            avl.Add(24);
            avl.Add(10);
            avl.Add(1337);

            avl.Remove(10);

            string expected = "24, 1337";
            string actual = ArrayToString(avl.ToArray());

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void AddManyValuesWithDoubleRotations() {
            AVLTree<int> avl = new AVLTree<int>();
            avl.Add(24);
            avl.Add(100);
            avl.Add(90);
            avl.Add(7);
            avl.Add(8);
            avl.Add(9);
            avl.Add(12);
            avl.Add(13);
            avl.Add(10);
            avl.Add(11);
            avl.Add(110);
            avl.Add(1400);
            avl.Add(1337);
            avl.Add(1350);
            avl.Add(1500);

            string expected = "12, 9, 100, 8, 10, 24, 1337, 7, 11, 13, 90, 110, 1400, 1350, 1500";
            string actual = ArrayToString(avl.ToArray());

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void RemoveRootFromLargeTree() {
            AVLTree<int> avl = new AVLTree<int>();
            avl.Add(24);
            avl.Add(10);
            avl.Add(1337);
            avl.Add(8);
            avl.Add(12);
            avl.Add(100);
            avl.Add(1400);
            avl.Add(7);
            avl.Add(9);
            avl.Add(11);
            avl.Add(13);
            avl.Add(90);
            avl.Add(110);
            avl.Add(1350);
            avl.Add(1500);

            avl.Remove(24);

            string expected = "90, 10, 1337, 8, 12, 100, 1400, 7, 9, 11, 13, 110, 1350, 1500";
            string actual = ArrayToString(avl.ToArray());

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void RemoveRightBranchRoot() {
            AVLTree<int> avl = new AVLTree<int>();
            avl.Add(24);
            avl.Add(10);
            avl.Add(1337);
            avl.Add(8);
            avl.Add(12);
            avl.Add(100);
            avl.Add(1400);
            avl.Add(7);
            avl.Add(9);
            avl.Add(11);
            avl.Add(13);
            avl.Add(90);
            avl.Add(110);
            avl.Add(1350);
            avl.Add(1500);

            avl.Remove(1337);

            string expected = "24, 10, 1350, 8, 12, 100, 1400, 7, 9, 11, 13, 90, 110, 1500";
            string actual = ArrayToString(avl.ToArray());

            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void RemoveRightLeaf() {
            AVLTree<int> avl = new AVLTree<int>();

            avl.Add(24);
            avl.Add(10);
            avl.Add(1337);

            avl.Remove(1337);

            string expected = "24, 10";
            string actual = ArrayToString(avl.ToArray());

            Assert.AreEqual(expected, actual);
        }

    }
}
