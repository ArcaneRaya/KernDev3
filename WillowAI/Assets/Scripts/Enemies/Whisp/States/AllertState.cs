using UnityEngine;
using System.Collections;

public class AllertState : BaseState<Whisp> {
    public AllertState(Whisp target) : base(target) {
    }

    protected override void OnStart() {
        if (Target.PathFindingAgent.IsMoving) {
            Target.PathFindingAgent.Stop();
        }
    }

    protected override void OnTick(float deltaTime) {
        bool isPlayerWithinViewRange = (Player.Instance.Position - Target.Position).sqrMagnitude < Target.PlayerViewRange * Target.PlayerViewRange;
        if (isPlayerWithinViewRange == false) {
            //Target.StateMachine.GoToState(new ExploreState(Target));
        }
    }

    protected override void OnStop() {
    }
}
