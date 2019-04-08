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
                bool[] requests = new bool[threadCount];
                bool[] bs = new bool[threadCount];
                long taskID = 0;
                for(int i = 0; i < threadCount; i++) {
                    int index = i;
                    int other = (i + 1) % 2;
                    bs[index] = false;
                    new Thread(() => {
                        requests[index] = true;
                        while(requests[other]){
                            if(taskID != index) {
                                int delay = 2;
                                requests[index] = false;
                                while(taskID != index) {
                                    Thread.Sleep(delay = delay * delay);
                                }
                                requests[index] = true;
                            }
                        }

                        int x = sharedData;
                        Thread.Sleep(10);
                        x++;
                        sharedData = x;
                        requests[index] = true;

                        taskID = other;
                        requests[index] = false;
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
