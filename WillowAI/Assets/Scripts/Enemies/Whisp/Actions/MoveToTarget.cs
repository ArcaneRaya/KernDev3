using System;
namespace WhispActions {
    public class MoveToTarget : InstanceBoundBehaviour<Whisp> {
        private bool reachedDestination;

        public MoveToTarget(Whisp target) : base(target) {
        }

        protected override void OnInitialize() {
            base.OnInitialize();
            reachedDestination = false;
            currentNodeState = NodeStates.RUNNING;
            target.PathFindingAgent.OnDestinationReachedAction += OnDestinationReached;
            target.PathFindingAgent.MoveTowards(target.TargetMovePosition, this);
        }

        private void OnDestinationReached() {
            reachedDestination = true;
            target.PathFindingAgent.OnDestinationReachedAction -= OnDestinationReached;
            currentNodeState = NodeStates.SUCCESS;
        }

        protected override NodeStates MyAction(float deltaTime) {
            target.SetLastMoveTimeToNow();
            return CurrentNodeState;
        }

        protected override void OnTerminate() {
            base.OnTerminate();
            if (reachedDestination == false) {
                target.PathFindingAgent.OnDestinationReachedAction -= OnDestinationReached;
                if (target.PathFindingAgent.OrderedBy == this) {
                    target.PathFindingAgent.Stop();
                }
            }
        }
    }
}
