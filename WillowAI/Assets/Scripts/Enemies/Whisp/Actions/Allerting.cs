using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WhispActions {
    public class Allerting : InstanceBoundBehaviour<Whisp> {

        private Player player;

        public Allerting(Whisp target) : base(target) {
        }

        protected override NodeStates MyAction(float deltaTime) {
            if (player == null) {
                player = (MainController.Instance.GetControllerOfType(typeof(PlayerController)) as PlayerController).Player;
            }

            bool isPlayerWithinViewRange = (player.Position - target.Position).sqrMagnitude < target.PlayerViewRange * target.PlayerViewRange;
            if (isPlayerWithinViewRange == false) {
                return NodeStates.FAILURE;
            }

            if (target.PathFindingAgent.IsMoving) {
                target.PathFindingAgent.Stop();
            }

            return NodeStates.RUNNING;
        }

        protected override void OnTerminate() {
            base.OnTerminate();
            player = null;
        }
    }
}