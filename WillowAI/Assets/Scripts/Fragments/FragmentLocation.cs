using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentLocation : MonoBehaviour {
    public Transform FragmentContainer {
        get {
            return fragmentContainer;
        }
    }

    [SerializeField] private Transform fragmentContainer = null;
}
