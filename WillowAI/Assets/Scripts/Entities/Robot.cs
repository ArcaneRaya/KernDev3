using UnityEngine;
using System.Collections;
using System;
using BehaviourTree;
using BehaviourTree.Generic;
using BehaviourTree.BehaviourAgent;
using BehaviourTree.BehaviourRobot;
using BehaviourTree.FragmentCollection;

public class Robot : MonoBehaviour, IBehaviourAgent, IFragmentCollector {

    public BehaviourTree.BehaviourTree BehaviourTree { get; private set; }
    public PathfindingAgent PathFindingAgent { get { return pathfindingAgent; } }
    public FragmentController FragmentController { get; private set; }
    public Vector3 Position { get { return transform.position; } }
    public float Speed { get { return speed; } }
    public Vector3 TargetMovePosition { get; private set; }
    public Vector3 TargetLookPosition { get; private set; }
    public float LastMoveTime { get; private set; }
    public float FragmentViewRange { get { return fragmentViewRange; } }
    public Fragment TargetFragment { get; private set; }
    public float FragmentPickupTime { get { return fragmentPickupTime; } }
    public int FragmentsInPosessionCount { get { return pickedUpFragmentCount; } }
    public Transform Transform { get { return transform; } }
    public float RotationSpeed { get { return rotationSpeed; } }
    public bool IsFrozen { get; private set; }

    [SerializeField] private PathfindingAgent pathfindingAgent = null;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float maxFollowPlayerDistance = 8;
    [SerializeField] private float viewRange = 8f;
    [SerializeField] private float maxOfferFragmentDistance = 4;
    [SerializeField] private float fragmentViewRange = 10f;
    [SerializeField] private float fragmentPickupTime = 1;
    private Player targetPlayer;

    private Node freeze;
    private Node bringFragmentToPlayer;
    private Node collectFragment;
    private Node followPlayer;
    private int pickedUpFragmentCount;

    public void Initialize(Player targetPlayer, FragmentController fragmentController) {
        this.targetPlayer = targetPlayer;
        FragmentController = fragmentController;
        PathFindingAgent.Initialize(this);
    }

    public void Setup() {
        Node moveToTargetNode = new MoveToTarget(this);
        Player player = (MainController.GetControllerOfType(typeof(PlayerController)) as PlayerController).Player;

        freeze = new Sequence(
            new CanSeeWhisp(this, viewRange),
            new Inverter(new IsPlayerCloserThanWhisps(this, viewRange)),
            new SetFrozen(this),
            new Monitor(
                new DoNothing(),
                new IsFrozen(this))
        );

        bringFragmentToPlayer = new Monitor(
            new ActiveSelector(
                new Monitor(
                    new Sequence(
                        new SetTargetLookPosition(this, Helpers.PlayerPosition),
                        new LookAtTarget(this)),
                    new IsPlayerWithinRange(this, maxOfferFragmentDistance)),
                new Sequence(
                    new SetTargetMovePosition(this, Helpers.PlayerPosition),
                    moveToTargetNode)),
            new HasFragment(this)
        );

        collectFragment = new Sequence(
            new IsFragmentNear(this),
            new SetTargetFragment(this),
            new Monitor(
                    new Sequence(
                        new SetTargetMovePosition(this, Helpers.CurrentFragmentTarget),
                        moveToTargetNode,
                        new PickupFragment(this)),
                    new IsTargetFragmentAlive(this))
            );

        followPlayer = new ActiveSelector(
            new Sequence(
                new Inverter(new IsPlayerWithinRange(this, maxFollowPlayerDistance)),
                new SetTargetMovePosition(this, Helpers.PlayerPosition),
                moveToTargetNode),
            new Sequence(
                new SetTargetMovePosition(this, Helpers.RandomPosition),
                moveToTargetNode,
                new WaitRandom(0.5f, 2f))
            );

        ActiveSelector RootSelector = new ActiveSelector(freeze, bringFragmentToPlayer, collectFragment, followPlayer);
        BehaviourTree = new BehaviourTree.BehaviourTree(RootSelector);
    }

    public bool ExchangeFragment() {
        if (FragmentsInPosessionCount > 0) {
            pickedUpFragmentCount--;
            return true;
        }
        return false;
    }

    public void Tick(float elapsedTime) {
        pathfindingAgent.Tick(elapsedTime);
        BehaviourTree.Tick(elapsedTime);
    }

    public void Terminate() {

    }

    public void SetPosition(Vector3 position) {
        transform.position = position;
    }

    public void SetTargetMovePosition(Vector3 position) {
        TargetMovePosition = position;
    }

    public void SetTargetLookPosition(Vector3 position) {
        TargetLookPosition = position;
    }

    public void SetLastMoveTimeToNow() {
        LastMoveTime = MainController.Instance.GameTime;
    }

    public void SetTargetFragment(Fragment fragment) {
        TargetFragment = fragment;
    }

    public void IncrementPickupCount() {
        pickedUpFragmentCount++;
    }

    public void SetFrozen() {
        IsFrozen = true;
    }

    internal void UnFreeze() {
        IsFrozen = false;
    }
}
