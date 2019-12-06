using System;
using System.Collections.Generic;
using BehaviourTree;

namespace BehaviourTree {
    namespace FragmentCollection {

        public class SetTargetFragment : InstanceBoundBehaviour<IFragmentCollector> {

            public SetTargetFragment(IFragmentCollector target) : base(target) {

            }

            protected override void OnInitialize() {
                base.OnInitialize();

                target.SetTargetFragment(GetTargetFragment());
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
}