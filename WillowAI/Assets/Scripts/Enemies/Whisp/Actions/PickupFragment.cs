using System;
namespace WhispActions {
    public class PickupFragment : InstanceBoundBehaviour<IFragmentCollector> {

        float timePickingUp;

        public PickupFragment(IFragmentCollector target) : base(target) {
        }

        protected override void OnInitialize() {
            base.OnInitialize();
            timePickingUp = 0;
            currentNodeState = NodeStates.RUNNING;
        }

        protected override NodeStates MyAction(float deltaTime) {
            timePickingUp += deltaTime;
            if (timePickingUp < target.FragmentPickupTime) {
                currentNodeState = NodeStates.RUNNING;
            } else {
                target.IncrementPickupCount();
                target.TargetFragment.Pickup();
                currentNodeState = NodeStates.SUCCESS;
            }
            return currentNodeState;
        }
    }
}
