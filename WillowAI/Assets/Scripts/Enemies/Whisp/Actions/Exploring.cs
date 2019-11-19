﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WhispActions {
    public class Exploring : InstanceBoundBehaviour<Whisp> {

        private Vector3 targetPosition = Vector3.zero;
        private bool isTargetSet = false;
        private float timeSinceLastExploration;
        private float targetWaitTime;
        private float minWaitTime = 0.4f;
        private float maxWaitTime = 5;

        public Exploring(Whisp target) : base(target) {
            ResetWaitTime();
        }

        protected override NodeStates MyAction(float deltaTime) {
            if (timeSinceLastExploration + targetWaitTime > MainController.Instance.GameTime) {
                return NodeStates.FAILURE;
            }

            if (isTargetSet) {
                return NodeStates.RUNNING;
            }

            targetPosition = new Vector3(UnityEngine.Random.Range(-target.distanceWalk, target.distanceWalk), target.transform.position.y, UnityEngine.Random.Range(-target.distanceWalk, target.distanceWalk));
            target.PathFindingAgent.MoveTowards(targetPosition);
            target.PathFindingAgent.OnDestinationReachedAction += OnDestinationReached;
            isTargetSet = true;
            return NodeStates.RUNNING;
        }

        private void OnDestinationReached() {
            target.PathFindingAgent.OnDestinationReachedAction -= OnDestinationReached;
            isTargetSet = false;
            ResetWaitTime();
        }

        private void ResetWaitTime() {
            timeSinceLastExploration = MainController.Instance.GameTime;
            targetWaitTime = UnityEngine.Random.Range(minWaitTime, maxWaitTime);
        }

        protected override void OnTerminate() {
            base.OnTerminate();
            if (isTargetSet) {
                target.PathFindingAgent.OnDestinationReachedAction -= OnDestinationReached;
                isTargetSet = false;
            }
        }
    }
}