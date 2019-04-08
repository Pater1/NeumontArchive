using System;
using System.Collections.Generic;
using System.Linq;

namespace Computer_Factory {
    class Program {
        const double DayLength = 8 * 60, DaysPerWeek = 5, WeeksSimulated = 1;

        const double Wage = 15;

        const double MeanMBInstallTime = 10,
                        MBInstallTimeSnrdrdv = 0.0;

        const double MeanProcessorInstallTime = 2,
                        ProcessorInstallTimeSnrdrdv = 0.0;

        const double MeanHDDInstallTime = 1,
                        HDDInstallTimeSnrdrdv = 0.0;

        const double MeanRAMInstallTime = 0.75,
                        RAMInstallTimeSnrdrdv = 0.0;

        private static IServiceProvider[] DefaultServers = new IServiceProvider[]{
            new MotherboardInstaller(0, () => LNTime(MeanMBInstallTime)),
            new MotherboardInstaller(1, () => LNTime(MeanMBInstallTime)),
            new MotherboardInstaller(2, () => LNTime(MeanMBInstallTime)),
            new MotherboardInstaller(3, () => LNTime(MeanMBInstallTime)),
            new MotherboardInstaller(4, () => LNTime(MeanMBInstallTime)),

            new ProcessorInstaller(0, () => LNTime(MeanProcessorInstallTime)),
            new ProcessorInstaller(1, () => LNTime(MeanProcessorInstallTime)),

            new HDDInstaller(0, () => LNTime(MeanHDDInstallTime)),

            new RAMInstaller(0, () => LNTime(MeanRAMInstallTime)),
            new RAMInstaller(1, () => LNTime(MeanRAMInstallTime)),
        };
        private static IServiceProvider[] AdjustedServers = new IServiceProvider[]{
            new MotherboardInstaller(0, () => LNTime(MeanMBInstallTime)),
            new MotherboardInstaller(1, () => LNTime(MeanMBInstallTime)),
            new MotherboardInstaller(2, () => LNTime(MeanMBInstallTime)),
            new MotherboardInstaller(3, () => LNTime(MeanMBInstallTime)),
            new MotherboardInstaller(4, () => LNTime(MeanMBInstallTime)),

            new ProcessorInstaller(0, () => LNTime(MeanProcessorInstallTime)),
            new ProcessorInstaller(1, () => LNTime(MeanProcessorInstallTime)),
            new ProcessorInstaller(2, () => LNTime(MeanProcessorInstallTime)),//new

            new HDDInstaller(0, () => LNTime(MeanHDDInstallTime)),
            new HDDInstaller(1, () => LNTime(MeanHDDInstallTime)),//new
            new HDDInstaller(2, () => LNTime(MeanHDDInstallTime)),//new
            
            new RAMInstaller(0, () => LNTime(MeanRAMInstallTime)),
            new RAMInstaller(1, () => LNTime(MeanRAMInstallTime)),
            //new RAMInstaller(2, () => LNTime(MeanRAMInstallTime)),//Reasigned (From MB)
        };

