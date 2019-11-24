using System;
namespace WhispConditions {
    public class IsTargetFragmentAlive : InstanceBoundCondition<Whisp> {
        public IsTargetFragmentAlive(Whisp target) : base(target) {
        }

        protected override bool MyCondition() {
            return target.TargetFragment != null;
        }
    }
}
