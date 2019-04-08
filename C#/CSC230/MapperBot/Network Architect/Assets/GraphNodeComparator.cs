using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphNodeComparator: IEqualityComparer<GraphNode> {
    public bool Equals(GraphNode x, GraphNode y) {
        return x.name == y.name;
    }

    public int GetHashCode(GraphNode obj) {
        return obj.name.GetHashCode();
    }
}
