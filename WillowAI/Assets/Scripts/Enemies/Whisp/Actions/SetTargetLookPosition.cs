using System;
using UnityEngine;

namespace WhispActions {
    public class SetTargetLookPosition : InstanceBoundBehaviour<IAgent> {
        private Func<IAgent, Vector3> func;

        public SetTargetLookPosition(IAgent target, Func<IAgent, Vector3> func) : base(target) {
            this.func = func;
        }

        protected override void OnInitialize() {
            base.OnInitialize();

            target.SetTargetLookPosition(func.Invoke(target));
            currentNodeState = NodeStates.SUCCESS;
        }

        protected override NodeStates MyAction(float deltaTime) {
            return currentNodeState;
        }
    }
}
