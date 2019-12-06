using System;
using UnityEngine;

namespace WhispConditions {
    public class CanSeeAgent : InstanceBoundCondition<IAgent> {

        private IAgent lookTarget;
        private float viewRange;

        public CanSeeAgent(IAgent target, IAgent lookTarget, float viewRange) : base(target) {
            this.lookTarget = lookTarget;
            this.viewRange = viewRange;
        }

        protected override bool MyCondition() {
            float sqrLookTargetDistance = (lookTarget.Position - target.Position).sqrMagnitude;
            return sqrLookTargetDistance < viewRange * viewRange;
        }

        protected override void OnTerminate() {
            base.OnTerminate();
        }
    }
}
