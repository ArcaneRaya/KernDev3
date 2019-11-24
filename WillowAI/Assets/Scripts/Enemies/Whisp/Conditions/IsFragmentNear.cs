using System;
namespace WhispConditions {
    public class IsFragmentNear : InstanceBoundCondition<Whisp> {

        public IsFragmentNear(Whisp target) : base(target) {
        }

        protected override bool MyCondition() {
            return target.FragmentController.GetFragmentsInRange(target.Position, target.FragmentViewRange).Count > 0;
        }
    }
}
