using System;
using System.Linq;
using AlgoDataStructures;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1 {
    [TestClass]
    public class MaxHeapPriorityQueueTests {
        private static MaxHeapPriorityQueue BaseQueue() {
            MaxHeapPriorityQueue queue = new MaxHeapPriorityQueue();

            queue.Enqueue(1, 1);
            queue.Enqueue(2, 2);
            queue.Enqueue(3, 3);
            queue.Enqueue(4, 4);
            queue.Enqueue(5, 5);
            queue.Enqueue(6, 6);
            queue.Enqueue(7, 7);
            queue.Enqueue(8, 8);
            queue.Enqueue(9, 9);
            queue.Enqueue(10, 10);
            queue.Enqueue(11, 11);
            queue.Enqueue(12, 12);
            queue.Enqueue(13, 13);
            queue.Enqueue(14, 14);
            queue.Enqueue(15, 15);
            queue.Enqueue(16, 16);
            queue.Enqueue(17, 17);
            queue.Enqueue(18, 18);
            queue.Enqueue(19, 19);
            queue.Enqueue(20, 20);

            return queue;
        }
        [TestMethod]
        public void TestEnqueue() {
            MaxHeapPriorityQueue queue = BaseQueue();

            string expectedStr = "20:20,19:19,14:14,17:17,18:18,11:11,13:13,10:10,16:16,9:9,8:8,2:2,6:6,5:5,12:12,1:1,7:7,4:4,15:15,3:3";
            string actualStr = queue.ToString();
            Assert.AreEqual(expectedStr, actualStr, "To String Fail");

            int[] expectedArr = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };
            int[] actualArr = queue.ToSortedArray().Select(x => x.Value).ToArray();
            Assert.AreEqual(expectedArr.Length, actualArr.Length, "Array Sort Fail -Length");
            for(int i = 0; i < expectedArr.Length; i++) {
                Assert.AreEqual(expectedArr[i], actualArr[i], $"Array Sort Fail -{i}");
            }
        }
        [TestMethod]
        public void TestPeek() {
            MaxHeapPriorityQueue queue = BaseQueue();

            int expectedDeq = 20;
            int actualDeq = queue.Peek().Value;
            Assert.AreEqual(expectedDeq, actualDeq, "Peek Fail");

            string expectedStr = "20:20,19:19,14:14,17:17,18:18,11:11,13:13,10:10,16:16,9:9,8:8,2:2,6:6,5:5,12:12,1:1,7:7,4:4,15:15,3:3";
            string actualStr = queue.ToString();
            Assert.AreEqual(expectedStr, actualStr, "To String Fail");

            int[] expectedArr = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };
            int[] actualArr = queue.ToSortedArray().Select(x => x.Value).ToArray();
            Assert.AreEqual(expectedArr.Length, actualArr.Length, "Array Sort Fail -Length");
            for(int i = 0; i < expectedArr.Length; i++) {
                Assert.AreEqual(expectedArr[i], actualArr[i], $"Array Sort Fail -{i}");
            }
        }
        [TestMethod]
        public void TestDequeue() {
            MaxHeapPriorityQueue queue = BaseQueue();

            int expectedDeq = 20;
            int actualDeq = queue.Dequeue().Value;
            Assert.AreEqual(expectedDeq, actualDeq, "Dequeue Fail");

            string expectedStr = "19:19,18:18,14:14,17:17,9:9,11:11,13:13,10:10,16:16,3:3,8:8,2:2,6:6,5:5,12:12,1:1,7:7,4:4,15:15";
            string actualStr = queue.ToString();
            Assert.AreEqual(expectedStr, actualStr, "ToString Fail");

            int[] expectedArr = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };
            int[] actualArr = queue.ToSortedArray().Select(x => x.Value).ToArray();
            Assert.AreEqual(expectedArr.Length, actualArr.Length, "Array Sort Fail -Length");
            for(int i = 0; i < expectedArr.Length; i++) {
                Assert.AreEqual(expectedArr[i], actualArr[i], $"Array Sort Fail -{i}");
            }
        }
    }
}
