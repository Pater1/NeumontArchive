using System;
using System.Collections.Generic;
using System.Linq;

namespace Barbershop {
    class Program {
        const int CustomersPerDay = 100_000;
        const double CustomersEnteredMeanTime = 60 / 3;
        const double CustomersProcessedMeanTime = 60 / 4;
        static void Main(string[] args) {
            Queue<(int id, double hourEntered, double cutTime)> customersToProcess =
                new Queue<(int id, double hourEntered, double cutTime)>();

            double lastTime = 0.0;
            for(int i = 0; i < CustomersPerDay; i++){
                lastTime += LNTime(CustomersEnteredMeanTime);
                customersToProcess.Enqueue((i, lastTime, LNTime(CustomersProcessedMeanTime)));
            }

            List<(int id, double hourEntered, double cutTime, double waitTime)> processedCustomers =
                new List<(int id, double hourEntered, double cutTime, double waitTime)>(customersToProcess.Count);
            double simTime = 0;
            (int id, double hourEntered, double cutTime) currentCustomer;
            while(customersToProcess.Any()){
                currentCustomer = customersToProcess.Dequeue();
                if(simTime < currentCustomer.hourEntered){
                    simTime = currentCustomer.hourEntered;
                }
                Console.WriteLine($"Customer {currentCustomer.id,6} entered {Math.Round(simTime, 2),6} mins after opening.");

                double waitTime = simTime - currentCustomer.hourEntered;
                Console.WriteLine($"Customer {currentCustomer.id,6} waited {Math.Round(waitTime, 2),6} mins.");
                processedCustomers.Add((currentCustomer.id, 
                                        currentCustomer.hourEntered, 
                                        currentCustomer.cutTime, 
                                        waitTime));
                simTime += currentCustomer.cutTime;

                Console.WriteLine($"Customer {currentCustomer.id,6} haircut took {Math.Round(currentCustomer.cutTime, 2),6} min.");
                Console.WriteLine($"Customer {currentCustomer.id,6} left {Math.Round(simTime, 2),6} mins after opening.");
                Console.WriteLine();
            }

            double meanWaitTime = processedCustomers.Select(x => x.waitTime).Aggregate((x, y) => x + y) / processedCustomers.Count();
            double minWaitTime = processedCustomers.Select(x => x.waitTime).Min();
            double maxWaitTime = processedCustomers.Select(x => x.waitTime).Max();
            Console.WriteLine();
            Console.WriteLine($"Min Wait Time: {Math.Round(minWaitTime, 2)} mins.");
            Console.WriteLine($"Max Wait Time: {Math.Round(maxWaitTime, 2)} mins.");
            Console.WriteLine($"Mean Wait Time: {Math.Round(meanWaitTime, 2)} mins.");

            Console.ReadLine();
        }

        private static Random r = new Random();
        private static double LNTime(double mean){
            double d = r.NextDouble();
            return -Math.Log(1 - d) * mean;
        }
    }
}
