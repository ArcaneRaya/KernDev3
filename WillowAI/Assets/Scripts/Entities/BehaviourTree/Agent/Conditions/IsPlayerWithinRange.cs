using System;
using UnityEngine;
using BehaviourTree;

namespace BehaviourTree {
    namespace BehaviourAgent {

        public class IsPlayerWithinRange : InstanceBoundCondition<IAgent> {

            private Player player;
            private float range;

            public IsPlayerWithinRange(IAgent target, float range) : base(target) {
                this.range = range;
            }

            protected override bool MyCondition() {
                player = player ?? (MainController.GetControllerOfType(typeof(PlayerController)) as PlayerController).Player;

                float sqrPlayerDistance = (player.Position - target.Position).sqrMagnitude;
                return sqrPlayerDistance <= range * range;
            }

            protected override void OnTerminate() {
                base.OnTerminate();
                player = null;
            }
        }
    }
}