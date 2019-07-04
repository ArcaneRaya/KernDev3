using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WhispActions {
    public class Exploring : ActionNode<Whisp> {

        private Vector3 targetPosition = Vector3.zero;
        private bool targetSet = false;
        private float timeWaiting;
        private float targetWaitTime;
        private float minWaitTime = 0.4f;
        private float maxWaitTime = 5;

        public Exploring(Whisp target) : base(target) {
            ResetWaitTime();
        }

        public override NodeStates MyAction(Whisp target, float deltaTime) {
            timeWaiting += deltaTime;

            if (timeWaiting < targetWaitTime) {
                return NodeStates.FAILURE;
            }

            if (targetSet) {
                return NodeStates.RUNNING;
            }

            targetPosition = new Vector3(UnityEngine.Random.Range(-target.distanceWalk, target.distanceWalk), target.transform.position.y, UnityEngine.Random.Range(-target.distanceWalk, target.distanceWalk));
            target.PathFindingAgent.MoveTowards(targetPosition);
            target.PathFindingAgent.OnDestinationReachedAction += OnDestinationReached;
            targetSet = true;
            return NodeStates.SUCCESS;
        }

        private void OnDestinationReached() {
            target.PathFindingAgent.OnDestinationReachedAction -= OnDestinationReached;
            targetSet = false;
            ResetWaitTime();
        }

        private void ResetWaitTime() {
            timeWaiting = 0;
            targetWaitTime = UnityEngine.Random.Range(minWaitTime, maxWaitTime);
        }
    }
}