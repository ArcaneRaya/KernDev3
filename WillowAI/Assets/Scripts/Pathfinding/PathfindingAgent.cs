using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAgent {
    Vector3 Position { get; }
    float Speed { get; }
    void SetPosition(Vector3 position);
}

public class PathfindingAgent : MonoBehaviour {

    public Action OnDestinationReachedAction;
    public bool IsMoving {
        get {
            return !(currentPath == null || currentPath.Count == 0);
        }
    }

    private PathfindingController pathfindingController;
    private IAgent agent;

    private Queue<PathfindingNode> currentPath;
    private bool destinationReachedFired;

    public void Initialize(IAgent entity) {
        pathfindingController = GameObject.FindObjectOfType<PathfindingController>();
        this.agent = entity;
        this.currentPath = new Queue<PathfindingNode>();
        this.agent.SetPosition(pathfindingController.Grid.GetClosestWalkableNode(entity.Position).WorldPosition);
    }

    public void Tick(float elapsedTime) {
        if (currentPath.Count > 0) {
            UpdateMovement(elapsedTime);
        }
        else {
            if (destinationReachedFired == false) {
                if (OnDestinationReachedAction != null) {
                    OnDestinationReachedAction();
                }
                destinationReachedFired = true;
            }
        }
    }

    public void MoveTowards(Vector3 targetPosition) {
        Stop();

        PathfindingNode targetNode = pathfindingController.Grid.GetClosestWalkableNode(targetPosition);
        PathfindingNode currentNode = pathfindingController.Grid.GetClosestWalkableNode(agent.Position);

        currentPath = pathfindingController.CalculatePath(currentNode, targetNode);
        destinationReachedFired = false;
    }

    private void UpdateMovement(float elapsedTime) {
        float traversableDistance = agent.Speed * elapsedTime;
        PathfindingNode currentTarget = currentPath.Peek();
        float sqrDistTarget = (currentTarget.WorldPosition - agent.Position).sqrMagnitude;
        // remove node from path if entity is close enough and can move further;
        if (sqrDistTarget < traversableDistance * traversableDistance) {
            if (currentPath.Count > 1) {
                currentPath.Dequeue();
                currentTarget = currentPath.Peek();
            }
            else {
                currentPath.Dequeue();
                agent.SetPosition(currentTarget.WorldPosition);
                if (OnDestinationReachedAction != null) {
                    OnDestinationReachedAction();
                }
                destinationReachedFired = true;
                return;
            }
        }

        Vector3 movementDirection = currentTarget.WorldPosition - agent.Position;
        agent.SetPosition(agent.Position + movementDirection.normalized * agent.Speed * elapsedTime);
    }

    public void Stop() {
        if (currentPath != null) {
            currentPath.Clear();
        }
    }
}
