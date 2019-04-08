using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Graph : MonoBehaviour {
    public string buildFromFile = "";
    public GameObject baseNode, baseEdge;
    public Vector2 graphSpace = new Vector2(15, 10);
    public GraphNode[] nodes;
    Dictionary<string, GraphNode> nodesMap = new Dictionary<string, GraphNode>();

    public GraphEdge[] edges;

    // Use this for initialization
    void Awake () {
        //BuildFromFile(buildFromFile);
    }

    public void BuildFromFile(string path) {
        if(string.IsNullOrEmpty(path)) {
            path = GameObject.Find("InputField").GetComponent<InputField>().text;
        }
        if(string.IsNullOrEmpty(path)) {
            return;
        }

        BuildFromFile(File.ReadAllLines(path));
    }
    public void BuildFromFile(string[] lines) {
        StopAllCoroutines();
        for(int i = 0; i < transform.childCount; i++) {
            Destroy(transform.GetChild(i).gameObject);
        }

        nodes = lines[0].Split(',')
                        .Select(x => {
                            GameObject g = baseNode != null ? Instantiate(baseNode) : new GameObject(x, typeof(GraphNode));
                            g.name = x;
                            g.transform.parent = transform;
                            g.transform.position = new Vector3(Random.Range(-graphSpace.x, graphSpace.x), Random.Range(-graphSpace.y, graphSpace.y), 0);
                            return g.GetComponent<GraphNode>();
                        })
                        .ToArray();
        nodesMap = nodes.ToDictionary(x => x.gameObject.name);

        List<GraphEdge> es = new List<GraphEdge>();
        for(int i = 1; i < lines.Length; i++) {
            string[] connections = lines[i].Split(',');
            GraphNode g = nodesMap[connections[0]];
            IEnumerable<GraphEdge> ge = connections.Skip(1).Select(x => x.Split(':')).Select(x => {
                GameObject e = baseEdge != null ? Instantiate(baseEdge) : new GameObject("", typeof(GraphEdge));
                e.name = string.Format("Connection {0} - {1}", g.name, x[0]);
                e.transform.parent = transform;
                GraphEdge dg = e.GetComponent<GraphEdge>();
                dg.a = g;
                dg.b = nodesMap[x[0]];
                dg.distance = float.Parse(x[1]);
                Color c = Color.white;
                if(x.Length > 2 && ColorUtility.TryParseHtmlString(x[2], out c)){
                    dg.Color = c;
                }
                return dg;
            });
            es.AddRange(ge);
        }
        edges = es.Distinct(new GraphEdgeCompairitor()).ToArray();
        foreach(GameObject go in es.Where(x => !edges.Contains(x)).Select(x => x.gameObject)) {
            DestroyImmediate(go);
        }
        foreach(GraphNode gn in nodes) {
            gn.connections = edges.Where(x => x.a == gn || x.b == gn).ToArray();
        }

        //StartCoroutine(ConstructMST());
    }

    private float WaitForSecondsTimeout = 0.5f;
    IEnumerator mst;
    public IEnumerator ConstructMST(){
        IEnumerable<GraphEdge> sortEdges = edges.OrderBy(x => x.distance);
        List<GraphEdge> mst = new List<GraphEdge>();
        foreach(GraphEdge e in sortEdges){
            e.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
            yield return new WaitForSeconds(WaitForSecondsTimeout);
            if(!WouldLoop(mst, e)){
                mst.Add(e);
                e.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0);
            }else {
                e.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            }
        }

        List<List<GraphEdge>> seperateTrees = new List<List<GraphEdge>>();
        foreach(GraphEdge e in mst){
            bool added = false;
            foreach(List<GraphEdge> tree in seperateTrees){
                IEnumerable<string> nodeNames = tree.Select(x => new string[] { x.a.name, x.b.name }).SelectMany(x => x);
                if(nodeNames.Contains(e.a.name) || nodeNames.Contains(e.b.name)){
                    tree.Add(e);
                    added = true;
                    break;
                }
            }
            if(!added){
                List<GraphEdge> toAdd = new List<GraphEdge>();
                toAdd.Add(e);
                seperateTrees.Add(toAdd);
            }
        }

        foreach(List<GraphEdge> tree in seperateTrees){
            Color treeColor = Random.ColorHSV(0, 1, 0.5f, 1, 0.75f, 1);
            foreach(GraphEdge edge in tree) {
                edge.GetComponent<SpriteRenderer>().color = treeColor;
            }
            yield return new WaitForSeconds(WaitForSecondsTimeout);
        }

        foreach(List<GraphEdge> tree in seperateTrees) {
            GraphNode center = null;
            float centerDist = Mathf.Infinity;
            IEnumerable<GraphNode> nodes = tree.Select(x => new GraphNode[] { x.a, x.b }).SelectMany(x => x).Distinct(new GraphNodeComparator());
            foreach(GraphNode node in nodes){
                float cntrdst = node.MaximumDistanceInTree(tree);
                node.GetComponent<SpriteRenderer>().color = new Color(0,0,1);
                yield return new WaitForSeconds(WaitForSecondsTimeout);

                if(cntrdst < centerDist){
                    centerDist = cntrdst;
                    if(center != null) center.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
                    center = node;
                    center.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
                }else {
                    node.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
                }
            }
        }
    }
    public bool WouldLoop(IEnumerable<GraphEdge> check, GraphEdge edge){
        bool hasA = false, hasB = false;
        foreach(GraphEdge c in check) {
            if(c.a.name == edge.a.name || c.b.name == edge.a.name) {
                hasA = true;
            }
            if(c.a.name == edge.b.name || c.b.name == edge.b.name) {
                hasB = true;
            }
            if(hasA && hasB){
                break;
            }
        }
        return hasA && hasB;
    }
}
