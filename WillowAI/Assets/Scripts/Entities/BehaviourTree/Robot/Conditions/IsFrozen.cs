using UnityEngine;
using System.Collections;
using BehaviourTree;

namespace BehaviourTree {
    namespace BehaviourRobot {

        public class IsFrozen : InstanceBoundCondition<Robot> {

            public IsFrozen(Robot target) : base(target) {
            }

            protected override bool MyCondition() {
                return target.IsFrozen;
            }
        }
    }
}
