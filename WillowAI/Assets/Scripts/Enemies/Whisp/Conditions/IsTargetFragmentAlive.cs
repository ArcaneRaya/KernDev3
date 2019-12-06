using System;
namespace WhispConditions {
    public class IsTargetFragmentAlive : InstanceBoundCondition<IFragmentCollector> {
        public IsTargetFragmentAlive(IFragmentCollector target) : base(target) {
        }

        protected override bool MyCondition() {
            return target.TargetFragment != null;
        }
    }
}