        private static List<Computer> ComputersInProcess = new List<Computer>(), AllComputers = new List<Computer>();
        static void Main(string[] args) {
            List<IServiceProvider> servers = AdjustedServers.ToList();
            for(int a = 0; a < WeeksSimulated; a++) {
                Console.WriteLine($"Week {a} Start\n");
                for(int b = 0; b < DaysPerWeek; b++) {
                    Console.WriteLine($"Day {b} Start\n");
                    double simTime = 0;
                    while(simTime < DayLength) {
                        var v = servers.OrderBy(x => x.BusyUntil).ThenBy(x => servers.IndexOf(x)).Where(x => x.Processable.Any()).First();
                        v.Serve(ref simTime);
                    }
                    foreach(IServiceProvider sp in servers) {
                        sp.BusyUntil = 0.0;
                    }
                    for(int i = ComputersInProcess.Count - 1; i >= 0; i--) {
                        Computer trashed = ComputersInProcess[i];
                        Console.WriteLine($"Computer {trashed.id} {(trashed.Finished ? "finished by" : "discarded at")} end of day");
                        if(trashed.MBInstalled) {
                            Console.WriteLine(@"    Motherboard installed, -$100");
                        }
                        if(trashed.ProcessorInstalled) {
                            Console.WriteLine(@"    Processor installed, -$75");
                        }
                        if(trashed.HDDInstalled) {
                            Console.WriteLine(@"    HDD installed, -$50");
                        }
                        if(trashed.RAMInstalled) {
                            Console.WriteLine(@"    RAM installed, -$25");
                        }
                        if(trashed.Finished) {
                            Console.WriteLine(@"    Computer Complete, +$500");
                        }
                        Console.WriteLine();
                        AllComputers.Add(trashed);
                    }
                    ComputersInProcess.Clear();
                    Console.WriteLine($"Day {b} End");
                }
                Console.WriteLine($"Week {a} End\n");
            }

            Console.WriteLine();

            double laborCosts = (DayLength / 60) * DaysPerWeek * WeeksSimulated * servers.Count * Wage;
            double partsCost = AllComputers.Where(x => x.MBInstalled).Count() * 100 +
                                AllComputers.Where(x => x.ProcessorInstalled).Count() * 75 +
                                AllComputers.Where(x => x.HDDInstalled).Count() * 50 +
                                AllComputers.Where(x => x.RAMInstalled).Count() * 25 +
                                laborCosts;
            double totalRevenue = AllComputers.Where(x => x.Finished).Count() * 500;

            Console.WriteLine($"Labor costs: ${Math.Round(laborCosts, 2)}");
            Console.WriteLine($"Parts costs: ${Math.Round(partsCost, 2)}");
            Console.WriteLine($"Total operating costs: ${Math.Round(partsCost + laborCosts, 2)}");
            Console.WriteLine();
            Console.WriteLine($"Total revenue: ${Math.Round(totalRevenue, 2)}");
            Console.WriteLine($"Gross profit: ${Math.Round(totalRevenue - partsCost - laborCosts, 2)}");

            int MBWasted = AllComputers.Where(x => x.MBInstalled && !x.Finished).Count();
            int ProcessorsWasted = AllComputers.Where(x => x.ProcessorInstalled && !x.Finished).Count();
            int HDDWasted = AllComputers.Where(x => x.HDDInstalled && !x.Finished).Count();
            int RAMWasted = AllComputers.Where(x => x.RAMInstalled && !x.Finished).Count();
            int ComputersFinished = AllComputers.Where(x => x.Finished).Count();

            Console.WriteLine();
            Console.WriteLine($"Motherboards wasted: {MBWasted}, leakage: ${Math.Round((double)MBWasted * 100, 2)}");
            Console.WriteLine($"Processors wasted: {ProcessorsWasted}, leakage: ${Math.Round((double)ProcessorsWasted * 75, 2)}");
            Console.WriteLine($"HDD wasted: {HDDWasted}, leakage: ${Math.Round((double)HDDWasted * 50, 2)}");
            Console.WriteLine($"RAM wasted: {RAMWasted}, leakage: ${Math.Round((double)RAMWasted * 25, 2)}");
            Console.WriteLine();
            Console.WriteLine($"Computers finished: {ComputersFinished}");

            double MBIdleTime = servers.Where(x => x is MotherboardInstaller).Select(x => x.IdleTime).Sum() / 60;
            double ProcessorIdleTime = servers.Where(x => x is ProcessorInstaller).Select(x => x.IdleTime).Sum() / 60;
            double HDDIdleTime = servers.Where(x => x is HDDInstaller).Select(x => x.IdleTime).Sum() / 60;
            double RAMIdleTime = servers.Where(x => x is RAMInstaller).Select(x => x.IdleTime).Sum() / 60;
            double totalIdleTime = MBIdleTime + ProcessorIdleTime + HDDIdleTime + RAMIdleTime;

            Console.WriteLine();
            Console.WriteLine($"Motherboard installers idle time: {Math.Round(MBIdleTime, 2)}, leakage: ${Math.Round(MBIdleTime * Wage, 2)}");
            Console.WriteLine($"Processor installers idle time: {Math.Round(ProcessorIdleTime, 2)}, leakage: ${Math.Round(ProcessorIdleTime * Wage, 2)}");
            Console.WriteLine($"HDD installers idle time: {Math.Round(HDDIdleTime, 2)}, leakage: ${Math.Round(HDDIdleTime * Wage, 2)}");
            Console.WriteLine($"RAM installers idle time: {Math.Round(RAMIdleTime, 2)}, leakage: ${Math.Round(RAMIdleTime * Wage, 2)}");
            Console.WriteLine();
            Console.WriteLine($"Total idle time: {Math.Round(totalIdleTime, 2)}, leakage: ${Math.Round(totalIdleTime * Wage, 2)}");

            Console.ReadLine();
        }

