using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LinearBezierCurve : BezierCurve {

    protected override void InitializeNodes() { }

    public override Vector3 GetCurvePoint(float t) {
        return (1 - t) * startPoint + 
               t * endPoint; //linear interpolation: https://www.gamedeveloper.com/business/how-to-work-with-bezier-curve-in-games-with-unity
    }
}
