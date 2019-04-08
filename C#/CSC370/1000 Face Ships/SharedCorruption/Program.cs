using System;
using System.Threading;
using System.Linq;

namespace SharedCorruption {
    class Program {
        private static volatile int sharedData;
        private static int threadCount = 2, testIterations = 100;
        static void Main(string[] args) {
            for(int a = 0; a < testIterations; a++) {
                sharedData = 0;
                Console.Write($"Test[{a}]: ");
                bool[] bs = new bool[threadCount];
                for(int i = 0; i < threadCount; i++) {
                    int index = i;
                    bs[index] = false;
                    new Thread(() => {
                        int x = sharedData;
                        Thread.Sleep(10);
                        x++;
                        sharedData = x;
                        bs[index] = true;
                    }).Start();
                }

                int del = 2;
                while(!bs.Aggregate((x, y) => x && y)) {
                    Thread.Sleep(del = del * del);
                }

                Console.WriteLine(sharedData);
                Console.WriteLine();
            }
        }
    }
}
