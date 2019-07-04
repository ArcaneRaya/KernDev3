using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Fragment : MonoBehaviour {

    public Action<Fragment> OnPickedUpAction;

    public void Pickup() {
        if (OnPickedUpAction != null) {
            OnPickedUpAction(this);
        }
        Destroy(gameObject);
    }
}
