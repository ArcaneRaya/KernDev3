namespace BehaviourTree {

    public abstract class Decorator : Node {

        protected Node decoratedNode;

        public Decorator(Node decoratedNode) {
            this.decoratedNode = decoratedNode ?? throw new System.ArgumentException("Node cannot be null");
        }

        protected override void OnInitialize() {
            decoratedNode.Initialize();
        }

        public override NodeStates Evaluate(float deltaTime) {
            currentNodeState = MyAction(decoratedNode.Evaluate(deltaTime));
            return currentNodeState;
        }

        protected override void OnTerminate() {
            decoratedNode.Terminate();
        }

        protected abstract NodeStates MyAction(NodeStates nodeState);
    }
}