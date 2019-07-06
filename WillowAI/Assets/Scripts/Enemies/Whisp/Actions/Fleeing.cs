using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WhispActions {
    public class Fleeing : ActionNode<Whisp> {

        private bool isFleeingFromPlayer;

        public Fleeing(Whisp target) : base(target) {
        }

        public override NodeStates MyAction(Whisp target, float deltaTime) {
            if (isFleeingFromPlayer) {
                return NodeStates.RUNNING;
            }

            float sqrPlayerDistance = (Player.Instance.Position - target.Position).sqrMagnitude;
            if (sqrPlayerDistance > target.PlayerFleeRange * target.PlayerFleeRange) {
                return NodeStates.FAILURE;
            }

            StartFleeing();
            return NodeStates.SUCCESS;
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

        public override void CancelNode() {
            if (isFleeingFromPlayer) {
                target.PathFindingAgent.OnDestinationReachedAction -= OnDestinationReached;
                isFleeingFromPlayer = false;
            }
        }
    }
}
