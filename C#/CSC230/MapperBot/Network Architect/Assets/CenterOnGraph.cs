using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CenterOnGraph : MonoBehaviour {
    public Graph toCenterOn;

    // Update is called once per frame
    void Update() {
        if(toCenterOn.nodes.Length > 0) {
            transform.position = new Vector3(0, 0, -10) + toCenterOn.nodes.Select(x => x.transform.position).Aggregate((x, y) => x + y) / toCenterOn.nodes.Length;
            GetComponent<Camera>().orthographicSize = toCenterOn.nodes.Select(x => Vector3.Distance(transform.position, x.transform.position)).Max();
        }
    }
}
