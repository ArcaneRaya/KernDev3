using UnityEngine;
using System.Collections;
using BehaviourTree;

namespace BehaviourTree {
    namespace BehaviourRobot {
        public class SetFrozen : InstanceBoundBehaviour<Robot> {
            public SetFrozen(Robot target) : base(target) {
            }

            protected override void OnInitialize() {
                base.OnInitialize();
                target.SetFrozen();
                currentNodeState = NodeStates.SUCCESS;
            }

            protected override NodeStates MyAction(float deltaTime) {
                return currentNodeState;
            }
        }
    }
}