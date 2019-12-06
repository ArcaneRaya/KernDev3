using UnityEngine;
using System.Collections;
using BehaviourTree;

namespace BehaviourTree {
    namespace BehaviourRobot {

        public class IsPlayerCloserThanWhisps : InstanceBoundCondition<IAgent> {

            private Player player;
            private EnemyController enemyController;
            private float viewRange;

            public IsPlayerCloserThanWhisps(IAgent target, float viewRange) : base(target) {
                player = (MainController.GetControllerOfType(typeof(PlayerController)) as PlayerController).Player;
                enemyController = MainController.GetControllerOfType(typeof(EnemyController)) as EnemyController;
                this.viewRange = viewRange;
            }

            protected override bool MyCondition() {
                float distanceSqrToPlayer = (player.Position - target.Position).sqrMagnitude;
                float distanceSqrToWhisp = float.MaxValue;
                foreach (var whisp in enemyController.GetEnemiesInRange(target.Position, viewRange)) {
                    distanceSqrToWhisp = (whisp.Position - target.Position).sqrMagnitude;
                    if (distanceSqrToWhisp < distanceSqrToPlayer) {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}