using System;
using UnityEngine;
using System.Collections;

public abstract class Behaviour : Node {
    public override void Initialize() {
    }

    public override NodeStates Evaluate(float deltaTime) {
        currentNodeState = MyAction(deltaTime);
        return currentNodeState;
    }

    public override void Terminate() {
        base.Terminate();
    }

    protected abstract NodeStates MyAction(float deltaTime);
}

public abstract class InstanceBoundActionNode<T> : Behaviour {
    protected T target;

    public InstanceBoundActionNode(T target) {
        this.target = target;
    }
}