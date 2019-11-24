using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu]
[System.Serializable]
public class PathfindingGrid {

    //[Header("Settings")]
    public LayerMask Floor;
    public LayerMask Obstacle;
    public float NodeSize = 1;
    public float UnitHeight = 2;

    public int rowAmount;
    public int columnAmount;
    public PathfindingNode[] Nodes;

    public PathfindingNode GetClosestWalkableNode(Vector3 worldPosition) {
        float sqrDist = float.MaxValue;
        PathfindingNode closestNode = null;

        foreach (PathfindingNode node in Nodes) {
            if (node.IsWalkable == false) { continue; }

            float newSqrDist = (node.WorldPosition - worldPosition).sqrMagnitude;
            if (newSqrDist < sqrDist) {
                sqrDist = newSqrDist;
                closestNode = node;
            }
        }

        return closestNode;
    }

    public List<PathfindingNode> GetNeighbourNodes(PathfindingNode origin) {
        List<PathfindingNode> neighbours = new List<PathfindingNode>();

        // to the left
        if (origin.GridX > 0) {
            neighbours.Add(Nodes[(origin.GridX - 1) * columnAmount + origin.GridY]);
            if (origin.GridY > 0) {
                neighbours.Add(Nodes[(origin.GridX - 1) * columnAmount + (origin.GridY - 1)]);
            }
            if (origin.GridY < columnAmount - 1) {
                neighbours.Add(Nodes[(origin.GridX - 1) * columnAmount + (origin.GridY + 1)]);
            }
        }
        // top and bottom
        if (origin.GridY > 0) {
            neighbours.Add(Nodes[(origin.GridX) * columnAmount + (origin.GridY - 1)]);
        }
        if (origin.GridY < columnAmount - 1) {
            neighbours.Add(Nodes[(origin.GridX) * columnAmount + (origin.GridY + 1)]);
        }
        // to the right
        if (origin.GridX < rowAmount - 1) {
            neighbours.Add(Nodes[(origin.GridX + 1) * columnAmount + origin.GridY]);
            if (origin.GridY > 0) {
                neighbours.Add(Nodes[(origin.GridX + 1) * columnAmount + (origin.GridY - 1)]);
            }
            if (origin.GridY < columnAmount - 1) {
                neighbours.Add(Nodes[(origin.GridX + 1) * columnAmount + (origin.GridY + 1)]);
            }
        }

        return neighbours;
    }

    //[ContextMenu("Generate From Open Scene")]
    public void GenerateGridFromOpenScene() {
        Vector3 bottomFrontLeft;
        Vector3 topBackRight;
        FindGridExtent(out bottomFrontLeft, out topBackRight);

        rowAmount = Mathf.CeilToInt((topBackRight.x - bottomFrontLeft.x) / NodeSize);
        columnAmount = Mathf.CeilToInt((topBackRight.z - bottomFrontLeft.z) / NodeSize);

        Nodes = new PathfindingNode[rowAmount * columnAmount];
        float aboveWorldHeight = topBackRight.y + 1;
        float maxSpherecastLength = (topBackRight.y - bottomFrontLeft.y) + 2;

        Vector2 firstNodeCenter = new Vector2(bottomFrontLeft.x + NodeSize / 2, bottomFrontLeft.z + NodeSize / 2);

        for (int x = 0; x < rowAmount; x++) {
            for (int z = 0; z < columnAmount; z++) {
                bool isObstacle = false;
                bool isWalkable = false;
                RaycastHit hit;
                Vector2 nodeCenter = new Vector2(firstNodeCenter.x + x * NodeSize, firstNodeCenter.y + z * NodeSize);
                Vector3 raycastOrigin = new Vector3(nodeCenter.x, aboveWorldHeight, nodeCenter.y);
                float spherecastRadius = NodeSize / 3;
                float height = float.MinValue;

                Vector3 worldPositionFloor = new Vector3(nodeCenter.x, bottomFrontLeft.y, nodeCenter.y);
                if (Physics.SphereCast(raycastOrigin, spherecastRadius, Vector3.down, out hit, maxSpherecastLength, Floor)) {
                    height = hit.point.y;
                    isWalkable = true;
                    worldPositionFloor.y = height;
                }

                float heightChecked = 0;
                while (heightChecked < UnitHeight) {
                    float heightAdjust = heightChecked > (UnitHeight - spherecastRadius * 2) ? UnitHeight - spherecastRadius * 2 : heightChecked;
                    Vector3 spherePos = worldPositionFloor + Vector3.up * spherecastRadius + Vector3.up * heightAdjust;
                    if (Physics.CheckSphere(spherePos, spherecastRadius, Obstacle)) {
                        isWalkable = false;
                        isObstacle = true;
                        break;
                    }
                    heightChecked += spherecastRadius * 2;
                }

                Nodes[x * columnAmount + z] = new PathfindingNode(isObstacle, isWalkable, x, z, height, nodeCenter);
            }
        }
    }

    private void FindGridExtent(out Vector3 bottomFrontLeft, out Vector3 topBackRight) {
        Queue<GameObject> floorObjects = new Queue<GameObject>(FindGameObjectsWithLayerMask(Floor));

        if (floorObjects == null || floorObjects.Count == 0) {
            throw new System.Exception(string.Format("No objects found in layer {0}, could not generate grid.", Floor));
        }

        bool initializedValues = false;

        bottomFrontLeft = Vector3.zero;
        topBackRight = Vector3.zero;

        while (initializedValues == false) {
            if (floorObjects.Count == 0) {
                throw new System.Exception(string.Format("No objects with collider found in layer {0}, could not generate grid.", Floor));
            }

            GameObject firstObject = floorObjects.Dequeue();

            Collider collider = firstObject.GetComponent<Collider>();
            if (collider == null) {
                continue;
            }

            bottomFrontLeft = collider.bounds.min;
            topBackRight = collider.bounds.max;
            initializedValues = true;
        }

        while (floorObjects.Count != 0) {
            GameObject floorObject = floorObjects.Dequeue();

            Collider collider = floorObject.GetComponent<Collider>();
            if (collider == null) {
                continue;
            }

            if (bottomFrontLeft.x > collider.bounds.min.x) {
                bottomFrontLeft.x = collider.bounds.min.x;
            }
            if (bottomFrontLeft.y > collider.bounds.min.y) {
                bottomFrontLeft.y = collider.bounds.min.y;
            }
            if (bottomFrontLeft.z > collider.bounds.min.z) {
                bottomFrontLeft.z = collider.bounds.min.z;
            }

            if (topBackRight.x < collider.bounds.max.x) {
                topBackRight.x = collider.bounds.max.x;
            }
            if (topBackRight.y < collider.bounds.max.y) {
                topBackRight.y = collider.bounds.max.y;
            }
            if (topBackRight.z < collider.bounds.max.z) {
                topBackRight.z = collider.bounds.max.z;
            }
        }
    }

    private GameObject[] FindGameObjectsWithLayerMask(LayerMask mask) {
        GameObject[] sceneGameObjects = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
        List<GameObject> matchingList = new List<GameObject>();
        for (int i = 0; i < sceneGameObjects.Length; i++) {
            if ((mask & (1 << sceneGameObjects[i].layer)) > 0) {
                matchingList.Add(sceneGameObjects[i]);
            }
        }
        return matchingList.ToArray();
    }
}
