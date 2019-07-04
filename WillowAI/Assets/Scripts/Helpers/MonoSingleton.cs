using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T> {

    public static MonoSingleton<T> Instance {
        get {
            if (internalInstance == null) {
                internalInstance = GetInstance();
            }
            return internalInstance;
        }
    }

    private static MonoSingleton<T> internalInstance;

    private static MonoSingleton<T> GetInstance() {
        MonoSingleton<T>[] potentialInstances = FindObjectsOfType<MonoSingleton<T>>();
        if (potentialInstances.Length == 0) {
            throw new Exception("Could not find instance of " + typeof(MonoSingleton<T>));
        }
        if (potentialInstances.Length > 1) {
            throw new Exception("Multiple instances exist of " + typeof(MonoSingleton<T>));
        }
        return potentialInstances[0];
    }
}
