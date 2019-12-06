namespace BehaviourTree {

    public class ActiveSelector : Selector {

        public ActiveSelector(params Node[] nodes) : base(nodes) {
        }

        public override NodeStates Evaluate(float deltaTime) {
            int previousSubNode = currentSubNode;
            base.OnInitialize();
            NodeStates result = base.Evaluate(deltaTime);
            if (currentSubNode < previousSubNode) {
                nodes[previousSubNode].Terminate();
            }
            return result;
        }
    }
}