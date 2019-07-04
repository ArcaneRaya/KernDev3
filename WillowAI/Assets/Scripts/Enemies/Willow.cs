using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Willow : MonoBehaviour {

    private Node Root;

    public void Initialize() {
        Selector rootSelector = new Selector(new List<Node>() { });
        Sequence rootSequence = new Sequence(new List<Node>() { });

        Root = rootSequence;
    }

    public void Setup() {

    }

    public void Tick(float deltaTime) {
        Root.Evaluate();
    }
}
