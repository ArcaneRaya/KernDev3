using System;
namespace WhispActions {
    public class WaitRandom : InstanceBoundBehaviour<Whisp> {

        private float startTime;
        private float waitTime;

        public WaitRandom(Whisp target) : base(target) {
        }

        protected override void OnInitialize() {
            base.OnInitialize();
            startTime = MainController.Instance.GameTime;
            waitTime = UnityEngine.Random.Range(1, 5f);
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
