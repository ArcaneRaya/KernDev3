using System;
using UnityEngine;
using BehaviourTree;

namespace BehaviourTree {
    namespace BehaviourAgent {

        public class SetTargetLookPosition : InstanceBoundBehaviour<IBehaviourAgent> {
            private Func<IAgent, Vector3> func;

            public SetTargetLookPosition(IBehaviourAgent target, Func<IAgent, Vector3> func) : base(target) {
                this.func = func;
            }

            protected override void OnInitialize() {
                base.OnInitialize();

                target.SetTargetLookPosition(func.Invoke(target));
                currentNodeState = NodeStates.SUCCESS;
            }

            protected override NodeStates MyAction(float deltaTime) {
                return currentNodeState;
            }
        }
    }
}
