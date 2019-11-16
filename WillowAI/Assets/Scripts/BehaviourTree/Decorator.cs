using UnityEngine;
using System.Collections;
using System;

public abstract class Decorator : Node {

    protected Node decoratedNode;

    public Decorator(Node decoratedNode) {
        this.decoratedNode = decoratedNode ?? throw new System.ArgumentException("Node cannot be null");
    }

    public override void Initialize() {
        decoratedNode.Initialize();
    }

    public override NodeStates Evaluate(float deltaTime) {
        currentNodeState = MyAction(decoratedNode.Evaluate(deltaTime));
        return currentNodeState;
    }

    public override void Terminate() {
        decoratedNode.Terminate();
    }

    protected abstract NodeStates MyAction(NodeStates nodeStates);
}
