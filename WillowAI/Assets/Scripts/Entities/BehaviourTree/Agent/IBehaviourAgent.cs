using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBehaviourAgent : IAgent {
    Vector3 TargetLookPosition { get; }
    Vector3 TargetMovePosition { get; }
    float LastMoveTime { get; }

    void SetTargetMovePosition(Vector3 position);
    void SetLastMoveTimeToNow();
    void SetTargetLookPosition(Vector3 vector3);
}
