

public class BehaviourTree {
    private Node root;

    public BehaviourTree(Node root) {
        this.root = root;
    }

    public void Tick(float deltaTime) {
        if (root.CurrentNodeState == NodeStates.INVALID) {
            root.Initialize();
        }
        root.Evaluate(deltaTime);
        if (root.CurrentNodeState != NodeStates.RUNNING) {
            root.Terminate();
        }
    }
}