using System;

namespace BehaviourTree {
    namespace Generic {

        public class InvokeDelegate : Behaviour {
            private Action delegateToInvoke;

            public InvokeDelegate(Action delegateToInvoke) {
                this.delegateToInvoke = delegateToInvoke;
            }

            protected override void OnInitialize() {
                base.OnInitialize();
                delegateToInvoke.Invoke();
                currentNodeState = NodeStates.SUCCESS;
            }

            protected override NodeStates MyAction(float deltaTime) {
                return currentNodeState;
            }
        }
    }
}