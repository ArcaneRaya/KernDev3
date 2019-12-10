using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class PillarGenerator : MonoBehaviour {

    [SerializeField] private int minGroupSize = 200;

    private PerlinRoomGenerator perlinRoomGenerator {
        get {
            if (perlinRoomGeneratorReference == null) {
                perlinRoomGeneratorReference = GetComponent<PerlinRoomGenerator>();
            }
            return perlinRoomGeneratorReference;
        }
    }
    private PerlinRoomGenerator perlinRoomGeneratorReference;

    private EnvironmentContainer environment {
        get {
            if (environmentReference == null) {
                environmentReference = GetComponent<EnvironmentContainer>();
            }
            return environmentReference;
        }
    }
    private EnvironmentContainer environmentReference;

    [ContextMenu("Generate Pillars")]
    public void GeneratePillars() {
        Pillar[] pillars = GetComponentsInChildren<Pillar>();
        for (int i = pillars.Length - 1; i >= 0; i--) {
            if (Application.isPlaying) {
                Destroy(pillars[i].gameObject);
            } else {
                DestroyImmediate(pillars[i].gameObject);
            }
        }

        Group[] groups = GetComponentsInChildren<Group>();
        foreach (var group in groups) {
            if (group is Path) { continue; }

            if (group.Cells.Count >= minGroupSize) {
                SpawnPillars(group);
            }
        }
    }

    private void SpawnPillars(Group group) {
        List<Vector2Int> pillarPositions = new List<Vector2Int>();
        foreach (var cell in group.Cells) {
            if (perlinRoomGenerator.GetFloorHeightAt(cell.Position) > 0.7f) {
                if (CanBuildPillarAt(cell.Position, pillarPositions)) {
                    var pillarPos = cell.Position;
                    pillarPositions.Add(pillarPos);
                    float height = perlinRoomGenerator.GetFloorHeightAt(pillarPos);
                    CreatePillarAt(group, pillarPos, height);
                }
            }
        }
    }

    private void CreatePillarAt(Group group, Vector2Int pillarPos, float height) {
        GameObject Cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        Cylinder.AddComponent<Pillar>().Group = group;
        Cylinder.transform.SetParent(transform);
        Cylinder.transform.localPosition = new Vector3(pillarPos.x, height, pillarPos.y);
        Cylinder.layer = ShiftingHelper.ShiftBack(environment.ObstacleLayer);
    }

    private bool CanBuildPillarAt(Vector2Int position, List<Vector2Int> pillarPositions) {
        float minDistance = 15;
        foreach (var pillarPos in pillarPositions) {
            int sqrDist = (pillarPos - position).sqrMagnitude;
            if (sqrDist < minDistance * minDistance) {
                return false;
            }
        }
        return true;
    }
}
