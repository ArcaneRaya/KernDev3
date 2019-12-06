using UnityEngine;
using System.Collections;
using System;

public class Robot : MonoBehaviour, IAgent, IFragmentCollector {

    public BehaviourTree BehaviourTree { get; private set; }
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
        Behaviour moveToTargetNode = new WhispActions.MoveToTarget(this);
        Player player = (MainController.GetControllerOfType(typeof(PlayerController)) as PlayerController).Player;

        freeze = new Sequence(
            new CanSeeWhisp(this, viewRange),
            new Inverter(new IsPlayerCloserThanWhisps(this, viewRange)),
            new SetFrozen(this),
            new Monitor(
                new DoNothing(),
                new IsFrozen(this)
                //new Inverter(new WhispConditions.CanSeeAgent(this, player, viewRange)),
                )
        );

        bringFragmentToPlayer = new Monitor(
            new ActiveSelector(
                new Monitor(
                    new Sequence(
                        new WhispActions.SetTargetLookPosition(this, WhispActions.Helpers.PlayerPosition),
                        new WhispActions.LookAtTarget(this)),
                    new WhispConditions.IsPlayerWithinRange(this, maxOfferFragmentDistance)),
                new Sequence(
                    new WhispActions.SetTargetMovePosition(this, WhispActions.Helpers.PlayerPosition),
                    moveToTargetNode)),
            new FragmentCollector.HasFragment(this)
        );

        collectFragment = new Sequence(
            new WhispConditions.IsFragmentNear(this),
            new WhispActions.SetTargetFragment(this),
            new Monitor(
                    new Sequence(
                        new WhispActions.SetTargetMovePosition(this, WhispActions.Helpers.CurrentFragmentTarget),
                        moveToTargetNode,
                        new WhispActions.PickupFragment(this)),
                    new WhispConditions.IsTargetFragmentAlive(this))
            );

        followPlayer = new ActiveSelector(
            new Sequence(
                new Inverter(new WhispConditions.IsPlayerWithinRange(this, maxFollowPlayerDistance)),
                new WhispActions.SetTargetMovePosition(this, WhispActions.Helpers.PlayerPosition),
                moveToTargetNode),
            new Sequence(
                new WhispActions.SetTargetMovePosition(this, WhispActions.Helpers.RandomPosition),
                moveToTargetNode,
                new WhispActions.WaitRandom(this, 0.5f, 2f))
            );

        ActiveSelector RootSelector = new ActiveSelector(freeze, bringFragmentToPlayer, collectFragment, followPlayer);
        BehaviourTree = new BehaviourTree(RootSelector);
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
