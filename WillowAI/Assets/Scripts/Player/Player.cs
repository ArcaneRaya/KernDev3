using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IAgent {

    public Vector3 Position {
        get {
            return transform.position;
        }
    }

    public float Speed {
        get {
            return speed;
        }
    }

    [SerializeField] private PathfindingAgent pathfindingAgent = null;
    [SerializeField] private float speed = 5;

    public void Initialize() {
        pathfindingAgent.Initialize(this);
    }

    public void Tick(float deltaTime) {
        Vector3 inputDirection = HandleInput(deltaTime);
        HandleMovement(inputDirection);
        pathfindingAgent.Tick(deltaTime);
    }

    public void SetPosition(Vector3 position) {
        transform.position = position;
    }

    private void HandleMovement(Vector3 inputDirection) {
        pathfindingAgent.MoveTowards(Position + inputDirection * speed);
    }

    private Vector3 HandleInput(float deltaTime) {
        Vector3 moveDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) {
            moveDirection += transform.forward;
        }
        if (Input.GetKey(KeyCode.S)) {
            moveDirection -= transform.forward;
        }
        if (Input.GetKey(KeyCode.A)) {
            moveDirection -= transform.right;
        }
        if (Input.GetKey(KeyCode.D)) {
            moveDirection += transform.right;
        }
        return moveDirection.normalized;
    }
}
