using System;
using BehaviourTree;

namespace BehaviourTree {
    namespace FragmentCollection {

        public class IsTargetFragmentAlive : InstanceBoundCondition<IFragmentCollector> {
            public IsTargetFragmentAlive(IFragmentCollector target) : base(target) {
            }

            protected override bool MyCondition() {
                return target.TargetFragment != null;
            }
        }
    }
}