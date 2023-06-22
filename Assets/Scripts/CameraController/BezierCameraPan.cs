using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCameraPan : MonoBehaviour {
    [SerializeField] private Camera cam;
    [SerializeField] private BezierCurve bezierCurve;
    [SerializeField] private double totalPanTime = 2.0f;

    enum Easing : int {
        Smoothest = 1,
        Smoother = 2,
        Smooth = 3,
        Jerky = 4,
        Jerkier = 5,
        Jerkiest = 6
    }
    [SerializeField] private Easing smoothness = Easing.Smoothest; //can go between 1, 6

    private bool startPan = false;
    private bool panning = false;
    private double panningTime = 0;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.Space) && !panning) {
            startPan = true;
        }
    }

    void FixedUpdate() {
        if(startPan) {
            panning = true;
            startPan = false;
            panningTime = 0;
        }

        if(panningTime > totalPanTime) {
            panning = false;
        }

        if(panning) {
            panningTime += Time.deltaTime;
            cam.transform.position = GetPanningPoint(panningTime);
        }
    }

    private Vector3 GetPanningPoint(double panTime) {
        float tFrac = (float) (panTime / totalPanTime);
        int[] eulerians = { 0, 1, 4, 11, 26, 57 };
        Debug.Log(tFrac);

        double n = (int) Math.Pow(2, (int)smoothness);
        double a = Math.Pow(2, eulerians[(int)smoothness-1]) * n;
        float tBezier = (float)(tFrac <= 0.5f
            ? a * Math.Pow(tFrac, n)
            : -a * Math.Pow(tFrac - 1, n) + 2 * a * Math.Pow(0.5, n));

        return bezierCurve.GetCurvePoint(tBezier);
    }
}
