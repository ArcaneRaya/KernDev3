using System;
using UnityEngine;

namespace WhispActions {
    public class SetTargetLookPosition : InstanceBoundBehaviour<Whisp> {
        private Func<Whisp, Vector3> func;

        public SetTargetLookPosition(Whisp target, Func<Whisp, Vector3> func) : base(target) {
            this.func = func;
        }

        protected override void OnInitialize() {
            base.OnInitialize();

            target.TargetLookPosition = func.Invoke(target);
            currentNodeState = NodeStates.SUCCESS;
        }

        protected override NodeStates MyAction(float deltaTime) {
            return currentNodeState;
        }
    }
}
