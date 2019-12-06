using System;

namespace BehaviourTree {
    namespace Generic {

        public class WaitRandom : Behaviour {

            private float startTime;
            private float waitTime;
            private readonly float minTime;
            private readonly float maxTime;

            public WaitRandom(float minTime, float maxTime) {
                this.minTime = minTime;
                this.maxTime = maxTime;
            }

            protected override void OnInitialize() {
                base.OnInitialize();
                startTime = MainController.Instance.GameTime;
                waitTime = UnityEngine.Random.Range(minTime, maxTime);
                currentNodeState = NodeStates.RUNNING;
            }

            protected override NodeStates MyAction(float deltaTime) {
                if (startTime + waitTime < MainController.Instance.GameTime) {
                    currentNodeState = NodeStates.SUCCESS;
                }
                return currentNodeState;
            }
        }
    }
}