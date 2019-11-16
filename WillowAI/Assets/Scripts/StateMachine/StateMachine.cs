using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T> {
    public T Target { get; private set; }
    public BaseState<T> CurrentState { get; private set; }

    public StateMachine() {

    }

    public void Tick(float deltaTime) {
        if (CurrentState != null) {
            CurrentState.Tick(deltaTime);
        }
    }

    public void GoToState(BaseState<T> state) {
        if (CurrentState != null) {
            CurrentState.Stop();
        }
        CurrentState = state;
        state.Start();
    }
}
