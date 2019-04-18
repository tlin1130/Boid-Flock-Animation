using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatmullRomSpline3D{

    private Vector3[] cps;
    private Vector3[] tangents;
    private float parametricRange;

    private Vector4 h_fun(float t) {
        float tSqr = t * t;
        float tCube = tSqr * t;
        float hx = 2 * tCube - 3 * tSqr + 1;
        float hy = tCube - 2 * tSqr + t;
        float hz = -2 * tCube + 3 * tSqr;
        float hw = tCube - tSqr;

        return new Vector4(hx, hy, hz, hw);
    }

    public Vector3[] ControlPoints {
        set {
            cps = value;
            parametricRange = cps.Length - 3;//3
            tangents = new Vector3[cps.Length - 2];
            for (int i = 0; i < tangents.Length; i++) {
                tangents[i] = (cps[i + 2] - cps[i]) / 0.2f;//0.2f
            }
        }
    }

    public Vector3 Sample(float t) {
        if (t > 0 && t < parametricRange) {
            int knotOld = Mathf.FloorToInt(t);
            float localParm = t - (float)(knotOld);
            Vector4 h = h_fun(localParm);
            Vector3 p0 = cps[knotOld + 1];
            Vector3 m0 = tangents[knotOld];
            Vector3 p1 = cps[knotOld + 2];
            Vector3 m1 = tangents[knotOld + 1];
            return h.x * p0 + h.y * m0 + h.z * p1 + h.w * m1;
        }
        else {
            return Vector3.zero;
        }
    }

}
