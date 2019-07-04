using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T> {

    public static T Instance {
        get {
            if (internalInstance == null) {
                internalInstance = GetInstance();
            }
            return internalInstance;
        }
    }

    private static T internalInstance;

    private static T GetInstance() {
        T[] potentialInstances = FindObjectsOfType<T>();
        if (potentialInstances.Length == 0) {
            throw new Exception("Could not find instance of " + typeof(MonoSingleton<T>));
        }
        if (potentialInstances.Length > 1) {
            throw new Exception("Multiple instances exist of " + typeof(MonoSingleton<T>));
        }
        return potentialInstances[0];
    }
}
