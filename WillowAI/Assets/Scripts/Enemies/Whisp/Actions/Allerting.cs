using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WhispActions {
    public class Allerting : InstanceBoundBehaviour<Whisp> {

        public Allerting(Whisp target) : base(target) {
        }

        protected override NodeStates MyAction(float deltaTime) {
            bool isPlayerWithinViewRange = (Player.Instance.Position - target.Position).sqrMagnitude < target.PlayerViewRange * target.PlayerViewRange;
            if (isPlayerWithinViewRange == false) {
                return NodeStates.FAILURE;
            }

            if (target.PathFindingAgent.IsMoving) {
                target.PathFindingAgent.Stop();
            }

            return NodeStates.RUNNING;
        }
    }
}