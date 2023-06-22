using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadraticBezierCurve : BezierCurve {
    [SerializeField] private GameObject controlObject;

    protected override void InitializeNodes() {
        controlPoints = new GameObject[1];
        controlPoints[0] = controlObject;
    }


    public override Vector3 GetCurvePoint(float t) {
        return (float) Math.Pow(1 - t, 2) * startPoint +
               2 * (1 - t) * t * controlObject.transform.position +
               (float) Math.Pow(t, 2) *
               endPoint; //quadratic interpolation: https://www.gamedeveloper.com/business/how-to-work-with-bezier-curve-in-games-with-unity
    }
}
