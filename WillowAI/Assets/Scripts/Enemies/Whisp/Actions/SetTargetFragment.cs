using System;
using System.Collections.Generic;

namespace WhispActions {
    public class SetTargetFragment : InstanceBoundBehaviour<Whisp> {

        public SetTargetFragment(Whisp target) : base(target) {

        }

        protected override void OnInitialize() {
            base.OnInitialize();

            target.TargetFragment = GetTargetFragment();
            if (target.TargetFragment == null) {
                currentNodeState = NodeStates.FAILURE;
            } else {
                currentNodeState = NodeStates.SUCCESS;
            }
        }

        protected override NodeStates MyAction(float deltaTime) {
            return currentNodeState;
        }

        private Fragment GetTargetFragment() {
            List<Fragment> potentialTargets = target.FragmentController.GetFragmentsInRange(target.Position, target.FragmentViewRange);
            if (potentialTargets.Count == 0) {
                return null;
            } else if (potentialTargets.Count == 1) {
                return potentialTargets[0];
            }

            potentialTargets.Sort((lhs, rhs) => {
                float lhsSqrDist = (lhs.transform.position - target.Position).sqrMagnitude;
                float rhsSqrDist = (rhs.transform.position - target.Position).sqrMagnitude;
                return lhsSqrDist.CompareTo(rhsSqrDist);
            });
            return potentialTargets[0];
        }
    }
}
