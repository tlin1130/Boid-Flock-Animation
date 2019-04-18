using System.Collections.Generic;
using UnityEngine;

public class BoidFlockMovement : MonoBehaviour { 

    public GameObject cloneObject;
    public int numberOfBoids;
    public float speed;
    private List<GameObject> objectList;
    private Vector3[] catMullRomCps;         
   
    void Start() {
        objectList = new List<GameObject>();
        for (int i = 0; i < numberOfBoids; i++) {
            GameObject clone = Instantiate(cloneObject, cloneObject.transform.position, cloneObject.transform.rotation) as GameObject;            
            Vector3 position = new Vector3(Random.Range(-150f, 150f), Random.Range(100f, 150f), Random.Range(-150f, 150f));
            CheckPosition(position, objectList);
            clone.transform.position = position;                    
            clone.transform.localScale = new Vector3(1f, 1f, 1f);
            clone.name = cloneObject + i.ToString();            
            objectList.Add(clone);
        }
    }

    // Update is called once per frame
    void Update() {
        for (int i = 0; i < objectList.Count; i++) {
            moveTowardRedBoid(objectList[i]);
        }
    }
    
    public void CheckPosition(Vector3 newboid, List<GameObject> objectList) {
        foreach (GameObject oldBoid in objectList) {
            if (Vector3.SqrMagnitude(newboid - oldBoid.transform.position) < 10f * 10f) {
                newboid = newboid + (newboid - oldBoid.transform.position).normalized * 10f;
                CheckPosition(newboid, objectList);
            }
        }
    }

    public void moveTowardRedBoid(GameObject boid){
        Vector3[] cps;
        CatmullRomSpline3D Spline;
        cps = new Vector3[4];
        cps[0] = -2 * (boid.transform.forward) + boid.transform.position;
        cps[1] = boid.transform.position;
        cps[2] = cloneObject.transform.position + new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f)) - cloneObject.transform.forward;
        cps[3] = cloneObject.transform.forward + cloneObject.transform.position;
        Spline = new CatmullRomSpline3D();
        Spline.ControlPoints = cps;

        Vector3 currPos = EvaluteAt(speed, Spline, cps.Length);

        foreach (GameObject obj in objectList) {
            if (obj != boid) {
                if (Vector3.SqrMagnitude(currPos - obj.transform.position) < 10f * 10f) {
                    currPos = (currPos - obj.transform.position).normalized * 10f + obj.transform.position;
                }
            }
        }      
        Vector3 nextPos = EvaluteAt(speed + speed, Spline, cps.Length);
        boid.transform.position = currPos;
        Vector3 forward = nextPos - currPos;
        if (forward.sqrMagnitude > 1e-5){
            forward.Normalize();
            boid.transform.forward = forward;
        }
    }

    Vector3 EvaluteAt(float t, CatmullRomSpline3D spline, int p) {
        if (t > p) t -= p;
        return spline.Sample(t);
    }

}
