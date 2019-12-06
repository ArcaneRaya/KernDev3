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

    public Vector3 TargetMovePosition { get; private set; }
    public PathfindingAgent PathFindingAgent { get { return pathfindingAgent; } }
    public Transform Transform { get { return transform; } }
    public float RotationSpeed { get { return rotationSpeed; } }

    [SerializeField] private PathfindingAgent pathfindingAgent = null;
    [SerializeField] private float speed = 5;
    [SerializeField] private float pickupRange = 2f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private int collectedFragmentAmount;

    private FragmentController fragmentController;
    private FriendlyController friendlyController;

    public void Initialize(FragmentController fragmentController, FriendlyController friendlyController) {
        pathfindingAgent.Initialize(this);
        this.fragmentController = fragmentController;
        this.friendlyController = friendlyController;
    }

    public void Setup() {

    }

    public void Tick(float deltaTime) {
        HandleMovement(deltaTime);
        pathfindingAgent.Tick(deltaTime);
        HandlePickup();
        HandleRobotInteraction();
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

    private void HandleRobotInteraction() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            List<Robot> robotsInRange = friendlyController.GetFriendliesInRange(Position, pickupRange);
            foreach (Robot robot in robotsInRange) {
                if (robot.IsFrozen) {
                    robot.UnFreeze();
                } else {
                    bool exchangeSucceeded = robot.ExchangeFragment();
                    if (exchangeSucceeded) {
                        collectedFragmentAmount++;
                    }
                }
            }
        }
    }

    private void HandleMovement(float deltaTime) {
        Vector3 inputDirection = HandleMovementInput();
        pathfindingAgent.MoveTowards(Position + inputDirection * speed, this);
    }

    private Vector3 HandleMovementInput() {
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
