using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WhispActions {
    public class Fleeing : InstanceBoundBehaviour<Whisp> {

        private bool isFleeingFromPlayer;

        public Fleeing(Whisp target) : base(target) {
        }

        protected override NodeStates MyAction(float deltaTime) {
            if (isFleeingFromPlayer) {
                return NodeStates.RUNNING;
            }

            float sqrPlayerDistance = (Player.Instance.Position - target.Position).sqrMagnitude;
            if (sqrPlayerDistance > target.PlayerFleeRange * target.PlayerFleeRange) {
                return NodeStates.FAILURE;
            }

            StartFleeing();
            return NodeStates.RUNNING;
        }

        private void StartFleeing() {
            Vector3 targetDirection = (Player.Instance.Position - target.Position).normalized * -1;
            target.PathFindingAgent.MoveTowards(target.Position + targetDirection * target.Speed);
            target.PathFindingAgent.OnDestinationReachedAction += OnDestinationReached;
        }

        private void OnDestinationReached() {
            target.PathFindingAgent.OnDestinationReachedAction -= OnDestinationReached;
            isFleeingFromPlayer = false;
        }

        protected override void OnTerminate() {
            base.OnTerminate();
            if (isFleeingFromPlayer) {
                target.PathFindingAgent.OnDestinationReachedAction -= OnDestinationReached;
                isFleeingFromPlayer = false;
            }
        }
    }
}