        private static Random rand = new Random();
        private static double LNTime(double mean) {
            double d = rand.NextDouble();
            return -Math.Log(1 - d) * mean;
        }
        private static double NormalTime(double mean, double standardDeviation) {
            double u1 = 1.0 - rand.NextDouble(); //uniform(0,1] random doubles
            double u2 = 1.0 - rand.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                         Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            double randNormal =
                         mean + standardDeviation * randStdNormal; //random normal(mean,stdDev^2)

            return randNormal;
        }

        private class Computer {
            public int id;
            public double? hourMBInstalled,
                            hourProcessorInstallStart, hourProcessorInstallFinished,
                            hourHDDInstallStart, hourHDDInstallFinished,
                            hourRAMInstallStart, hourRAMInstallFinished;

            public bool MBInstalled {
                get {
                    return hourMBInstalled.HasValue && hourMBInstalled.Value < DayLength;
                }
            }
            public bool ProcessorInstalled {
                get {
                    return hourProcessorInstallStart.HasValue && hourProcessorInstallStart.Value < DayLength;
                }
            }
            public bool HDDInstalled {
                get {
                    return hourHDDInstallStart.HasValue && hourHDDInstallStart.Value < DayLength;
                }
            }
            public bool RAMInstalled {
                get {
                    return hourRAMInstallStart.HasValue && hourRAMInstallStart.Value < DayLength;
                }
            }
            public bool Finished {
                get {
                    return hourRAMInstallFinished.HasValue && hourRAMInstallFinished.Value < DayLength;
                }
            }

            public Computer(int id, double hourMBInstalled) {
                this.id = id;

                this.hourMBInstalled = hourMBInstalled;
                hourProcessorInstallStart = null; hourProcessorInstallFinished = null;
                hourHDDInstallStart = null; hourHDDInstallFinished = null;
                hourRAMInstallStart = null; hourRAMInstallFinished = null;
            }
        }

        private interface IServiceProvider {
            int ID { get; }
            double BusyUntil { get; set; }
            double IdleTime { get; set; }
            Func<double> TimeToServiceGenerator { get; }
            double ProjectedWaitTime(double simTime);
            void Serve(ref double simTime);
            IEnumerable<Computer> Processable { get; }
        }

        private class MotherboardInstaller: IServiceProvider {
            public int ID { get; }
            public Func<double> TimeToServiceGenerator { get; }

            public double BusyUntil { get; set; } = 0.0;

            public double IdleTime { get; set; }

            public MotherboardInstaller(int id, Func<double> timeToServiceGenerator) {
                ID = id;
                TimeToServiceGenerator = timeToServiceGenerator;
            }

            public IEnumerable<Computer> Processable => new Computer[] { null, null };

            public double ProjectedWaitTime(double simTime) {
                double ret = BusyUntil - simTime;
                return ret > 0 ? ret : 0;
            }
            public void Serve(ref double simTime) {
                if(BusyUntil > simTime) {
                    simTime = BusyUntil;
                }
                if(simTime > BusyUntil) {
                    IdleTime += simTime - BusyUntil;
                }

                BusyUntil = simTime + TimeToServiceGenerator();

                Computer processing = new Computer(ComputersInProcess.Count, BusyUntil);
                ComputersInProcess.Add(processing);

                Console.WriteLine($"Installing Motherboard in computer {processing.id} by installer {this.ID}; from {Math.Round(simTime, 2)} to {Math.Round(BusyUntil, 2)}");
            }
        }

        private class ProcessorInstaller: IServiceProvider {
            public int ID { get; }
            public Func<double> TimeToServiceGenerator { get; }

            public double BusyUntil { get; set; } = 0.0;

            public ProcessorInstaller(int id, Func<double> timeToServiceGenerator) {
                ID = id;
                TimeToServiceGenerator = timeToServiceGenerator;
            }

            public IEnumerable<Computer> Processable => ComputersInProcess.Where(x => x.hourMBInstalled.HasValue &&
                                                                              !x.hourProcessorInstallStart.HasValue)
                                                                    .OrderBy(x =>
                                                                                x.hourMBInstalled.Value);

            public double IdleTime { get; set; }

            public double ProjectedWaitTime(double simTime) {
                if(Processable.Any()) {
                    double ret = BusyUntil - Processable.Select(x => x.hourMBInstalled.Value).First();
                    return ret > 0 ? ret : 0;
                } else {
                    return int.MaxValue;
                }
            }

