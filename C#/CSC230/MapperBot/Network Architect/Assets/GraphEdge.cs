using System;
using UnityEngine;

public class GraphEdge : MonoBehaviour {
    public GraphNode a, b;
    public Color Color {
        get{
            throw new NotImplementedException();
        }  
        set{
            
        }
    }
    public float distance, dist;
    float scalarFactor = 0.35f;

    public float annealingTime = 1, curAnnealTime;
    public float annealingScalar = 10;
    public float AnnealingFactor {
        get {
            return curAnnealTime / annealingTime;
        }
    }

    void Start() {
        curAnnealTime = annealingTime;
    }

    void Update () {
        transform.position = (a.transform.position + b.transform.position)/2;

        Vector3 diff = (a.transform.position - b.transform.position).normalized;
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

        dist = Vector3.Distance(a.transform.position, b.transform.position);
        transform.localScale = Vector3.Scale(transform.localScale, new Vector3(1,0,1)) + new Vector3(0,dist*scalarFactor,0);
        if(Mathf.Abs(dist - distance) > 0.25f * (1.0f - AnnealingFactor)){
            float delta = distance - dist;
            a.GetComponent<Rigidbody2D>().AddForce((a.transform.position - transform.position + ((Vector3)UnityEngine.Random.insideUnitCircle * AnnealingFactor)).normalized * delta * Mathf.Lerp(annealingScalar, 1.0f, AnnealingFactor));
            b.GetComponent<Rigidbody2D>().AddForce((b.transform.position - transform.position + ((Vector3)UnityEngine.Random.insideUnitCircle * AnnealingFactor)).normalized * delta * Mathf.Lerp(annealingScalar, 1.0f, AnnealingFactor));
        }

        curAnnealTime -= Time.deltaTime;
        if(curAnnealTime < 0) curAnnealTime = 0;
    }
}
