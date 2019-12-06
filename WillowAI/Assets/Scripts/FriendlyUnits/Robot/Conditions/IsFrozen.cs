using UnityEngine;
using System.Collections;

public class IsFrozen : InstanceBoundCondition<Robot> {

    public IsFrozen(Robot target) : base(target) {
    }

    protected override bool MyCondition() {
        return target.IsFrozen;
    }
}
