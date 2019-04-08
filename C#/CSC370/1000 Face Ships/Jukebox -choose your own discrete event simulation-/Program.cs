using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Jukebox__choose_your_own_discrete_event_simulation_ {
    class Program {
        const int PatronsPerDay = 100_000;
        const double PatronsEnteredMeanTime = 60 / 12; //5
        const double MeanSongLength = 60 / 15, //4
                        SongLengthStddev = 1;
        const int ProviderCount = 1;
        private class SongMeanEnumerator: IEnumerable<double> {
            public IEnumerator<double> GetEnumerator() {
                while(true) {
                    yield return NormalTime(MeanSongLength, SongLengthStddev);
                }
            }

            IEnumerator IEnumerable.GetEnumerator() {
                return GetEnumerator();
            }
        }
        private class JukeboxEnumerator: IEnumerable<Jukebox> {
            int id = 0;
            public IEnumerator<Jukebox> GetEnumerator() {
                while(true) {
                    yield return new Jukebox(id, x => LNTime(SongMeanByIndex[x]));
                    id++;
                }
            }

            IEnumerator IEnumerable.GetEnumerator() {
                return GetEnumerator();
            }
        }

        private static double[] SongMeanByIndex = new SongMeanEnumerator().Take(ProviderCount).ToArray();
        private static Jukebox[] servers = new JukeboxEnumerator().Take(ProviderCount).ToArray();
        static void Main(string[] args) {
            Queue<Patron> patronsToProcess =
                new Queue<Patron>();

            double lastTime = 0.0;
            for(int i = 0; i < PatronsPerDay; i++) {
                lastTime += LNTime(PatronsEnteredMeanTime);

                patronsToProcess.Enqueue(new Patron(i, lastTime));
            }

            List<Patron> processedPatrons =
                new List<Patron>(patronsToProcess.Count);
            double simTime = 0;
            Patron currentPatron;
            while(patronsToProcess.Any()) {
                currentPatron = patronsToProcess.Dequeue();
                if(simTime < currentPatron.hourEntered) {
                    simTime = currentPatron.hourEntered;
                }

                Jukebox server = servers.OrderBy(x => x.ProjectedWaitTime(currentPatron.hourEntered)).First();
                double waitTime = server.ProjectedWaitTime(currentPatron.hourEntered);

                Console.WriteLine($"Patron {currentPatron.id,6} entered {Math.Round(currentPatron.hourEntered, 2),6} mins after opening.");
                Console.WriteLine($"Patron {currentPatron.id,6} waited {Math.Round(waitTime, 2),6} mins.");
                
                server.Serve(currentPatron, ref simTime);
                processedPatrons.Add(currentPatron);

                Console.WriteLine($"Patron {currentPatron.id,6} played a {Math.Round(currentPatron.songLength, 2),6} song min on Jukebox {server.id}.");
                Console.WriteLine($"Patron {currentPatron.id,6} left {Math.Round(server.busyUntil, 2),6} mins after opening.");
                Console.WriteLine();
            }

            double meanWaitTime = processedPatrons.Select(x => x.waitTime).Sum() / processedPatrons.Count;
            double minWaitTime = processedPatrons.Select(x => x.waitTime).Min();
            double maxWaitTime = processedPatrons.Select(x => x.waitTime).Max();
            Console.WriteLine();
            Console.WriteLine($"Min Wait Time: {Math.Round(minWaitTime, 2)} mins.");
            Console.WriteLine($"Max Wait Time: {Math.Round(maxWaitTime, 2)} mins.");
            Console.WriteLine($"Mean Wait Time: {Math.Round(meanWaitTime, 2)} mins.");

            Console.ReadLine();
        }

        private static Random rand = null;
        private static double LNTime(double mean) {
            if(rand == null) rand = new Random();

            double d = rand.NextDouble();
            return -Math.Log(1 - d) * mean;
        }
        private static double NormalTime(double mean, double standardDeviation) {
            if(rand == null) rand = new Random();

            double u1 = 1.0 - rand.NextDouble(); //uniform(0,1] random doubles
            double u2 = 1.0 - rand.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                         Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            double randNormal =
                         mean + standardDeviation * randStdNormal; //random normal(mean,stdDev^2)

            return randNormal;
        }

        private class Patron {
            public int id;
            public double hourEntered, songLength, waitTime;

            public Patron(int id, double hourEntered/*, double songLength*/) {
                this.id = id;
                this.hourEntered = hourEntered;
                //this.songLength = songLength;
            }
        }
        private class Jukebox {
            public int id;
            public double busyUntil = 0.0;
            public Func<int, double> TimeToServiceGenerator { get; }

            public Jukebox(int id, Func<int, double> timeToServiceGenerator) {
                this.id = id;
                TimeToServiceGenerator = timeToServiceGenerator;
            }

            public double ProjectedWaitTime(double simTime){
                double ret = busyUntil - simTime;
                return ret > 0? ret: 0;
            }
            public void Serve(Patron p, ref double simTime){
                p.waitTime = ProjectedWaitTime(p.hourEntered);

                if(busyUntil > simTime) {
                    simTime = busyUntil;
                }

                p.songLength = TimeToServiceGenerator(id);
                busyUntil = simTime + p.songLength;
            }
        }
    }
}
