using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class GroupConnector : MonoBehaviour {

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

    private PerlinRoomGenerator perlinRoomGenerator {
        get {
            if (perlinRoomGeneratorReference == null) {
                perlinRoomGeneratorReference = GetComponent<PerlinRoomGenerator>();
            }
            return perlinRoomGeneratorReference;
        }
    }
    private EnvironmentContainer environmentReference;
    private PerlinRoomGenerator perlinRoomGeneratorReference;

    private List<Group> connectedGroups;

    [ContextMenu("Generate Paths")]
    public void GeneratePaths() {
        if (transform.childCount < 2) {
            Debug.LogWarning("Cannot make paths with less than 2 groups");
            return;
        }

        Path[] existingPaths = GetComponentsInChildren<Path>();
        for (int i = existingPaths.Length - 1; i >= 0; i--) {
            for (int cellitterator = existingPaths[i].Cells.Count - 1; cellitterator >= 0; cellitterator--) {
                if (Application.isPlaying) {
                    Destroy(existingPaths[i].Cells[cellitterator].gameObject);
                } else {
                    DestroyImmediate(existingPaths[i].Cells[cellitterator].gameObject);
                }
            }
            if (Application.isPlaying) {
                Destroy(existingPaths[i].gameObject);
            } else {
                DestroyImmediate(existingPaths[i].gameObject);
            }
        }

        connectedGroups = new List<Group>();

        var groupsToConnect = GetComponentsInChildren<Group>();
        int pathCount = 0;
        for (int i = 0; i < groupsToConnect.Length; i++) {
            if (connectedGroups.Contains(groupsToConnect[i])) { continue; }
            pathCount++;
            ConnectGroups(groupsToConnect[i], groupsToConnect[(i + 1) % groupsToConnect.Length], pathCount);
        }
    }

    private void ConnectGroups(Group group1, Group group2, int pathCount) {
        Cell start = group1.Cells[UnityEngine.Random.Range(0, group1.Cells.Count)];
        Cell end = group2.Cells[UnityEngine.Random.Range(0, group2.Cells.Count)];

        GameObject pathObject = new GameObject("Path " + pathCount);
        pathObject.transform.SetParent(transform);
        Path newPath = pathObject.AddComponent<Path>();
        newPath.StartGroup = group1;
        newPath.EndGroup = group2;

        connectedGroups.Add(group1);

        int x = start.Position.x;
        int y = start.Position.y;
        //bool startedPlacingCells = false;

        while (x != end.Position.x) {
            if (x < end.Position.x) {
                x++;
            } else {
                x--;
            }

            if (cells[x, y] == null) {
                CreatPathCell(newPath, x, y);
            }
            //else {
            //    if (startedPlacingCells) {
            //        if (cells[x, y].Group != group1) {
            //            connectedGroups.Add(cells[x, y].Group);
            //            return;
            //        }
            //    }
            //}
        }

        while (y != end.Position.y) {
            if (y < end.Position.y) {
                y++;
            } else {
                y--;
            }

            if (cells[x, y] == null) {
                CreatPathCell(newPath, x, y);
            }
            //else {
            //    if (startedPlacingCells) {
            //        if (cells[x, y].Group != group1) {
            //            connectedGroups.Add(cells[x, y].Group);
            //            return;
            //        }
            //    }
            //}
        }
    }

    private void CreatPathCell(Path newPath, int x, int y) {
        //startedPlacingCells = true;
        float height = perlinRoomGenerator.GetFloorHeightAt(new Vector2Int(x, y));
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //plane.transform.localScale = Vector3.one * 0.1f;
        plane.transform.SetParent(transform);
        plane.transform.localPosition = new Vector3(x, height * 1.5f - 0.5f, y);
        plane.GetComponent<Renderer>().material = environment.FloorMat;
        plane.layer = ShiftingHelper.ShiftBack(environment.FloorLayer);
        Cell cell = plane.AddComponent<Cell>();
        cell.Position = new Vector2Int(x, y);
        cells[x, y] = cell;
        newPath.Cells.Add(cell);
        cell.Group = newPath;
        cell.transform.SetParent(newPath.transform);
    }
}

