using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Whisp : MonoBehaviour, IAgent {

    public float distanceWalk = 5f;

    //public StateMachine<Whisp> StateMachine { get; private set; }
    public BehaviourTree BehaviourTree { get; private set; }
    public PathfindingAgent PathFindingAgent { get { return pathfindingAgent; } }
    public FragmentController FragmentController { get; private set; }
    public Vector3 Position { get { return transform.position; } }
    public Vector3 TargetMovePosition;
    public Vector3 TargetLookPosition;
    public Fragment TargetFragment { get; set; }

    public float LastFleeTime { get; private set; }
    public float LastMoveTime { get; private set; }
    public float Speed { get { return speed; } }
    public float RotationSpeed { get { return rotationSpeed; } }
    public float PlayerViewRange { get { return playerViewRange; } }
    public float PlayerFleeRange { get { return playerFleeRange; } }
    public float FragmentViewRange { get { return fragmentViewRange; } }
    public float FragmentPickupRange { get { return fragmentPickupRange; } }
    public float FragmentPickupTime { get { return fragmentPickupTime; } }

    [SerializeField] private PathfindingAgent pathfindingAgent = null;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float playerViewRange = 8f;
    [SerializeField] private float playerFleeRange = 5f;
    [SerializeField] private float fragmentViewRange = 10f;
    [SerializeField] private float fragmentPickupRange = 2;
    [SerializeField] private float fragmentPickupTime = 1;

    private Node fleeingAction;
    private Node allertingAction;
    private Node exploringAction;
    private Node idleAction;
    private int pickedUpFragmentCount;

    public void Initialize(FragmentController fragmentController) {
        FragmentController = fragmentController;
        //StateMachine = new StateMachine<Whisp>();
        pathfindingAgent.Initialize(this);
    }

    public void Setup() {
        Behaviour moveToTargetNode = new WhispActions.MoveToTarget(this);
        Condition canSeePlayerNode = new WhispConditions.CanSeePlayer(this);

        fleeingAction = new Sequence(
            canSeePlayerNode,
            new WhispConditions.IsPlayerWithinRange(this, PlayerFleeRange),
            new WhispActions.InvokeDelegate(this, SetLastFleeTimeToNow),
            new WhispActions.SetTargetMovePosition(this, WhispActions.Helpers.AwayFromPlayer),
            moveToTargetNode
            );

        allertingAction = new ActiveSelector(
            new Monitor(
                new Sequence(
                    new WhispActions.SetTargetLookPosition(this, WhispActions.Helpers.PlayerPosition),
                    new WhispActions.LookAtTarget(this)),
                canSeePlayerNode),
            new Monitor(
                new Sequence(
                    new WhispActions.SetTargetLookPosition(this, WhispActions.Helpers.RandomPosition),
                    new WhispActions.LookAtTarget(this)),
                new WhispConditions.HasFledRecently(this))
            );

        exploringAction = new ActiveSelector(
            new Sequence(
                new WhispConditions.IsFragmentNear(this),
                new WhispActions.SetTargetFragment(this),
                new Monitor(
                    new Sequence(
                        new WhispActions.SetTargetMovePosition(this, WhispActions.Helpers.CurrentFragmentTarget),
                        moveToTargetNode,
                        new WhispActions.PickupFragment(this)),
                    new WhispConditions.IsTargetFragmentAlive(this))),
            new Sequence(
                new Inverter(new WhispConditions.HasMovedRecently(this)),
                new WhispActions.SetTargetMovePosition(this, WhispActions.Helpers.RandomPosition),
                moveToTargetNode)
            );

        idleAction = new WhispActions.WaitRandom(this, 2, 4);

        ActiveSelector RootSelector = new ActiveSelector(fleeingAction, allertingAction, exploringAction, idleAction);
        //Sequence RootSequence = new Sequence(new List<Node>() { RootSelector });

        BehaviourTree = new BehaviourTree(RootSelector);
    }

    public void Tick(float deltaTime) {
        pathfindingAgent.Tick(deltaTime);
        BehaviourTree.Tick(deltaTime);
    }

    public void Terminate() {

    }

    public void IncrementPickupCount() {
        pickedUpFragmentCount++;
    }

    public void SetPosition(Vector3 position) {
        transform.position = position;
    }

    public void SetLastFleeTimeToNow() {
        LastFleeTime = MainController.Instance.GameTime;
    }

    public void SetLastMoveTimeToNow() {
        LastMoveTime = MainController.Instance.GameTime;
    }
}
