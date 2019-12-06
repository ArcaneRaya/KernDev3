using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CanSeeWhisp : InstanceBoundCondition<IAgent> {
    private readonly float viewRange;
    private EnemyController enemyController;

    public CanSeeWhisp(IAgent target, float viewRange) : base(target) {
        this.viewRange = viewRange;
        enemyController = MainController.GetControllerOfType(typeof(EnemyController)) as EnemyController;
    }

    protected override bool MyCondition() {
        List<Whisp> whispsWithinRange = enemyController.GetEnemiesInRange(target.Position, viewRange);
        foreach (var whisp in whispsWithinRange) {
            if (InstanceVisible(whisp)) {
                return true;
            }
        }
        return false;
    }

    private bool InstanceVisible(Whisp whisp) {
        return true;
    }
}
