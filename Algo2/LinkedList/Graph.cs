using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AlgoDataStructures{
    public class Graph<D>: IEnumerable<D>{
        public static Graph<string>[] ParseFile(string fullPath){
            string[] fileLines = File.ReadAllLines(fullPath);
            List<List<string>> lists = new List<List<string>>();
            List<string> tmp = new List<string>();

            for(int i = 0; i < fileLines.Length; i++){
                if(string.IsNullOrEmpty(fileLines[i]) && tmp.Count > 0){
                    lists.Add(tmp);
                    tmp = new List<string>();
                }else{
                    tmp.Add(fileLines[i]);
                }
            }
            if(tmp.Count > 0) {
                lists.Add(tmp);
            }

            return lists.Select(x => ParseConnectionList(x)).ToArray();
        }
        public static Graph<string> ParseConnectionList(List<string> lines){ 
            Graph<string> ret = new Graph<string>();
            
            ret.graphMap = lines[0].Split(',').Select(x => new GraphNode<string>(x)).ToDictionary(x => x.Data);
            
            ret.startNode = ret.graphMap[lines[1].Split(',')[0]];
            ret.endNode = ret.graphMap[lines[1].Split(',')[1]];

            for(int i = 2; i < lines.Count; i++){
                string[] connections = lines[i].Split(',');
                for(int j = 1; j < connections.Length; j++){
                    GraphConnenction<string, GraphNode<string>>.Connect(ret.graphMap[connections[0]], ret.graphMap[connections[j]]);
                }
            }

            return ret;
        }

        private Dictionary<D, GraphNode<D>> graphMap = new Dictionary<D, GraphNode<D>>();
        private GraphNode<D> startNode, endNode;

        public bool TryMazeSolve(out IEnumerable<D> enu) {
            enu = MazeSolve();
            return enu != null;
        }
        public IEnumerable<D> MazeSolve() {
            IEnumerable<List<GraphNode<D>>> paths = new List<List<GraphNode<D>>>() { new List<GraphNode<D>>() { startNode } };

            bool changed = false;
            do {
                changed = false;
                paths = paths.Select(x => {
                    List<List<GraphNode<D>>> r = new List<List<GraphNode<D>>>();
                    foreach(GraphNode<D> g in x.Last().connections.Select(w => w.Follow())) {
                        if(!x.Contains(g)) {
                            List<GraphNode<D>> e = x.ToList();//clone
                            e.Add(g);
                            r.Add(e);
                            changed = true;
                        }
                    }
                    return r;
                }).SelectMany(x => x).ToList();

                IEnumerable<List<GraphNode<D>>> solutions = paths.Where(x => x.Contains(endNode)).OrderBy(x => x.Count).ToList();
                if(solutions.Any()){
                    return solutions.First().Select(x => x.Data);
                }
            } while(changed);

            return null;
        }

        public IEnumerator<D> GetEnumerator() {
            List<GraphNode<D>> traversed = new List<GraphNode<D>>();
            IEnumerable<GraphNode<D>> traversing = new GraphNode<D>[]{ startNode };
            while(traversing.Any()){
                traversing = traversing.Where(x => x != null && !traversed.Contains(x));
                foreach(GraphNode<D> n in traversing){
                    traversed.Add(n);
                    yield return n.Data;
                }
                traversing = traversing.Select(x => x.connections).SelectMany(x => x).Select(x => x.Follow());
            }
        }
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }

    public class GraphNode<T> {
        public T Data{ get; private set; }
        internal List<GraphConnenction<T, GraphNode<T>>> connections = new List<GraphConnenction<T, GraphNode<T>>>();

        internal protected GraphNode(T data){
            Data = data;
        }
    }

    public class GraphConnenction<D, N>: GraphConnenction<D, N, D, N> where N : GraphNode<D> {
        public static void Connect(N f, N t) {
            f.connections.Add(new GraphConnenction<D, GraphNode<D>>(f, t));
            //t.connections.Add(new GraphConnenction<D, GraphNode<D>>(t, f));
        }

        internal protected GraphConnenction(N from, N to) : base(from, to){}
    }
    public class GraphConnenction<D1, N1, D2, N2> where N1: GraphNode<D1>  where N2 : GraphNode<D2> {
        public N1 From { get; private set; }
        public N2 To { get; private set; }

        public virtual N2 Follow() => To;

        internal protected GraphConnenction(N1 from, N2 to) {
            From = from;
            To = to;
        }
    }
}