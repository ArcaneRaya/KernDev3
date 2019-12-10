using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownpourGenerator : MonoBehaviour {

    private Cell[,] cells {
        get {
            return environment.Cells;
        }
        set {
            environment.Cells = value;
        }
    }

    private EnvironmentContainer environment {
        get {
            if (environmentReference == null) {
                environmentReference = GetComponent<EnvironmentContainer>();
            }
            return environmentReference;
        }
    }
    private EnvironmentContainer environmentReference;

    [ContextMenu("Generate Downpour")]
    public void GenerateDownpour() {
        DownpourPlane[] downpourPlanes = GetComponentsInChildren<DownpourPlane>();
        for (int i = downpourPlanes.Length - 1; i >= 0; i--) {
            if (Application.isPlaying) {
                Destroy(downpourPlanes[i].gameObject);
            } else {
                DestroyImmediate(downpourPlanes[i].gameObject);
            }
        }

        for (int x = 0; x < environment.Dimensions.x; x++) {
            for (int y = 0; y < environment.Dimensions.y; y++) {
                if (cells[x, y] != null) {
                    GenerateDownPourForCel(x, y);
                }
            }
        }
    }

    private void GenerateDownPourForCel(int x, int y) {
        if (x == 0 || (x > 0 && cells[x - 1, y] == null)) {
            CreateDownpourWithSettings(x, y, new Vector3(-0.501f, -2.3f, 0), new Vector3(0, 180, -90));
        }
        if (x == environment.Dimensions.x - 1 || (x < environment.Dimensions.x - 1 && cells[x + 1, y] == null)) {
            CreateDownpourWithSettings(x, y, new Vector3(0.501f, -2.3f, 0), new Vector3(0, 0, -90));
        }
        if (y == 0 || (y > 0 && cells[x, y - 1] == null)) {
            CreateDownpourWithSettings(x, y, new Vector3(0, -2.3f, -0.501f), new Vector3(0, 90, -90));
        }
        if (y == environment.Dimensions.y - 1 || (y < environment.Dimensions.y - 1 && cells[x, y + 1] == null)) {
            CreateDownpourWithSettings(x, y, new Vector3(0, -2.3f, 0.501f), new Vector3(0, 270, -90));
        }
    }

    private void CreateDownpourWithSettings(int x, int y, Vector3 localOffset, Vector3 localRotation) {
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.SetParent(cells[x, y].transform);
        plane.transform.localScale = new Vector3(0.5f, 0.1f, 0.1f);
        plane.transform.localPosition = localOffset;
        plane.transform.localEulerAngles = localRotation;
        plane.GetComponent<Renderer>().material = environment.DownpourMat;
        plane.AddComponent<DownpourPlane>();
    }
}
