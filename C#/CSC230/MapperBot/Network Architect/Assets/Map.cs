using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets {
    public class Map: MonoBehaviour {
        public Graph graph;

        public Path activePath = null;
        public InputField responce;
        public List<string> Cities = new List<string>();
        public List<Connection> connections = new List<Connection>();

        public Path ActivePath {
            get {
                return activePath;
            }

            set {
                activePath = value;
                if(responce != null) {
                    responce.text = activePath.cityFrom;
                    responce.text += new string(activePath.citiesThrough.SelectMany(x => " => " + x).ToArray());
                    responce.text += " => " + activePath.cityTo;
                }
            }
        }

        [System.Serializable]
        public class Connection{
            public string cityA, cityB;
            public float distance;

            public Connection(string cityA, string cityB, float distance) {
                this.cityA = cityA;
                this.cityB = cityB;
                this.distance = distance;
            }
        }

        public void AddCity(string city) {
            if(!Cities.Contains(city.ToLowerInvariant())) {
                Cities.Add(city.ToLowerInvariant());
            }
        }
        public void AddConnection(string cityA, string cityB, float dist) {
            cityA = cityA.ToLowerInvariant();
            cityB = cityB.ToLowerInvariant();

            AddCity(cityA);
            AddCity(cityB);
            Connection c = connections.Where(x =>
                (x.cityA == cityA && x.cityB == cityB) ||
                (x.cityA == cityB && x.cityB == cityA)
                ).FirstOrDefault();
            if(c != null){
                int index = connections.IndexOf(c);
                connections.Remove(c);
                c.distance = dist;
                connections.Add(c);
            } else{
                connections.Add(new Connection(cityA.ToLowerInvariant(), cityB.ToLowerInvariant(), dist));
            }
        }

        public void Graph() {
            List<string> lines = new List<string>();

            string line0Register = "";
            foreach(string s in Cities){
                if(!string.IsNullOrEmpty(line0Register)){
                    line0Register += ",";
                }
                line0Register += s;

                IEnumerable<Connection> cons = connections.Where(x => x.cityA == s || x.cityB == s);
                string lineReg = s;
                foreach(Connection c in cons) {
                    lineReg += "," + (c.cityA == s? c.cityB: c.cityA) + ":" + c.distance;
                }
                lines.Add(lineReg);
            }

            lines.Insert(0, line0Register);

            graph.BuildFromFile(lines.ToArray());
        }

        public List<string> ShortestPath(string start, string finish, out float distance) {
            start = start.ToLowerInvariant();
            finish = finish.ToLowerInvariant();

            List<string> evaluated = new List<string>();
            IEnumerable<Path> underConstruction = new List<Path>() { new Path(new string[] { start }, 0.0f) };

            IEnumerable<Path> toEvaluate = null;

            do {
                underConstruction = underConstruction.OrderBy(x => x.TravelDistance);
                Path nextEval = underConstruction.First();
                underConstruction = underConstruction.Skip(1);

                if(nextEval.cityTo == finish){
                    distance = nextEval.TravelDistance;
                    return (new string[] { nextEval.cityFrom }).Concat(nextEval.citiesThrough).Concat(new string[] { nextEval.cityTo }).ToList();
                }

                toEvaluate = connections
                                .Where(x => x.cityA == nextEval.cityTo || x.cityB == nextEval.cityTo)
                                .Where(x => (x.cityA == nextEval.cityTo) ? !evaluated.Contains(nextEval.cityFrom) : !evaluated.Contains(nextEval.cityTo))
                                .Select(x => new string[] { (x.cityA == nextEval.cityTo) ? x.cityB : x.cityA })
                                .Select(x => (new string[] { nextEval.cityFrom }).Concat(nextEval.citiesThrough).Concat(nextEval.cityTo == nextEval.cityFrom? new string[0]: new string[] { nextEval.cityTo }).Concat(x).Distinct().ToArray())
                                .Select(x => {
                                    Connection c = connections
                                    .Where(y => (y.cityA == nextEval.cityTo && y.cityB == x.Last()) ||
                                                (y.cityB == nextEval.cityTo && y.cityA == x.Last()))
                                    .FirstOrDefault();
                                    if(c != null) {
                                        return new Path(x, nextEval.TravelDistance + c.distance);
                                    }else{
                                        return null;
                                    }
                                }).Where(x => x != null).ToArray();

                evaluated.Add(nextEval.cityFrom);
                underConstruction = underConstruction.Concat(toEvaluate).ToArray();
            } while(toEvaluate.Any());

            distance = 0;
            return null;
        }

        [System.Serializable]
        public class Path{
            public string cityFrom, cityTo;
            public string[] citiesThrough;

            public float TravelDistance;

            public Path(IEnumerable<string> citiesThrough, float dist) {
                this.cityFrom = citiesThrough.First();
                this.cityTo = citiesThrough.Last();
                this.citiesThrough = citiesThrough.Skip(1).Reverse().Skip(1).Reverse().ToArray();
                TravelDistance = dist;
            }
        }
        public class PathFactory_From {
            public string cityFrom;

            public PathFactory_From(string cityFrom) {
                this.cityFrom = cityFrom;
            }
        }
        public class PathFactory_From_To {
            public string cityFrom, cityTo;
            public PathFactory_From_To(PathFactory_From from, string cityTo) {
                this.cityFrom = from.cityFrom;
                this.cityTo = cityTo;
            }
        }
        public class PathFactory_From_To_Through {
            public List<string> pathingRoute;
            public PathFactory_From_To_Through(PathFactory_From_To from, IEnumerable<string> citiesThrough) {
                pathingRoute = new List<string>();
                pathingRoute.Add(from.cityFrom);
                pathingRoute.AddRange(citiesThrough);
                pathingRoute.Add(from.cityTo);
            }

            public Path Render(Map map) {
                List<string> pathThrough = new List<string>() { pathingRoute[0] };
                float dist, distance = 0;
                for(int i = 1; i < pathingRoute.Count; i++){
                    pathThrough.AddRange(map.ShortestPath(pathingRoute[i - 1], pathingRoute[i], out dist).Skip(1));
                    distance += dist;
                }
                return new Path(pathThrough, distance);
            }
        }

        public void PathThrough(MapCommandInterpreter.PathFindCommand pathFindCommand) {
            IEnumerable<PathFactory_From> pff = pathFindCommand.cityFrom != "" ? new PathFactory_From[] { new PathFactory_From(pathFindCommand.cityFrom) } :
                                                    pathFindCommand.citiesThrough.Select(x => new PathFactory_From(x));

            IEnumerable<PathFactory_From_To> pfft = pff.Select(x => 
                pathFindCommand.cityTo != "" ? new PathFactory_From_To[] { new PathFactory_From_To(x, pathFindCommand.cityTo) } :
                                                    pathFindCommand.citiesThrough.Except(new string[] { x.cityFrom }).Select(y => new PathFactory_From_To(x, y))
            ).SelectMany(x => x);

            IEnumerable<PathFactory_From_To_Through> pfftt = pfft.Select(x => {
                IEnumerable<IEnumerable<string>> throughs = pathFindCommand.citiesThrough.Except(new string[] { x.cityFrom, x.cityTo }).DifferentCombinations();
                return throughs.Select(y => new PathFactory_From_To_Through(x, y));
            }).SelectMany(x => x);

            IEnumerable<Path> paths = pfftt.Select(x => x.Render(this)).OrderBy(x => x.TravelDistance).ToArray();

            ActivePath = paths.FirstOrDefault();
        }
    }
}