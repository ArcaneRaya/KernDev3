using System;
namespace WhispActions {
    public class InvokeDelegate : InstanceBoundBehaviour<Whisp> {
        private Action delegateToInvoke;

        public InvokeDelegate(Whisp target, Action delegateToInvoke) : base(target) {
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
