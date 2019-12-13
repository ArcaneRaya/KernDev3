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
                CreateNewGroup(ref groupCount, x, y);
            }
        }
    }

    private void CreateNewGroup(ref int groupCount, int x, int y) {
        groupCount++;
        GameObject groupObject = new GameObject("Group " + groupCount);
        Group newGroup = groupObject.AddComponent<Group>();
        groupObject.transform.SetParent(transform);

        List<CellInfo> cellsToCheck = new List<CellInfo>();
        cellsToCheck.Add(new CellInfo(x, y));
        List<CellInfo> cachedCells = new List<CellInfo>();

        while (cellsToCheck.Count > 0) {
            CellInfo cell = cellsToCheck[0];
            cellsToCheck.RemoveAt(0);
            cachedCells.Add(cell);
            foreach (var potentialCell in GetNeighbours(cell)) {
                if (DoesCollectionContainCell(cachedCells, potentialCell) == false && DoesCollectionContainCell(cellsToCheck, potentialCell) == false) {
                    cellsToCheck.Add(potentialCell);
                }
            }
        }
        AddCellsToGroup(cachedCells, newGroup);
    }

    private bool DoesCollectionContainCell(List<CellInfo> collection, CellInfo cell) {
        foreach (var item in collection) {
            if (item.X == cell.X && item.Y == cell.Y) {
                return true;
            }
        }
        return false;
    }

    private void AddCellsToGroup(List<CellInfo> cellsInGroup, Group group) {
        foreach (var cell in cellsInGroup) {
            cells[cell.X, cell.Y].transform.SetParent(group.transform);
            group.Cells.Add(cells[cell.X, cell.Y]);
            cells[cell.X, cell.Y].Group = group;
        }
    }

    private class CellInfo {
        public int X;
        public int Y;
        public CellInfo(int x, int y) {
            X = x;
            Y = y;
        }
    }

    private List<CellInfo> GetNeighbours(CellInfo cell) {
        int x = cell.X;
        int y = cell.Y;
        List<CellInfo> neighbours = new List<CellInfo>();
        if (x > 0) {
            if (cells[x - 1, y] != null && cells[x - 1, y].transform.parent == transform) {
                neighbours.Add(new CellInfo(x - 1, y));
            }
        }
        if (x < environment.Dimensions.x - 1) {
            if (cells[x + 1, y] != null && cells[x + 1, y].transform.parent == transform) {
                neighbours.Add(new CellInfo(x + 1, y));
            }
        }
        if (y > 0) {
            if (cells[x, y - 1] != null && cells[x, y - 1].transform.parent == transform) {
                neighbours.Add(new CellInfo(x, y - 1));
            }
        }
        if (y < environment.Dimensions.y - 1) {
            if (cells[x, y + 1] != null && cells[x, y + 1].transform.parent == transform) {
                neighbours.Add(new CellInfo(x, y + 1));
            }
        }
        return neighbours;
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
