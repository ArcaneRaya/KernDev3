using System;

public abstract class Condition : Node {

    public override NodeStates Evaluate(float deltaTime) {
        return MyCondition() ? NodeStates.SUCCESS : NodeStates.FAILURE;
    }

    protected abstract bool MyCondition();
}
