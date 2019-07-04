using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Selector : Node {
    /** The child nodes for this selector */
    protected List<Node> m_nodes = new List<Node>();


    /** The constructor requires a lsit of child nodes to be  
     * passed in*/
    public Selector(List<Node> nodes) {
        m_nodes = nodes;
    }

    /* If any of the children reports a success, the selector will 
     * immediately report a success upwards. If all children fail, 
     * it will report a failure instead.*/
    public override NodeStates Evaluate(float deltaTime) {
        m_nodeState = NodeStates.FAILURE;
        foreach (Node node in m_nodes) {
            if (m_nodeState == NodeStates.FAILURE) {
                m_nodeState = node.Evaluate(deltaTime);
            }
            else {
                node.CancelNode();
            }
        }
        return m_nodeState;
    }

    public override void CancelNode() {
        foreach (Node node in m_nodes) {
            node.CancelNode();
        }
    }
}