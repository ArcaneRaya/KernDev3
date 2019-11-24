using System;
namespace WhispConditions {
    public class HasMovedRecently : InstanceBoundCondition<Whisp> {

        public HasMovedRecently(Whisp target) : base(target) {
        }

        protected override bool MyCondition() {
            return MainController.Instance.GameTime - target.LastMoveTime < 5;
        }
    }
}
