using UnityEngine;
using BehaviourTree;
using BehaviourTree.BehaviourAgent;
using BehaviourTree.Generic;
using BehaviourTree.BehaviourWhisp;
using BehaviourTree.FragmentCollection;

public class Whisp : MonoBehaviour, IBehaviourAgent, IFragmentCollector {

    public float distanceWalk = 5f;

    //public StateMachine<Whisp> StateMachine { get; private set; }
    public BehaviourTree.BehaviourTree BehaviourTree { get; private set; }
    public PathfindingAgent PathFindingAgent { get { return pathfindingAgent; } }
    public FragmentController FragmentController { get; private set; }
    public Vector3 Position { get { return transform.position; } }
    public Vector3 TargetMovePosition { get; private set; }
    public Vector3 TargetLookPosition { get; private set; }
    public Fragment TargetFragment { get; private set; }
    public Transform Transform { get { return transform; } }

    public float LastFleeTime { get; private set; }
    public float LastMoveTime { get; private set; }
    public float Speed { get { return speed; } }
    public float RotationSpeed { get { return rotationSpeed; } }
    public float PlayerViewRange { get { return playerViewRange; } }
    public float PlayerFleeRange { get { return playerFleeRange; } }
    public float FragmentViewRange { get { return fragmentViewRange; } }
    public float FragmentPickupRange { get { return fragmentPickupRange; } }
    public float FragmentPickupTime { get { return fragmentPickupTime; } }
    public int FragmentsInPosessionCount { get { return pickedUpFragmentCount; } }

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
        Node moveToTargetNode = new MoveToTarget(this);
        Player player = (MainController.GetControllerOfType(typeof(PlayerController)) as PlayerController).Player;
        Node canSeePlayerNode = new CanSeeAgent(this, player, playerViewRange);

        fleeingAction = new Sequence(
            canSeePlayerNode,
            new IsPlayerWithinRange(this, PlayerFleeRange),
            new InvokeDelegate(SetLastFleeTimeToNow),
            new SetTargetMovePosition(this, Helpers.AwayFromPlayer),
            moveToTargetNode
            );

        allertingAction = new ActiveSelector(
            new Monitor(
                new Sequence(
                    new SetTargetLookPosition(this, Helpers.PlayerPosition),
                    new LookAtTarget(this)),
                canSeePlayerNode),
            new Monitor(
                new Sequence(
                    new SetTargetLookPosition(this, Helpers.RandomPosition),
                    new LookAtTarget(this)),
                new HasFledRecently(this))
            );

        exploringAction = new ActiveSelector(
            new Sequence(
                new IsFragmentNear(this),
                new SetTargetFragment(this),
                new Monitor(
                    new Sequence(
                        new SetTargetMovePosition(this, Helpers.CurrentFragmentTarget),
                        moveToTargetNode,
                        new PickupFragment(this)),
                    new IsTargetFragmentAlive(this))),
            new Sequence(
                new Inverter(new HasMovedRecently(this)),
                new SetTargetMovePosition(this, Helpers.RandomPosition),
                moveToTargetNode)
            );

        idleAction = new WaitRandom(2, 4);

        ActiveSelector RootSelector = new ActiveSelector(fleeingAction, allertingAction, exploringAction, idleAction);

        BehaviourTree = new BehaviourTree.BehaviourTree(RootSelector);
    }

    public void Tick(float elapsedTime) {
        pathfindingAgent.Tick(elapsedTime);
        BehaviourTree.Tick(elapsedTime);
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

    public void SetTargetMovePosition(Vector3 position) {
        TargetMovePosition = position;
    }

    public void SetTargetLookPosition(Vector3 position) {
        TargetLookPosition = position;
    }

    public void SetTargetFragment(Fragment fragment) {
        TargetFragment = fragment;
    }
}
