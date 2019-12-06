using System;

namespace BehaviourTree {

    public abstract class Condition : Node {
        protected override void OnInitialize() {
        }

        public override NodeStates Evaluate(float deltaTime) {
            return MyCondition() ? NodeStates.SUCCESS : NodeStates.FAILURE;
        }

        protected override void OnTerminate() {
        }

        protected abstract bool MyCondition();
    }

    public abstract class InstanceBoundCondition<T> : Condition {
        protected T target;

        public InstanceBoundCondition(T target) {
            this.target = target;
        }
    }
}