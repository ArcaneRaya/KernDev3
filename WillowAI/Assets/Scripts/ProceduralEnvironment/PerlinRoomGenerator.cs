using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class PerlinRoomGenerator : MonoBehaviour {
    private enum NoiseType {
        Perlin,
        Simplex,
        CellularF1,
        CellularF2
    }

    [SerializeField] private NoiseType noiseType = NoiseType.Perlin;
    [SerializeField] private int noiseOffset = 1234;
    [SerializeField] private float stepMultiplier = 0.1f;
    [SerializeField] private float threshold = 0.5f;
    [SerializeField] private bool invert = true;
    [SerializeField] private bool useHeight = false;

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

    [ContextMenu("Generate Terrain Random")]
    public void GenerateTerrainRandom() {
        noiseOffset = UnityEngine.Random.Range(1, 100) * UnityEngine.Random.Range(1, 100);
        GenerateNewTerrain();
    }

    [ContextMenu("Generate Terrain")]
    public void GenerateNewTerrain() {
        if (transform.childCount > 0) {
            environment.CleanupChildren();
        }

        cells = new Cell[environment.Dimensions.x, environment.Dimensions.y];

        int x = environment.Dimensions.x - 1;
        GenerateCells(x);
        GroupCells();
    }

    public float GetFloorHeightAt(Vector2Int pos) {
        float height = 0;
        float2 scaledPosition = new float2((pos.x + noiseOffset) * stepMultiplier, (pos.y + noiseOffset) * stepMultiplier);
        switch (noiseType) {
            case NoiseType.Perlin:
                height = Unity.Mathematics.noise.cnoise(scaledPosition);
                break;
            case NoiseType.Simplex:
                height = Unity.Mathematics.noise.snoise(scaledPosition);
                break;
            case NoiseType.CellularF1:
                height = Unity.Mathematics.noise.cellular(scaledPosition).x;
                break;
            case NoiseType.CellularF2:
                height = Unity.Mathematics.noise.cellular(scaledPosition).y;
                break;
            default:
                Debug.LogWarning("Floor height calculation not implemented for " + noiseType);
                break;
        }
        return height;
    }

    private void GroupCells() {
        int groupCount = 0;
        for (int x = 0; x < environment.Dimensions.x; x++) {
            for (int y = 0; y < environment.Dimensions.y; y++) {
                if (cells[x, y] == null || cells[x, y].transform.parent != transform) {
                    continue;
                }
                groupCount++;
                GameObject groupObject = new GameObject("Group " + groupCount);
                Group newGroup = groupObject.AddComponent<Group>();
                groupObject.transform.SetParent(transform);
                AddToGroup(x, y, newGroup);
            }
        }
    }

    private void AddToGroup(int x, int y, Group group) {
        cells[x, y].transform.SetParent(group.transform);
        group.Cells.Add(cells[x, y]);
        cells[x, y].Group = group;
        if (x > 0) {
            if (cells[x - 1, y] != null && cells[x - 1, y].transform.parent == transform) {
                AddToGroup(x - 1, y, group);
            }
        }
        if (x < environment.Dimensions.x - 1) {
            if (cells[x + 1, y] != null && cells[x + 1, y].transform.parent == transform) {
                AddToGroup(x + 1, y, group);
            }
        }
        if (y > 0) {
            if (cells[x, y - 1] != null && cells[x, y - 1].transform.parent == transform) {
                AddToGroup(x, y - 1, group);
            }
        }
        if (y < environment.Dimensions.y - 1) {
            if (cells[x, y + 1] != null && cells[x, y + 1].transform.parent == transform) {
                AddToGroup(x, y + 1, group);
            }
        }
    }

    private void GenerateCells(int x) {
        while (x >= 0) {
            int y = environment.Dimensions.y - 1;
            GenerateLine(x, y);
            x--;
        }
    }

    private void GenerateLine(int x, int y) {
        while (y >= 0) {
            float height = GetFloorHeightAt(new Vector2Int(x, y));
            if ((height > threshold) != invert) {
                GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Cube);
                //plane.transform.localScale = Vector3.one * 0.1f;
                plane.transform.SetParent(transform);
                float planeHeight = useHeight ? height * 1.5f - 0.5f : 0;
                plane.transform.localPosition = new Vector3(x, planeHeight, y);
                plane.GetComponent<Renderer>().material = environment.FloorMat;
                plane.layer = ShiftingHelper.ShiftBack(environment.FloorLayer);
                Cell cell = plane.AddComponent<Cell>();
                cell.Position = new Vector2Int(x, y);
                cells[x, y] = cell;
            }
            y--;
        }
    }
}
