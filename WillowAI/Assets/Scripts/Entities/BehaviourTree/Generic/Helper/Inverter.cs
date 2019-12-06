using System;

namespace BehaviourTree {
    namespace Generic {

        public class Inverter : Decorator {

            public Inverter(Node decoratedNode) : base(decoratedNode) {
            }

            protected override NodeStates MyAction(NodeStates nodeState) {
                switch (nodeState) {
                    case NodeStates.FAILURE:
                        return NodeStates.SUCCESS;
                    case NodeStates.SUCCESS:
                        return NodeStates.FAILURE;
                    default:
                        throw new System.Exception("This should never happen!");
                }
            }
        }
    }
}