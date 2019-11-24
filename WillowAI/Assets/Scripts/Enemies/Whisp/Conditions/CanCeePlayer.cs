using System;
using UnityEngine;

namespace WhispConditions {
    public class CanSeePlayer : InstanceBoundCondition<Whisp> {

        private Player player = null;

        public CanSeePlayer(Whisp target) : base(target) {
        }

        protected override bool MyCondition() {
            player = player ?? (MainController.GetControllerOfType(typeof(PlayerController)) as PlayerController).Player;

            float sqrPlayerDistance = (player.Position - target.Position).sqrMagnitude;
            return sqrPlayerDistance < target.PlayerViewRange * target.PlayerViewRange;
        }

        protected override void OnTerminate() {
            base.OnTerminate();
            player = null;
        }
    }
}
