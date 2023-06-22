using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public abstract class BezierCurve : MonoBehaviour {
    [SerializeField] protected int curveSegments = 10;
    [SerializeField] protected GameObject startObject;
    [SerializeField] protected GameObject endObject;
    protected Vector3 startPoint;
    protected Vector3 endPoint;
    protected GameObject[] controlPoints = null;
    protected Vector3[] nodes;

    void OnValidate() {
        //Debug.Log("VALIDATED");
        startPoint = startObject.transform.position;
        endPoint = endObject.transform.position;
        nodes = new Vector3[curveSegments + 1];
        nodes[0] = startPoint;
        nodes[curveSegments] = endPoint;
        InitializeNodes();
        UpdateNodes();
    }

    // Update is called once per frame
    void Update() {
        //Debug.Log(startObject.transform.hasChanged);
        if(CheckForChanges()) {
            startPoint = startObject.transform.position;
            endPoint = endObject.transform.position;
            nodes[0] = startPoint;
            nodes[curveSegments] = endPoint;
            UpdateNodes();
        }
        RenderBezier();
    }

    private void RenderBezier() {
        for(int i = 1; i < curveSegments + 1; i++) {
            Debug.DrawLine(nodes[i-1], nodes[i]);
        }

        //Add Zach's weird bounding lines for cubics
        if(controlPoints is { Length: 2 }) {
            Debug.DrawLine(startPoint, controlPoints[0].transform.position, Color.black);
            Debug.DrawLine(controlPoints[1].transform.position, endPoint, Color.black);
        }
    }

    private bool CheckForChanges() {
        bool hasChanged = false;

        //check subpoints
        if(controlPoints != null) {
            foreach(var point in controlPoints) {
                if(point.transform.hasChanged) {
                    hasChanged = true;
                    break;
                }
            }
        }

        //check start and end
        if(!hasChanged) {
            hasChanged = startObject.transform.hasChanged || endObject.transform.hasChanged;
        }

        //reset flags
        if(hasChanged) {
            startObject.transform.hasChanged = false;
            endObject.transform.hasChanged = false;
            if(controlPoints != null) foreach(var point in controlPoints) point.transform.hasChanged = false;
        }

        return hasChanged;
    }

    private Vector3 GetStartPoint() { return startPoint; }
    private Vector3 GetEndPoint() { return endPoint; }

    private Vector3[] GetControlPoints() {
        if(controlPoints is null) {
            return null;
        } else {
            Vector3[] pointVectors = new Vector3[controlPoints.Length];
            for(int i = 0; i < controlPoints.Length; i++) {
                pointVectors[i] = controlPoints[i].transform.position;
            }

            return pointVectors;
        }
    }

    public Vector3[] GetBezierPoints() {
        Vector3[] bezierPoints = controlPoints is null ? new Vector3[2] : new Vector3[2 + controlPoints.Length];
        bezierPoints[0] = GetStartPoint();
        bezierPoints[^1] = GetEndPoint();
        if(controlPoints is not null) {
            for(int i = 1; i < bezierPoints.Length; i++) {
                bezierPoints[i] = GetControlPoints()[i];
            }
        }

        return bezierPoints;
    }

    public double GetBezierCurveLength(int segments = 1000) {
        return segments * (GetCurvePoint(1f / segments) - startPoint).magnitude;
    }

    private void UpdateNodes() {
        for(int i = 1; i < curveSegments; i++) {
            float t = (float) i / (float) curveSegments;
            nodes[i] = GetCurvePoint(t);
        }
    }

    protected abstract void InitializeNodes();
    public abstract Vector3 GetCurvePoint(float t);
}
