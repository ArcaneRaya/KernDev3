using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class FragmentLocationGenerator : MonoBehaviour {

    [SerializeField] private int minGroupSize = 200;
    [SerializeField] private GameObject locationPrefab = null;

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

    [ContextMenu("Generate Locations")]
    public void GenerateLocations() {
        FragmentLocation[] fragmentLocation = GetComponentsInChildren<FragmentLocation>();
        for (int i = fragmentLocation.Length - 1; i >= 0; i--) {
            if (Application.isPlaying) {
                Destroy(fragmentLocation[i].gameObject);
            } else {
                DestroyImmediate(fragmentLocation[i].gameObject);
            }
        }

        Group[] groups = GetComponentsInChildren<Group>();
        foreach (var group in groups) {
            if (group is Path) { continue; }

            if (group.Cells.Count >= minGroupSize) {
                SpawnLocations(group);
            }
        }
    }

    private void SpawnLocations(Group group) {
        List<Vector2Int> locationPositions = new List<Vector2Int>();
        foreach (var cell in group.Cells) {
            if (perlinRoomGenerator.GetFloorHeightAt(cell.Position) > 0.5f) {
                if (CanBuildLocationAt(cell.Position, locationPositions)) {
                    var pillarPos = cell.Position;
                    locationPositions.Add(pillarPos);
                    float height = perlinRoomGenerator.GetFloorHeightAt(pillarPos);
                    CreateLocationAt(group, pillarPos, height);
                }
            }
        }
    }

    private void CreateLocationAt(Group group, Vector2Int pos, float height) {
        GameObject location = Instantiate(locationPrefab, transform);
        location.transform.localPosition = new Vector3(pos.x, height + 0.3f, pos.y);
        location.transform.SetParent(group.transform);
    }

    private bool CanBuildLocationAt(Vector2Int position, List<Vector2Int> pillarPositions) {
        float minDistance = 4.5f;
        foreach (var pillarPos in pillarPositions) {
            int sqrDist = (pillarPos - position).sqrMagnitude;
            if (sqrDist < minDistance * minDistance) {
                return false;
            }
        }
        return true;
    }
}
