using System;
using UnityEngine;

namespace WhispActions {

    public class SetTargetMovePosition : InstanceBoundBehaviour<Whisp> {
        private Func<Whisp, Vector3> func;

        public SetTargetMovePosition(Whisp target, Func<Whisp, Vector3> func) : base(target) {
            this.func = func;
        }

        protected override void OnInitialize() {
            base.OnInitialize();

            target.TargetMovePosition = func.Invoke(target);
            currentNodeState = NodeStates.SUCCESS;
        }

        protected override NodeStates MyAction(float deltaTime) {
            return currentNodeState;
        }
    }

    public static partial class Helpers {
        public static Vector3 AwayFromPlayer(Whisp target) {
            Player player = (MainController.GetControllerOfType(typeof(PlayerController)) as PlayerController).Player;
            return target.Position + (player.Position - target.Position).normalized * -1;
        }

        public static Vector3 PlayerPosition(Whisp target) {
            return (MainController.GetControllerOfType(typeof(PlayerController)) as PlayerController).Player.Position;
        }

        public static Vector3 RandomPosition(Whisp target) {
            Vector3 randomPosition = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f)).normalized;
            return target.Position + randomPosition * 5;
        }

        public static Vector3 CurrentFragmentTarget(Whisp target) {
            return target.TargetFragment.transform.position;
        }
    }
}