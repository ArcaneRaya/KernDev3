using UnityEngine;
using System.Collections;

[System.Serializable]
public abstract class Node {
    /* The current state of the node */
    public NodeStates CurrentNodeState { get { return currentNodeState; } }
    protected NodeStates currentNodeState = NodeStates.INVALID;

    public NodeStates nodeState {
        get { return currentNodeState; }
    }

    public abstract void Initialize();

    /// <summary>
    /// Evaluate the Node for a NodeState result.
    /// </summary>
    /// <returns>The nodes current state.</returns>
    /// <param name="deltaTime">Delta time.</param>
    public abstract NodeStates Evaluate(float deltaTime);

    public virtual void Terminate() {
        currentNodeState = NodeStates.INVALID;
    }

}

public enum NodeStates {
    FAILURE,
    SUCCESS,
    RUNNING,
    INVALID
}