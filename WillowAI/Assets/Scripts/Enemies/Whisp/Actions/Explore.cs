using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WhispActions {
    public class Explore : InstanceBoundBehaviour<Whisp> {

        public bool IsCollectingFragment { get; private set; }
        private bool isWithinFragmentRange = false;
        private float timeWaiting;
        private Fragment targetFragment;

        public Explore(Whisp target) : base(target) {
        }

        protected override NodeStates MyAction(float deltaTime) {
            if (IsCollectingFragment == false) {
                List<Fragment> fragmentsInRange = target.FragmentController.GetFragmentsInRange(target.Position, target.FragmentViewRange);
                if (fragmentsInRange.Count == 0) {
                    return NodeStates.FAILURE;
                } else {
                    StartCollecting(fragmentsInRange[UnityEngine.Random.Range(0, fragmentsInRange.Count)]);
                    return NodeStates.RUNNING;
                }
            }

            if (isWithinFragmentRange) {
                timeWaiting += deltaTime;
                if (timeWaiting >= target.FragmentPickupTime) {
                    IsCollectingFragment = false;
                    isWithinFragmentRange = false;
                    timeWaiting = 0;
                    return NodeStates.SUCCESS;
                }
            }
            return NodeStates.RUNNING;
        }

        private void StartCollecting(Fragment fragment) {
            targetFragment = fragment;
            isWithinFragmentRange = false;
            timeWaiting = 0;
            target.PathFindingAgent.MoveTowards(fragment.transform.position, this);
            target.PathFindingAgent.OnDestinationReachedAction += OnDestinationReached;
            IsCollectingFragment = true;
        }

        private void OnDestinationReached() {
            target.PathFindingAgent.OnDestinationReachedAction -= OnDestinationReached;
            isWithinFragmentRange = true;
            targetFragment.Pickup();
            targetFragment = null;
        }

        protected override void OnTerminate() {
            base.OnTerminate();
            if (IsCollectingFragment && isWithinFragmentRange == false) {
                target.PathFindingAgent.OnDestinationReachedAction -= OnDestinationReached;
            }
            IsCollectingFragment = false;
            isWithinFragmentRange = false;
        }
    }
}