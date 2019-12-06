using System;
namespace WhispConditions {
    public class IsFragmentNear : InstanceBoundCondition<IFragmentCollector> {

        public IsFragmentNear(IFragmentCollector target) : base(target) {
        }

        protected override bool MyCondition() {
            return target.FragmentController.GetFragmentsInRange(target.Position, target.FragmentViewRange).Count > 0;
        }
    }
}

public interface IFragmentCollector : IAgent {
    FragmentController FragmentController { get; }
    float FragmentViewRange { get; }
    Fragment TargetFragment { get; }
    float FragmentPickupTime { get; }
    int FragmentsInPosessionCount { get; }

    void SetTargetFragment(Fragment fragment);
    void IncrementPickupCount();
}