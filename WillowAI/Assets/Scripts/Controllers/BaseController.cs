using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour {
    public void Initialize(List<BaseController> controllers) {
        OnInitialize(controllers);
    }

    public void Setup() {
        OnSetup();
    }

    public void Tick(float deltaTime) {
        OnTick(deltaTime);
    }

    protected abstract void OnInitialize(List<BaseController> controllers);
    protected abstract void OnSetup();
    protected abstract void OnTick(float deltaTime);
}
