using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PathfindingNode {

    public Vector3 WorldPosition {
        get {
            return new Vector3(FlatWorldPosition.x, Height, FlatWorldPosition.y);
        }
    }

    public int GridX;
    public int GridY;
    public float Height;
    public bool IsObstacle;
    public bool IsWalkable;

    public Vector2 GridPosition { get { return new Vector2(GridX, GridY); } }
    public Vector2 FlatWorldPosition;

    public PathfindingNode Parent;

    public PathfindingNode(bool isObstacle, bool isWalkable, int gridX, int gridY, float height, Vector2 flatWorldPosition) {
        this.IsObstacle = isObstacle;
        this.IsWalkable = isWalkable;
        this.GridX = gridX;
        this.GridY = gridY;
        this.Height = height;
        this.FlatWorldPosition = flatWorldPosition;
    }
}
