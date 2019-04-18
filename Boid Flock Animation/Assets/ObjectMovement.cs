using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour {

    [Range(0.001f, 1)]
    public float speed = 0.01f;
    private float currentTime;
    private CatmullRomSpline3D spline;

	void Start () {
        spline = new CatmullRomSpline3D();
        Vector3[] cps = new Vector3[6];
        cps[0] = new Vector3(-75, 30, -75);
        cps[1] = new Vector3(0, 60, 75);
        cps[2] = new Vector3(75, 30, -75);
        cps[3] = cps[0];
        cps[4] = cps[1];
        cps[5] = cps[2];
        spline.ControlPoints = cps;
        currentTime = 0;

    }
	
	// Update is called once per frame
	void Update () {
        currentTime += speed;
        if (currentTime > 3) {
            currentTime = currentTime - 3;
        }
        Vector3 currPos = EvaluteAt(currentTime);
        Vector3 nextPos = EvaluteAt(currentTime + speed);

        gameObject.transform.position = currPos;

        Vector3 forward = nextPos - currPos;

        if (forward.sqrMagnitude > 1e-5) {
            forward.Normalize();
            gameObject.transform.forward = forward;
        }

    }

    Vector3 EvaluteAt(float t){
        if (t > 3) t -= 3;
        return spline.Sample(t);
    }
}
