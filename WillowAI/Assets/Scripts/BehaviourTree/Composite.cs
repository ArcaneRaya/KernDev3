using UnityEngine;
using System.Collections;

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

    public override void Initialize() {
        foreach (Node node in nodes) {
            node.Initialize();
        }
    }

    public override void Terminate() {
        base.Terminate();
        foreach (Node node in nodes) {
            node.Terminate();
        }
    }

}
