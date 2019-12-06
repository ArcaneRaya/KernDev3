using System;
using BehaviourTree;

namespace BehaviourTree {
    namespace FragmentCollection {

        public class IsFragmentNear : InstanceBoundCondition<IFragmentCollector> {

            public IsFragmentNear(IFragmentCollector target) : base(target) {
            }

            protected override bool MyCondition() {
                return target.FragmentController.GetFragmentsInRange(target.Position, target.FragmentViewRange).Count > 0;
            }
        }
    }
}