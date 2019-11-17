using UnityEngine;
using System.Collections;

[System.Serializable]
public abstract class Node {
    public NodeStates CurrentNodeState { get { return currentNodeState; } }
    protected NodeStates currentNodeState = NodeStates.INVALID;

    public void Initialize() {
        if (currentNodeState != NodeStates.INVALID) {
            Debug.LogWarning("Initialize was called on a Node that was not invalid. Is this intended?");
        }
        OnInitialize();
    }

    protected abstract void OnInitialize();

    /// <summary>
    /// Evaluate the Node for a NodeState result.
    /// </summary>
    /// <returns>The nodes current state.</returns>
    /// <param name="deltaTime">Delta time.</param>
    public abstract NodeStates Evaluate(float deltaTime);

    public void Terminate() {
        currentNodeState = NodeStates.INVALID;
        OnTerminate();
    }

    protected abstract void OnTerminate();

}

public enum NodeStates {
    FAILURE,
    SUCCESS,
    RUNNING,
    INVALID
}