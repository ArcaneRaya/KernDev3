using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour {
    public void Initialize() {
        OnInitialize();
    }

    public void Setup() {
        OnSetup();
    }

    public void Tick(float deltaTime) {
        OnTick(deltaTime);
    }

    public void Terminate() {
        OnTerminate();
    }

    protected abstract void OnInitialize();
    protected abstract void OnSetup();
    protected abstract void OnTick(float deltaTime);
    protected abstract void OnTerminate();
}
