using System.Collections.Generic;

public class GraphEdgeCompairitor: IEqualityComparer<GraphEdge> {
    public bool Equals(GraphEdge x, GraphEdge y) {
        bool same = (x.a.name == y.a.name && x.b.name == y.b.name);
        bool swapped = (x.a.name == y.b.name && x.b.name == y.a.name);
        return same || swapped;
    }

    public int GetHashCode(GraphEdge obj) {
        return obj.a.name.GetHashCode() ^
            obj.b.name.GetHashCode();
    }
}
