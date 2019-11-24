using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour {
    public void Initialize(MainController mainController) {
        OnInitialize(mainController);
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

    protected abstract void OnInitialize(MainController mainController);
    protected abstract void OnSetup();
    protected abstract void OnTick(float deltaTime);
    protected abstract void OnTerminate();
}
