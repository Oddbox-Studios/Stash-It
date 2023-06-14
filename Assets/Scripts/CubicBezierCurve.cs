using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class CubicBezierCurve : BezierCurve {
    [SerializeField] private GameObject controlObjectOne;
    [SerializeField] private GameObject controlObjectTwo;

    protected override void InitializeNodes() {
        controlPoints = new GameObject[2];
        controlPoints[0] = controlObjectOne;
        controlPoints[1] = controlObjectTwo;
    }

    protected override void UpdateNodes() {
        for(int i = 1; i < curveSegments; i++) {
            float t = (float) i / (float) curveSegments;
            nodes[i] = (float) Math.Pow(1 - t, 3) * startPoint +
                       (float) Math.Pow(1 - t,2) * t * 3 * controlObjectOne.transform.position +
                       (float) Math.Pow(t, 2) * (1 - t) * 3 * controlObjectTwo.transform.position +
                       (float) Math.Pow(t, 3) * endPoint; //cubic interpolation: https://www.gamedeveloper.com/business/how-to-work-with-bezier-curve-in-games-with-unity
        }
    }
}
