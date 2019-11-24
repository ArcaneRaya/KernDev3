using System;
using UnityEngine;

namespace WhispActions {
    public class LookAtTarget : InstanceBoundBehaviour<Whisp> {

        public LookAtTarget(Whisp target) : base(target) {
        }

        protected override void OnInitialize() {
            base.OnInitialize();
            currentNodeState = NodeStates.RUNNING;
        }

        protected override NodeStates MyAction(float deltaTime) {
            Vector3 lookDirection = target.TargetLookPosition - target.Position;
            lookDirection.Normalize();
            if (Vector3.Angle(target.transform.forward, lookDirection) < 5) {
                currentNodeState = NodeStates.SUCCESS;
                return currentNodeState;
            }
            target.transform.rotation = Quaternion.Slerp(target.transform.rotation, Quaternion.LookRotation(lookDirection, Vector3.up), target.RotationSpeed * deltaTime);
            return currentNodeState;
        }
    }
}
