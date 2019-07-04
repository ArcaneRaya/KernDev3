using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingController : MonoBehaviour {

    public PathfindingGrid Grid;

    [ContextMenu("Generate From Scene")]
    public void GenerateGrid() {
        Grid.GenerateGridFromOpenScene();
    }

    private void OnDrawGizmosSelected() {
        if (Grid == null) { return; }
        if (Grid.rowAmount == 0 || Grid.columnAmount == 0) { return; }
        if (Grid.Nodes == null || Grid.Nodes.Length == 0) { return; }

        for (int i = 0; i < Grid.Nodes.Length; i++) {
            PathfindingNode node = Grid.Nodes[i];

            Vector3 center = node.WorldPosition;
            Vector3 size = Vector3.one * Grid.NodeSize;

            if (node.IsObstacle) {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(center, size);
            }
            else if (node.IsWalkable) {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireCube(center, size);
            }
            else {
                Debug.Log("whut?!" + node.WorldPosition);
                continue;
            }
        }
    }

    private List<ConsideredNode> Open;
    private List<ConsideredNode> Closed;

    public Queue<PathfindingNode> CalculatePath(PathfindingNode origin, PathfindingNode target) {
        Open = new List<ConsideredNode>();
        Closed = new List<ConsideredNode>();

        Open.Add(new ConsideredNode(origin, null, CalculateDirectTravelCost(origin, target)));

        while (true) {
            ConsideredNode currentNode = GetLowestCostNode(Open);
            if (currentNode == null) {
                Debug.LogError("Could not find path");
                return new Queue<PathfindingNode>();
            }

            Open.Remove(currentNode);
            Closed.Add(currentNode);

            if (currentNode.Info == target) {
                List<PathfindingNode> reversedPath = new List<PathfindingNode>();
                while (currentNode != null) {
                    reversedPath.Add(currentNode.Info);
                    currentNode = currentNode.Parent;
                }
                Queue<PathfindingNode> path = new Queue<PathfindingNode>();
                while (reversedPath.Count > 1) {
                    path.Enqueue(reversedPath[reversedPath.Count - 2]);
                    reversedPath.RemoveAt(reversedPath.Count - 2);
                }
                return path;
            }

            foreach (PathfindingNode neighbour in Grid.GetNeighbourNodes(currentNode.Info)) {
                if (neighbour.IsWalkable == false || Closed.Find(n => n.Info == neighbour) != null) {
                    continue;
                }

                ConsideredNode neighbourNode = Open.Find(n => n.Info == neighbour);
                if (neighbourNode == null) {
                    neighbourNode = new ConsideredNode(neighbour, currentNode, CalculateDirectTravelCost(neighbour, target));
                    Open.Add(neighbourNode);
                }
                else {
                    float newTravelCost = currentNode.TravelCost + CalculateDirectTravelCost(currentNode.Info, neighbour);
                    if (neighbourNode.TravelCost > newTravelCost) {
                        neighbourNode.SetParent(currentNode);
                    }
                }
            }
        }
    }

    private ConsideredNode GetLowestCostNode(List<ConsideredNode> List) {
        int currentNodeCost = int.MaxValue;
        ConsideredNode currentNode = null;

        foreach (ConsideredNode node in List) {
            if (node.TotalCost < currentNodeCost) {
                currentNodeCost = node.TotalCost;
                currentNode = node;
            }
        }

        return currentNode;
    }

    private int CalculateDirectTravelCost(PathfindingNode origin, PathfindingNode target) {
        int currentX = origin.GridX;
        int currentY = origin.GridY;
        int cost = 0;

        while (currentX != target.GridX || currentY != target.GridY) {
            if (currentX != target.GridX && currentY != target.GridY) {
                currentX = MoveCloser(currentX, target.GridX);
                currentY = MoveCloser(currentY, target.GridY);
                cost += 14;
            }
            else if (currentX != target.GridX) {
                currentX = MoveCloser(currentX, target.GridX);
                cost += 10;
            }
            else if (currentY != target.GridY) {
                currentY = MoveCloser(currentY, target.GridY);
                cost += 10;
            }
        }

        return cost;
    }

    private int MoveCloser(int origin, int target) {
        if (origin > target) {
            return origin - 1;
        }
        if (origin < target) {
            return origin + 1;
        }
        else {
            Debug.LogWarning("Could not move closer but was still called");
            return origin;
        }
    }

    private class ConsideredNode {
        public PathfindingNode Info;
        public ConsideredNode Parent { get; private set; }
        public int DirectTravelCostToTarget;

        public int TravelCost {
            get {
                if (Parent == null) {
                    return 0;
                }
                else if (Parent.Info.GridX == Info.GridX || Parent.Info.GridY == Info.GridY) {
                    return Parent.TravelCost + 10;
                }
                else {
                    return Parent.TravelCost + 14;
                }
            }
        }

        public int TotalCost {
            get {
                return TravelCost + DirectTravelCostToTarget;
            }
        }

        public ConsideredNode(PathfindingNode nodeInfo, ConsideredNode parent, int directTravelCostToTarget) {
            this.Info = nodeInfo;
            this.DirectTravelCostToTarget = directTravelCostToTarget;
            this.SetParent(parent);
        }

        public void SetParent(ConsideredNode parent) {
            this.Parent = parent;
        }
    }
}
