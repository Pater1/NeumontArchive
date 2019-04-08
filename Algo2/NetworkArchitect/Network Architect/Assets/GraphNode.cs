using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GraphNode : MonoBehaviour {
    public GraphEdge[] connections;

    private class BFSPath{
        public IEnumerable<GraphEdge> containingTree;
        public List<GraphEdge> path;
        public GraphNode head;
        public float length;

        public BFSPath(IEnumerable<GraphEdge> containingTree, List<GraphEdge> path, GraphNode head, float length) {
            this.containingTree = containingTree;
            this.path = path;
            this.head = head;
            this.length = length;
        }
        public bool PathOut(out List<BFSPath> paths){
            paths = containingTree.Where(x => (x.a.name == head.name || x.b.name == head.name) &&
                                                !path.Select(y => y.name).Contains(x.name))
                                    .Select(x => {
                                        List<GraphEdge> pCopy = path.ToList();
                                        pCopy.Add(x);
                                        GraphNode nHead = x.a.name == head.name? x.b: x.a;
                                        float l = length + x.distance;
                                        return new BFSPath(containingTree, pCopy, nHead, l);
                                    })
                                    .ToList();
            if(paths.Any()){
                return true;
            }else{
                paths = new List<BFSPath>();
                paths.Add(this);
                return false;
            }
        }
    }
    public float MaximumDistanceInTree(IEnumerable<GraphEdge> tree){
        List<BFSPath> path = new List<BFSPath>(new BFSPath[] { new BFSPath(tree, new List<GraphEdge>(), this, 0) });
        bool anyPath = true;
        do {
            anyPath = false;
            path = path.Select(x => {
                List<BFSPath> tmp;
                bool b = x.PathOut(out tmp);
                if(b) anyPath = true;
                return tmp;
            }).SelectMany(x => x).ToList();
        } while(anyPath);

        return path.Select(x => x.length).Max();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
