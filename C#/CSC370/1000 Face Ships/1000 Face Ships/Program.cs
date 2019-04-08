using System;
using System.Collections.Generic;
using System.Threading;

namespace _1000_Face_Ships {
    class Program {
        static void Main(string[] args) {
            Random rand = new Random();
            for(int i = 0; i < 1000; i++){
                new Thread(() => {
                    int q = Thread.CurrentThread.ManagedThreadId;
                    string n = Thread.CurrentThread.Name;

                    Thread.Sleep(rand.Next(5, 800));
                    Console.WriteLine($"Ship #{q} \"{n}\" is leaving port!");
                }).Start();
            }
        }
    }
}
