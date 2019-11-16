using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActiveSelector : Selector {

    public ActiveSelector(params Node[] nodes) : base(nodes) {
    }

    public override void Initialize() {
        base.Initialize();
    }

    public override NodeStates Evaluate(float deltaTime) {
        int previousSubNode = currentSubNode;
        base.Initialize();
        NodeStates result = base.Evaluate(deltaTime);
        if (currentSubNode != previousSubNode) {
            nodes[previousSubNode].Terminate();
        }
        return result;
    }
}