using UnityEngine;
using System.Collections;

namespace BehaviourTree {
    namespace Generic {

        public class DoNothing : Behaviour {

            protected override void OnInitialize() {
                currentNodeState = NodeStates.RUNNING;
            }

            protected override NodeStates MyAction(float deltaTime) {
                return currentNodeState;
            }
        }
    }

}