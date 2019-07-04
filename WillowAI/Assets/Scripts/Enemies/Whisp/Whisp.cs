using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Whisp : MonoBehaviour, IAgent {

    public float distanceWalk = 5f;
    private Node Root;

    public PathfindingAgent PathFindingAgent {
        get {
            return pathfindingAgent;
        }
    }

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
    [SerializeField] private float speed = 5f;
    [SerializeField] private float ViewRange = 10f;

    public void Initialize() {
        pathfindingAgent.Initialize(this);
    }

    public void OnSetup() {
        ActionNode<Whisp> SetExploringTargetAction = new WhispActions.Exploring(this);

        Selector RootSelector = new Selector(new List<Node>() { SetExploringTargetAction });
        Sequence RootSequence = new Sequence(new List<Node>() { RootSelector });

        Root = RootSequence;
    }

    public void Tick(float deltaTime) {
        pathfindingAgent.Tick(deltaTime);
        Root.Evaluate(deltaTime);
    }

    public void SetPosition(Vector3 position) {
        transform.position = position;
    }

    private NodeStates Flee(object obj, float deltaTime) {
        if ((Player.Instance.transform.position - transform.position).sqrMagnitude < ViewRange * ViewRange)
            return NodeStates.RUNNING;

        return NodeStates.FAILURE;
    }
}
