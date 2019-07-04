using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour {
    [SerializeField] private List<BaseController> controllers = new List<BaseController>();
    [SerializeField] private GameObject pausedUIOverlay = null;

    private bool isGameRunning;

    public void StartGame() {
        isGameRunning = true;
        pausedUIOverlay.SetActive(false);
    }

    public void PauseGame() {
        isGameRunning = false;
        pausedUIOverlay.SetActive(true);
    }

    public BaseController GetControllerOfType(Type type) {
        if (type.IsSubclassOf(typeof(BaseController)) == false) {
            throw new ArgumentException("Requested type has to be subclass of type: " + typeof(BaseController));
        }

        foreach (BaseController controller in controllers) {
            if (controller.GetType() == type) {
                return controller;
            }
        }

        throw new ArgumentException("Could not find controller of type: " + type);
    }

    private void Awake() {
        isGameRunning = false;
        foreach (BaseController controller in controllers) {
            controller.Initialize(this);
        }
    }

    private void Start() {
        foreach (BaseController controller in controllers) {
            controller.Setup();
        }
    }

    private void Update() {
        HandleInput();

        if (isGameRunning == false) { return; }

        foreach (BaseController controller in controllers) {
            controller.Tick(Time.deltaTime);
        }
    }

    private void HandleInput() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isGameRunning) {
                PauseGame();
            }
            else {
                StartGame();
            }
        }
    }
}
