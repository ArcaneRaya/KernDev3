using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Whisp : MonoBehaviour, IAgent {

    public float distanceWalk = 5f;
    private Node Root;

    public PathfindingAgent PathFindingAgent { get { return pathfindingAgent; } }
    public FragmentController FragmentController { get { return fragmentController; } }
    public Vector3 Position { get { return transform.position; } }
    public float Speed { get { return speed; } }
    public float PlayerViewRange { get { return playerViewRange; } }
    public float PlayerFleeRange { get { return playerFleeRange; } }
    public float FragmentViewRange { get { return fragmentViewRange; } }
    public float FragmentPickupRange { get { return fragmentPickupRange; } }
    public float FragmentPickupTime { get { return fragmentPickupTime; } }
    public bool IsCollecting { get { return (collectingAction as WhispActions.Collecting).IsCollectingFragment; } }

    [SerializeField] private PathfindingAgent pathfindingAgent = null;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float playerViewRange = 8f;
    [SerializeField] private float playerFleeRange = 5f;
    [SerializeField] private float fragmentViewRange = 10f;
    [SerializeField] private float fragmentPickupRange = 2;
    [SerializeField] private float fragmentPickupTime = 1;

    private ActionNode<Whisp> fleeingAction;
    private ActionNode<Whisp> allertingAction;
    private ActionNode<Whisp> collectingAction;
    private ActionNode<Whisp> exploringAction;
    private FragmentController fragmentController;

    public void Initialize(FragmentController fragmentController) {
        this.fragmentController = fragmentController;
        pathfindingAgent.Initialize(this);
    }

    public void Setup() {
        fleeingAction = new WhispActions.Fleeing(this);
        allertingAction = new WhispActions.Allerting(this);
        collectingAction = new WhispActions.Collecting(this);
        exploringAction = new WhispActions.Exploring(this);

        Selector RootSelector = new Selector(fleeingAction, allertingAction, collectingAction, exploringAction);
        //Sequence RootSequence = new Sequence(new List<Node>() { RootSelector });

        Root = RootSelector;
    }

    public void Tick(float deltaTime) {
        pathfindingAgent.Tick(deltaTime);
        Root.Evaluate(deltaTime);
    }

    public void SetPosition(Vector3 position) {
        transform.position = position;
    }
}
