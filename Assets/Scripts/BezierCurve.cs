using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public abstract class BezierCurve : MonoBehaviour {
    [SerializeField] protected int curveSegments = 10;
    [SerializeField] protected GameObject startObject;
    [SerializeField] protected GameObject endObject;
    protected Vector3 startPoint;
    protected Vector3 endPoint;
    protected GameObject[] controlPoints;
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
    }

    private bool CheckForChanges() {
        bool hasChanged = false;

        //check subpoints
        if(controlPoints.Length > 0) {
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
            foreach(var point in controlPoints) point.transform.hasChanged = false;
        }

        return hasChanged;
    }

    protected abstract void InitializeNodes();
    protected abstract void UpdateNodes();
}
