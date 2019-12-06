namespace BehaviourTree {

    public class Selector : Composite {

        protected int currentSubNode = 0;

        public Selector(params Node[] nodes) : base(nodes) {

        }

        protected override void OnInitialize() {
            currentSubNode = 0;
        }

        public override NodeStates Evaluate(float deltaTime) {
            while (true) {
                // initialize subnode if it has not yet been initialized
                if (nodes[currentSubNode].CurrentNodeState == NodeStates.INVALID) {
                    nodes[currentSubNode].Initialize();
                }
                NodeStates currentSubNodeState = nodes[currentSubNode].Evaluate(deltaTime);

                switch (currentSubNodeState) {
                    case NodeStates.RUNNING:
                        currentNodeState = currentSubNodeState;
                        return currentNodeState;
                    case NodeStates.SUCCESS:
                        currentNodeState = currentSubNodeState;
                        return NodeStates.SUCCESS;
                    case NodeStates.FAILURE:
                        // if ran through all subnodes in sequence, return failure
                        if (currentSubNode + 1 == nodes.Length) {
                            currentNodeState = currentSubNodeState;
                            return NodeStates.FAILURE;
                        } else {
                            nodes[currentSubNode].Terminate();
                            currentSubNode++;
                        }
                        break;
                    case NodeStates.INVALID:
                        throw new System.Exception("This should never happen!");
                }
            }
        }

        protected override void OnTerminate() {
            if (nodes[currentSubNode].CurrentNodeState != NodeStates.INVALID) {
                nodes[currentSubNode].Terminate();
            }
        }
    }
}