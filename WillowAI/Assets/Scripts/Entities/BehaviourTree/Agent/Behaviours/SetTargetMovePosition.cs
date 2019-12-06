using System;
using UnityEngine;
using BehaviourTree;

namespace BehaviourTree {

    namespace BehaviourAgent {

        public class SetTargetMovePosition : InstanceBoundBehaviour<IBehaviourAgent> {
            private Func<IBehaviourAgent, Vector3> func;

            public SetTargetMovePosition(IBehaviourAgent target, Func<IAgent, Vector3> func) : base(target) {
                this.func = func;
            }

            protected override void OnInitialize() {
                base.OnInitialize();

                target.SetTargetMovePosition(func.Invoke(target));
                currentNodeState = NodeStates.SUCCESS;
            }

            protected override NodeStates MyAction(float deltaTime) {
                return currentNodeState;
            }
        }

        public static partial class Helpers {
            public static Vector3 AwayFromPlayer(IAgent target) {
                Player player = (MainController.GetControllerOfType(typeof(PlayerController)) as PlayerController).Player;
                return target.Position + (player.Position - target.Position).normalized * -1;
            }

            public static Vector3 PlayerPosition(IAgent target) {
                return (MainController.GetControllerOfType(typeof(PlayerController)) as PlayerController).Player.Position;
            }

            public static Vector3 RandomPosition(IAgent target) {
                Vector3 randomPosition = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f)).normalized;
                return target.Position + randomPosition * 5;
            }

            /// <summary>
            /// Only use for fragmentcollector
            /// </summary>
            /// <returns>The fragment target position</returns>
            /// <param name="target">Target.</param>
            public static Vector3 CurrentFragmentTarget(IAgent target) {
                return (target as IFragmentCollector).TargetFragment.transform.position;
            }
        }
    }
}