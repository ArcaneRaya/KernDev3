using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WhispActions {
    public class Fleeing : ActionNode<Whisp> {
        public Fleeing(Whisp target) : base(target) {
        }

        public override NodeStates MyAction(Whisp target, float deltaTime) {
            throw new System.NotImplementedException();
        }

        public override void CancelNode() {
            throw new System.NotImplementedException();
        }
    }
}
