using System;
using System.Threading;

namespace Seeking_Therapy {
    class Program {
        private static readonly TimeSpan dayLength = TimeSpan.FromMinutes(2);
        private static readonly TimeSpan minSessionLength = TimeSpan.FromSeconds(5), maxSessionLength = TimeSpan.FromSeconds(15);
        private static readonly TimeSpan minPatientAddDelay = TimeSpan.FromSeconds(5), maxPatientAddDelay = TimeSpan.FromSeconds(15);
        private static readonly int minPatientsAdd = 1, maxPatientsAdd = 3;

        static void Main(string[] args) {
            Random rand = new Random();
            Object therapist = new object();
            bool stall = true;
            int threadIndex = 0;
            Thread addPatients = new Thread(() => {
                DateTime open = DateTime.Now, close;
                do {
                    close = DateTime.Now;
                    for(int i = 0; i < rand.Next(minPatientsAdd, maxPatientsAdd + 1); i++) {
                        Thread t = new Thread(() => {
                            int id = (threadIndex += 1);
                            DateTime start = DateTime.Now, got;
                            lock(therapist) {
                                got = DateTime.Now;
                                if(((close - open) > dayLength)) {
                                    TimeSpan _wait = got - start;
                                    Console.WriteLine($"Patient {id} never saw the therapist after waiting {_wait.ToString()}.");

                                    return;
                                }
                                Thread.Sleep(TimeSpan.FromTicks(rand.Next((int)minSessionLength.Ticks, (int)maxSessionLength.Ticks)));
                            }
                            TimeSpan duration = DateTime.Now - got;
                            TimeSpan wait = got - start;
                            Console.WriteLine($"Patient {id} saw the therapist after {wait.ToString()}, with a session lasting {duration.ToString()}");
                        });
                        t.Start();
                    }
                    Thread.Sleep(TimeSpan.FromTicks(rand.Next((int)minPatientAddDelay.Ticks, (int)maxPatientAddDelay.Ticks)));
                } while((close - open) < dayLength);
                stall = false;
            });
            addPatients.Start();

            int stallDuration = 2;
            while(stall) {
                Thread.Sleep(stallDuration = stallDuration * stallDuration);
            }

            Console.ReadLine();
        }
    }
}
