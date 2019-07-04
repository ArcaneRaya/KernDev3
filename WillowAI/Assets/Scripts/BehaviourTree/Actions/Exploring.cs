using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exploring : ActionNode<Whisp> {
    public Exploring(Whisp obj) : base(obj) {
    }

    public override NodeStates MyAction(Whisp whisp, float deltaTime) {
        if (whisp.targetSet)
            return NodeStates.RUNNING;
        else {
            whisp.targetPosition = new Vector3(UnityEngine.Random.Range(-whisp.distanceWalk, whisp.distanceWalk), whisp.transform.position.y, UnityEngine.Random.Range(-whisp.distanceWalk, whisp.distanceWalk));
            whisp.targetSet = true;
            return NodeStates.SUCCESS;
        }
    }
}