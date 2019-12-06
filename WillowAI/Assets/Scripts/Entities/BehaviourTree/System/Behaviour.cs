using System;
using UnityEngine;
using System.Collections;

namespace BehaviourTree {

    public abstract class Behaviour : Node {
        protected override void OnInitialize() {
        }

        public override NodeStates Evaluate(float deltaTime) {
            currentNodeState = MyAction(deltaTime);
            return currentNodeState;
        }

        protected override void OnTerminate() {

        }

        protected abstract NodeStates MyAction(float deltaTime);
    }

    public abstract class InstanceBoundBehaviour<T> : Behaviour {
        protected T target;

        public InstanceBoundBehaviour(T target) {
            this.target = target;
        }
    }
}