namespace BehaviourTree {

    public abstract class Composite : Node {

        protected readonly Node[] nodes;

        public Composite(params Node[] nodes) {
            foreach (var node in nodes) {
                if (node == null) {
                    throw new System.ArgumentException("Node cannot be null");
                }
            }
            this.nodes = nodes;
        }
    }
}