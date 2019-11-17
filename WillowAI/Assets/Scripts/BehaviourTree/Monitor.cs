
public class Monitor : Composite {
    private readonly Node nodeToExecute;

    public Monitor(Node nodeToExecute, params Condition[] conditions) : base(conditions) {
        this.nodeToExecute = nodeToExecute ?? throw new System.ArgumentException("Node cannot be null");
    }

    protected override void OnInitialize() {
        nodeToExecute.Initialize();
    }

    public override NodeStates Evaluate(float deltaTime) {
        foreach (var condition in nodes) {
            if (condition.CurrentNodeState == NodeStates.INVALID) {
                condition.Initialize();
            }
            switch (condition.Evaluate(deltaTime)) {
                case NodeStates.FAILURE:
                    currentNodeState = NodeStates.FAILURE;
                    return currentNodeState;
                case NodeStates.SUCCESS:
                    break;
                case NodeStates.RUNNING:
                case NodeStates.INVALID:
                    throw new System.Exception("This should never happen!");
            }
            condition.Terminate();
        }
        currentNodeState = nodeToExecute.Evaluate(deltaTime);
        return currentNodeState;
    }

    protected override void OnTerminate() {
        nodeToExecute.Terminate();
    }
}