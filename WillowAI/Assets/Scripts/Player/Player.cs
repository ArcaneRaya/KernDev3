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
    [SerializeField] private float pickupRange = 2f;

    private FragmentController fragmentController;
    private int collectedFragmentAmount;

    public void Initialize(FragmentController fragmentController) {
        pathfindingAgent.Initialize(this);
        this.fragmentController = fragmentController;
    }

    public void Setup() {

    }

    public void Tick(float deltaTime) {
        HandleMovement(deltaTime);
        pathfindingAgent.Tick(deltaTime);
        HandlePickup();
    }

    public void Terminate() {

    }

    public void SetPosition(Vector3 position) {
        transform.position = position;
    }

    private void HandlePickup() {
        if (Input.GetKeyDown(KeyCode.E)) {
            List<Fragment> fragmentsInRange = fragmentController.GetFragmentsInRange(Position, pickupRange);
            foreach (Fragment fragment in fragmentsInRange) {
                fragment.Pickup();
                collectedFragmentAmount++;
            }
        }
    }

    private void HandleMovement(float deltaTime) {
        Vector3 inputDirection = HandleMovementInput(deltaTime);
        pathfindingAgent.MoveTowards(Position + inputDirection * speed);
    }

    private Vector3 HandleMovementInput(float deltaTime) {
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
