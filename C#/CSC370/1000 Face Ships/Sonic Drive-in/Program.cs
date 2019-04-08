using System;
using System.Linq;
using System.Threading;

namespace Sonic_Drive_in {
    class Program {
        private const int customers = 150, parkingSpaces = 50, employees = 15, POSes = 1;

        static void Main(string[] args) {
            Semaphore parkingLock = new Semaphore(0, parkingSpaces);
            Semaphore employeesLock = new Semaphore(0, employees);
            Semaphore POSLock = new Semaphore(0, POSes);
            ThreadPool.SetMaxThreads(75, 75);
            ThreadPool.SetMinThreads(75, 75);

            bool[] finishLock = new bool[customers];
            for(int i = 0; i < customers; i++){
                int index = i;
                finishLock[i] = false;
                ThreadPool.QueueUserWorkItem((x) => {
                    Console.WriteLine($"Customer {index}: waiting for parking space");

                    //get parking spot
                    parkingLock.WaitOne();
                    Console.WriteLine($"Customer {index}: received parking space");

                    //figure out order
                    Thread.Sleep(200);

                    Console.WriteLine($"Customer {index}: waiting for employee to take order");
                    //get employee to take order
                    employeesLock.WaitOne();
                    Thread.Sleep(100);
                    {
                        Console.WriteLine($"Customer {index}: waiting for POS");
                        POSLock.WaitOne();
                        Thread.Sleep(50);
                        POSLock.Release();
                        Console.WriteLine($"Customer {index}: payment processed");
                    }
                    employeesLock.Release();
                    Console.WriteLine($"Customer {index}: order taken; cooking now");


                    //wait for food to cook
                    Thread.Sleep(250);

                    Console.WriteLine($"Customer {index}: food cooked; waiting for employee to deliver");
                    //get employee to take food to car
                    employeesLock.WaitOne();
                    Thread.Sleep(50);
                    employeesLock.Release();
                    Console.WriteLine($"Customer {index}: food delivered; eating...");

                    //eat food
                    Thread.Sleep(500);

                    //leave parking spot
                    parkingLock.Release();
                    Console.WriteLine($"Customer {index}: left parking spot");

                    finishLock[index] = true;
                });
            }

            Console.WriteLine("Sonic opening!");
            parkingLock.Release(parkingSpaces);
            employeesLock.Release(employees);
            POSLock.Release(POSes);

            double delay = 2;
            while(!finishLock.Aggregate((x,y) => x&&y)){
                Thread.Sleep(TimeSpan.FromMilliseconds(delay = Math.Pow(delay, 1.01)));
            }
            Console.WriteLine("Sonic closing...");

            parkingLock.Close();
            employeesLock.Close();
            POSLock.Close();
            parkingLock.Dispose();
            employeesLock.Dispose();
            POSLock.Dispose();

            Console.ReadLine();
        }
    }
}
