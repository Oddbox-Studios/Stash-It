using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LinearBezierCurve : BezierCurve {

    protected override void InitializeNodes() { }
    protected override void UpdateNodes() {
        for(int i = 1; i < curveSegments; i++) {
            float t = (float)i / (float)curveSegments;
            nodes[i] = (1 - t) * startPoint + t * endPoint; //linear interpolation: https://www.gamedeveloper.com/business/how-to-work-with-bezier-curve-in-games-with-unity
        }
    }
}