            public void Serve(ref double simTime) {
                Computer processing = Processable.FirstOrDefault();

                if(processing == null) return;

                if(BusyUntil > simTime) {
                    simTime = BusyUntil;
                }
                if(processing.hourMBInstalled.Value > simTime) {
                    simTime = processing.hourMBInstalled.Value;
                }
                if(simTime > BusyUntil) {
                    IdleTime += simTime - BusyUntil;
                }

                BusyUntil = simTime + TimeToServiceGenerator();

                processing.hourProcessorInstallStart = simTime;
                processing.hourProcessorInstallFinished = BusyUntil;

                Console.WriteLine($"Installing Processor in computer {processing.id} by installer {this.ID}; from {Math.Round(simTime, 2)} to {Math.Round(BusyUntil, 2)} after {Math.Round(processing.hourProcessorInstallStart.Value - processing.hourMBInstalled.Value, 2)}min wait.");
            }
        }

        private class HDDInstaller: IServiceProvider {
            public int ID { get; }
            public Func<double> TimeToServiceGenerator { get; }


            public double BusyUntil { get; set; } = 0.0;

            public HDDInstaller(int id, Func<double> timeToServiceGenerator) {
                ID = id;
                TimeToServiceGenerator = timeToServiceGenerator;
            }

            public IEnumerable<Computer> Processable => ComputersInProcess.Where(x => x.hourProcessorInstallFinished.HasValue &&
                                                                                !x.hourHDDInstallStart.HasValue)
                                                                    .OrderBy(x =>
                                                                                x.hourProcessorInstallFinished.Value);

            public double IdleTime { get; set; }

            public double ProjectedWaitTime(double simTime) {
                if(Processable.Any()) {
                    double ret = BusyUntil - Processable.Select(x => x.hourProcessorInstallFinished.Value).First();
                    return ret > 0 ? ret : 0;
                } else {
                    return int.MaxValue;
                }
            }

            public void Serve(ref double simTime) {
                Computer processing = Processable.FirstOrDefault();

                if(processing == null) return;

                if(BusyUntil > simTime) {
                    simTime = BusyUntil;
                }
                if(processing.hourProcessorInstallFinished.Value > simTime) {
                    simTime = processing.hourProcessorInstallFinished.Value;
                }
                if(simTime > BusyUntil) {
                    IdleTime += simTime - BusyUntil;
                }

                BusyUntil = simTime + TimeToServiceGenerator();

                processing.hourHDDInstallStart = simTime;
                processing.hourHDDInstallFinished = BusyUntil;

                Console.WriteLine($"Installing HDD in computer {processing.id} by installer {this.ID}; from {Math.Round(simTime, 2)} to {Math.Round(BusyUntil, 2)} after {Math.Round(processing.hourHDDInstallStart.Value - processing.hourProcessorInstallFinished.Value, 2)}min wait.");
            }
        }

        private class RAMInstaller: IServiceProvider {
            public int ID { get; }
            public Func<double> TimeToServiceGenerator { get; }

            public double BusyUntil { get; set; } = 0.0;

            public RAMInstaller(int id, Func<double> timeToServiceGenerator) {
                ID = id;
                TimeToServiceGenerator = timeToServiceGenerator;
            }


            public IEnumerable<Computer> Processable => ComputersInProcess.Where(x => x.hourHDDInstallFinished.HasValue &&
                                                                                !x.hourRAMInstallStart.HasValue)
                                                                    .OrderBy(x =>
                                                                                x.hourHDDInstallFinished.Value);

            public double IdleTime { get; set; }

            public double ProjectedWaitTime(double simTime) {
                if(Processable.Any()) {
                    double ret = BusyUntil - Processable.Select(x => x.hourHDDInstallFinished.Value).First();
                    return ret > 0 ? ret : 0;
                } else {
                    return int.MaxValue;
                }
            }

            public void Serve(ref double simTime) {
                Computer processing = Processable.FirstOrDefault();

                if(processing == null) return;

                if(BusyUntil > simTime) {
                    simTime = BusyUntil;
                }
                if(processing.hourHDDInstallFinished.Value > simTime) {
                    simTime = processing.hourHDDInstallFinished.Value;
                }
                if(simTime > BusyUntil) {
                    IdleTime += simTime - BusyUntil;
                }

                BusyUntil = simTime + TimeToServiceGenerator();

                processing.hourRAMInstallStart = simTime;
                processing.hourRAMInstallFinished = BusyUntil;

                Console.WriteLine($"Installing RAM in computer {processing.id} by installer {this.ID}; from {Math.Round(simTime, 2)} to {Math.Round(BusyUntil, 2)} after {Math.Round(processing.hourHDDInstallFinished.Value - processing.hourRAMInstallStart.Value, 2)}min wait.");
                Console.WriteLine($"Computer {processing.id} complete after after {Math.Round(processing.hourRAMInstallFinished.Value - processing.hourMBInstalled.Value, 2)}min in processing!");
            }
        }
    }
}
