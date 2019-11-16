using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState<T> : BaseState {
    public T Target { get; private set; }
    public bool IsStateActive { get; private set; }
    public float TimeInState { get; private set; }

    public BaseState(T target) {
        Target = target;
    }

    public void Start() {
        IsStateActive = true;
        TimeInState = 0;
        OnStart();
    }

    public void Tick(float deltaTime) {
        TimeInState += deltaTime;
        OnTick(deltaTime);
    }

    public void Stop() {
        IsStateActive = false;
        OnStop();
    }

    protected abstract void OnStart();
    protected abstract void OnTick(float deltaTime);
    protected abstract void OnStop();
}

public abstract class BaseState {

}