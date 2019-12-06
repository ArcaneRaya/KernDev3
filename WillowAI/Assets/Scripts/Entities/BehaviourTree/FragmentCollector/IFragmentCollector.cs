using UnityEngine;

public interface IFragmentCollector : IAgent {
    FragmentController FragmentController { get; }
    float FragmentViewRange { get; }
    Fragment TargetFragment { get; }
    float FragmentPickupTime { get; }
    int FragmentsInPosessionCount { get; }

    void SetTargetFragment(Fragment fragment);
    void IncrementPickupCount();
}