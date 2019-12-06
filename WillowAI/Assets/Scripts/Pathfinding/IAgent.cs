using UnityEngine;

public interface IAgent {
    Vector3 Position { get; }
    float Speed { get; }
    PathfindingAgent PathFindingAgent { get; }
    Transform Transform { get; }
    float RotationSpeed { get; }
    void SetPosition(Vector3 position);
}