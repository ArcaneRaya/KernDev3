using System;
using BehaviourTree;

namespace BehaviourTree {
    namespace BehaviourAgent {

        public class HasMovedRecently : InstanceBoundCondition<IBehaviourAgent> {

            public HasMovedRecently(IBehaviourAgent target) : base(target) {
            }

            protected override bool MyCondition() {
                return MainController.Instance.GameTime - target.LastMoveTime < 5;
            }
        }
    }
}